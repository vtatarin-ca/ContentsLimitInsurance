using AutoMapper;
using Data;
using Data.Collection;
using Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Services
{
  public class DataService : IDataService
  {
    private readonly IDocumentRepository<Item> _itemDocumentRepository;
    private readonly IMongoCollection<Item> _itemCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;
    private readonly ILogger<DataService> _logger;

    public DataService(IOptions<DatabaseSettings> settings, IDocumentRepository<Item> itemDocumentRepository, IMapper mapper, ILogger<DataService> logger)
    {
      var client = new MongoClient(settings.Value.ConnectionString);
      var database = client.GetDatabase(settings.Value.Database);
      _itemCollection = database.GetCollection<Item>("Item");
      _categoryCollection = database.GetCollection<Category>("Category");

      _itemDocumentRepository = itemDocumentRepository;
      _mapper = mapper;
      _logger = logger;
    }

    #region Items
    public List<ItemModel> GetItems()
    {
      var result = (from item in _itemCollection.AsQueryable()
                    join category in _categoryCollection.AsQueryable() on item.CategoryId equals category.Id into categoryJoin
                    from category in categoryJoin.DefaultIfEmpty()
                    select new ItemModel
                    {
                      Id = item.Id,
                      Name = item.Name,
                      Value = item.Value,
                      CategoryId = item.CategoryId,
                      CategoryName = category.Name
                    }).ToList();

      return result;
    }

    public ItemModel? GetItemById(string id)
    {
      var item = _itemCollection.Find(x => x.Id == id).FirstOrDefault();
      if (item == null)
      {
        _logger.LogError("{Name}: Cannot find item with id = {Id}", GetType().Name, id);
        return null;
      }

      return FillItemWithCategoryInfo(item);
    }

    public ItemModel? GetItemByName(string name)
    {
      var item = _itemCollection.Find(x => x.Name == name).FirstOrDefault();
      if (item == null)
      {
        _logger.LogError("{Name}: Cannot find item with name = {ItemName}", GetType().Name, name);
        return null;
      }

      return FillItemWithCategoryInfo(item);
    }

    public async Task<ItemModel> AddItem(ItemModel model)
    {
      model.Id = Guid.NewGuid().ToString();
      var item = _mapper.Map<Item>(model);

      await _itemDocumentRepository.CreateItemAsync(item);
      
      return GetItemById(model.Id) ?? new ItemModel();
    }

    public async Task DeleteItem(string id)
    {
      var item = GetItemById(id);

      if (item == null)
      {
        _logger.LogError("{Name}: Cannot find item with id = {Id} to delete it", GetType().Name, id);
        return;
      }

      await _itemDocumentRepository.DeleteItemByIdAsync(id);
    }

    /// <summary>
    /// Private helper to reuse the same part of code to fill ItemModel with Category info (e.g. Category.Name)
    /// </summary>
    /// <param name="item">Item from the DB</param>
    /// <returns>ItemModel</returns>
    private ItemModel? FillItemWithCategoryInfo(Item item)
    {
      var category = _categoryCollection.Find(x => x.Id == item.CategoryId).FirstOrDefault();
      if (category == null)
      {
        _logger.LogError("{Name}: Cannot find category with id = {CategoryId}", GetType().Name, item.CategoryId);
        return null;
      }

      var result = _mapper.Map<ItemModel>(item);
      result.CategoryName = category.Name;

      return result;
    }

    #endregion Items

    #region Getegories
    public List<CategoryModel> GetCategories()
    {
      var result = (from category in _categoryCollection.AsQueryable()
                    select new CategoryModel
                    {
                      Id = category.Id,
                      Name = category.Name
                    }).ToList();

      return result;
    }

    public CategoryModel? GetCategoryById(string id)
    {
      var category = _categoryCollection.Find(x => x.Id == id).FirstOrDefault();
      if (category == null)
      {
        _logger.LogError("{Name}: Cannot find category with id = {Id}", GetType().Name, id);
        return null;
      }

      var result = _mapper.Map<CategoryModel>(category);

      return result;
    }

    #endregion Getegories
  }
}
