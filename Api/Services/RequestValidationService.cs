using Api.Models;
using Common.Exceptions;

namespace Api.Services
{
  public class RequestValidationService(IDataService dataService) : IRequestValidationService
  {
    private readonly IDataService _dataService = dataService;

    public void Validate(AddItemModel model)
    {
      if (model == null)
        throw new ValidationException("Model cannot be an empty", typeof(AddItemModel).Name);

      if (string.IsNullOrEmpty(model.Name))
        throw new ValidationException("Name cannot be an empty", typeof(AddItemModel).Name);

      // TODO: verify requirements if Item name should be unique, by default assuming you can add many items with the same name
      //if (_dataService.GetItemByName(model.Name) != null)
      //  throw new ValidationException($"Duplicate {model.Name} item name, the name is already exist", typeof(AddItemModel).Name);

      // TODO: verify the requirements, assuming we use $ which means no minus values
      if (  model.Value < 0)
        throw new ValidationException("Value cannot be less than zero", typeof(AddItemModel).Name);

      // Additional check if we pass correct Category Id
      if (_dataService.GetCategoryById(model.CategoryId) == null)
        throw new ValidationException($"There is not Category with id = {model.CategoryId}", typeof(AddItemModel).Name);
    }

    public void Validate(AddCategory model)
    {
      // TODO: add code to validate AddCategory 
      throw new NotImplementedException();
    }
  }
}
