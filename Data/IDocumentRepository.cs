namespace Data
{
  public interface IDocumentRepository<T> where T : class
  {
    Task<T> GetItemAsync(string id);
    Task CreateItemAsync(T item);
    Task<List<T>> GetManyItemsAsync(int limit = 100);
    Task<bool> DeleteItemByIdAsync(string id);
  }
}