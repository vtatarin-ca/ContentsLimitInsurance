using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
  public class AddItemModelRequest
  {
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Value { get; set; }

    [Required]
    public required string CategoryId { get; set; }
  }
}
