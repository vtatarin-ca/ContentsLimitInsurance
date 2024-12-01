using MongoDB.Bson.Serialization.Attributes;

namespace Data.Collection
{
  public class Item
  {
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("value")]
    public decimal Value { get; set; }

    [BsonElement("categoryId")]
    public string? CategoryId { get; set; }
  }
}
