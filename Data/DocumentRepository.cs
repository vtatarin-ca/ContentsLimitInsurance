using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data
{
  public class DocumentRepository<T> : IDocumentRepository<T> where T : class
  {
    private readonly IMongoCollection<T> _collection;

    public DocumentRepository(IOptions<DatabaseSettings> settings)
    {
      var client = new MongoClient(settings.Value.ConnectionString);
      var _db = client.GetDatabase(settings.Value.Database) ?? throw new ArgumentException("Cannot get DB");
      _collection = _db.GetCollection<T>(typeof(T).Name);
    }

    /// <summary>
    /// Get an item by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<T> GetItemAsync(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("ID cannot be null or empty", nameof(id));

      var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
      return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Create an item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task CreateItemAsync(T item)
    {
      ArgumentNullException.ThrowIfNull(item, nameof(item));

      await _collection.InsertOneAsync(item);
    }

    /// <summary>
    /// Get many items (by default limit to 100)
    /// </summary>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<List<T>> GetManyItemsAsync(int limit = 100)
    {
      return await _collection.Find(Builders<T>.Filter.Empty)
                              .Limit(limit)
                              .ToListAsync();
    }

    /// <summary>
    /// Return "true" if an item was deleted
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<bool> DeleteItemByIdAsync(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("ID cannot be null or empty", nameof(id));

      var filter = Builders<T>.Filter.Eq("_id", id);
      var result = await _collection.DeleteOneAsync(filter);
      return result.DeletedCount > 0;
    }
  }
}
