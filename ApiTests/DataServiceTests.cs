using Api.Services;
using AutoMapper;
using Data.Collection;
using Data.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Data;

namespace ApiTests;

public class DataServiceTests
{
  private readonly Mock<IDocumentRepository<Item>> _mockItemRepo;
  private readonly Mock<IMapper> _mockMapper;
  private readonly Mock<ILogger<DataService>> _mockLogger;
  private readonly Mock<IOptions<DatabaseSettings>> _mockOptions;
  private readonly DataService _dataService;

  public DataServiceTests()
  {
    _mockItemRepo = new Mock<IDocumentRepository<Item>>();
    _mockMapper = new Mock<IMapper>();
    _mockLogger = new Mock<ILogger<DataService>>();
    _mockOptions = new Mock<IOptions<DatabaseSettings>>();

    _mockOptions.Setup(o => o.Value)
        .Returns(new DatabaseSettings
        {
          ConnectionString = "mongodb://localhost:27017",
          Database = "TestDatabase"
        });

    _dataService = new DataService(
        _mockOptions.Object,
        _mockItemRepo.Object,
        _mockMapper.Object,
        _mockLogger.Object
    );
  }

  [Fact]
  public async Task AddItem_ShouldCreateAndReturnItemModel()
  {
    // Arrange
    var newItem = new ItemModel { Name = "NewItem", Value = 15, CategoryId = "Cat1" };

    _mockMapper.Setup(m => m.Map<Item>(It.IsAny<ItemModel>())).Returns(new Item { Id = "1" });

    _mockItemRepo.Setup(r => r.CreateItemAsync(It.IsAny<Item>())).Returns(Task.CompletedTask);

    // Act
    var result = await _dataService.AddItem(newItem);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("NewItem", result.Name);
  }
}