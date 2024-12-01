using Api.Services;
using Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  [EnableCors()]
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController(IDataService dataService, ILogger<ItemController> logger) : ControllerBase
  {
    private readonly IDataService _dataService = dataService;
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>List of CategoryModel</returns>
    [HttpGet]
    public IEnumerable<CategoryModel> Get()
    {
      try
      {
        return _dataService.GetCategories();
      }
      catch (Exception ex)
      {
        _logger.LogError("{Name}: Error - {Message}", GetType().Name, ex.Message);

        // TODO: Not the best solution, just have it to make sure we see the error and address it. Better to refactor error handling
        return [];
      }
    }
  }
}
