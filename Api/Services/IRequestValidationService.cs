using Api.Models;

namespace Api.Services
{
  public interface IRequestValidationService
  {
    void Validate(AddCategory model);
    void Validate(AddItemModel model);
  }
}