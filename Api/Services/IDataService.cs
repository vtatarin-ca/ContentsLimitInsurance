using Data.Models;

namespace Api.Services
{
  public interface IDataService
  {
    List<ItemModel> GetItems();
    List<CategoryModel> GetCategories();
    ItemModel? GetItemById(string id);
    ItemModel? GetItemByName(string name);
    Task<ItemModel> AddItem(ItemModel model);
    Task DeleteItem(string id);

    CategoryModel? GetCategoryById(string id);
  }
}