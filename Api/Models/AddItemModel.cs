using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
  public class AddItemModel
  {
    [Required]
    public required string Name { get; set; }

    [Required]
    public required decimal Value { get; set; }

    [Required]
    public required string CategoryId { get; set; }
  }
}
