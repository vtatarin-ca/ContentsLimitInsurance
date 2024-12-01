using MongoDB.Bson.Serialization.Attributes;

namespace Data.Collection
{
  public class Category
  {
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }
  }
}
