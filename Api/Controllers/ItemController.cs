using Api.Models;
using Api.Services;
using AutoMapper;
using Common.Exceptions;
using Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  [EnableCors()]
  [Route("api/[controller]")]
  [ApiController]
  public class ItemController(IDataService dataService, IRequestValidationService requestValidationService, IMapper mapper, ILogger<ItemController> logger) : ControllerBase
  {
    private readonly IDataService _dataService = dataService;
    private readonly IRequestValidationService _requestValidationService = requestValidationService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Get all items (assuming small amount, so no paging yet)
    /// </summary>
    /// <returns>List of ItemModel</returns>
    [HttpGet]
    public IEnumerable<ItemModel> Get()
    {
      try
      {
        return _dataService.GetItems();
      }
      catch (Exception ex)
      {
        _logger.LogError("{Name}: Error - {Message}", GetType().Name, ex.Message);

        // TODO: Not the best solution, just have it to make sure we see the error and address it. Better to refactor error handling
        return [];
      }
    }

    /// <summary>
    /// Get Item by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>ItemModel or null</returns>
    [HttpGet("{id}")]
    public ItemModel? GetById([FromRoute] string id)
    {
      try
      {
        return _dataService.GetItemById(id);
      }
      catch (Exception ex)
      {
        _logger.LogError("{Name}: Error - {Message}", GetType().Name, ex.Message);

        // TODO: Not the best solution, just have it to make sure we see the error and address it. Better to refactor error handling
        return null;
      }
    }

    /// <summary>
    /// Add new item
    /// </summary>
    /// <param name="item">AddItemModel</param>
    /// <returns>Ok or BadRequest</returns>
    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] AddItemModel item)
    {
      try
      {
        _requestValidationService.Validate(item);

        var result = await _dataService.AddItem(_mapper.Map<ItemModel>(item));
        return Ok(result);
      }
      catch (ValidationException ex)
      {
        _logger.LogError("{Name}: Validation Error - {Message}", GetType().Name, ex.Message);
        return ValidationProblem(ex.Message);
      }
      catch (Exception ex)
      {
        _logger.LogError("{Name}: Error - {Message}", GetType().Name, ex.Message);

        // TODO: Not the best solution, just have it to make sure we see the error and address it. Better to refactor error handling
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    /// <summary>
    /// Delete item by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>OK, NotFound or error</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteItem(string id)
    {
      try
      {
        var item = _dataService.GetItemById(id);

        if (item == null)
        {
          _logger.LogError("{Name}: Cannot find the item with id = {Id} to delete", GetType().Name, id);
          return NotFound("");
        }

        var result = _dataService.DeleteItem(id);
        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError("{Name}: Error - {Message}", GetType().Name, ex.Message);

        // TODO: Not the best solution, just have it to make sure we see the error and address it. Better to refactor error handling
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }
  }
}
