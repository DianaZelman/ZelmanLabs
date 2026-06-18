using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using CarShop.API.Controllers;
using CarShop.API.Data;
using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;
using Microsoft.AspNetCore.Hosting;

namespace CarShop.Tests;

public class CarsApiControllerTests : IDisposable
{
  private readonly AppDbContext _context;
  private readonly IWebHostEnvironment _environment;

  public CarsApiControllerTests()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite("Filename=:memory:")
        .Options;

    _context = new AppDbContext(options);
    _context.Database.OpenConnection();
    _context.Database.EnsureCreated();

    SeedData();

    _environment = Substitute.For<IWebHostEnvironment>();
    _environment.WebRootPath.Returns("/tmp");
  }

  private void SeedData()
  {
    var categories = new Category[]
    {
            new Category { Id = 1, Name = "Спорткары", NormalizedName = "sportcars" },
            new Category { Id = 2, Name = "Электромобили", NormalizedName = "electric" }
    };

    _context.Categories.AddRange(categories);
    _context.SaveChanges();

    var cars = new Car[]
    {
            new Car { Id = 1, Name = "Ferrari F40", CategoryId = 1 },
            new Car { Id = 2, Name = "Lamborghini Aventador", CategoryId = 1 },
            new Car { Id = 3, Name = "Tesla Cybertruck", CategoryId = 2 },
            new Car { Id = 4, Name = "Tesla Model S", CategoryId = 2 },
            new Car { Id = 5, Name = "Porsche 911", CategoryId = 1 }
    };

    _context.Cars.AddRange(cars);
    _context.SaveChanges();
  }

  /// <summary>
  /// Проверка фильтрации по категории
  /// </summary>
  [Fact]
  public async Task GetCars_WithCategoryFilter_ReturnsFilteredResults()
  {
    // Arrange
    var controller = new CarsController(_context, null!, _environment);

    // Act
    var result = await controller.GetCars("sportcars");

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var responseData = Assert.IsType<ResponseData<ListModel<Car>>>(okResult.Value);

    Assert.True(responseData.Success);
    Assert.NotNull(responseData.Data);
    Assert.All(responseData.Data.Items, car => Assert.Equal(1, car.CategoryId));
  }

  /// <summary>
  /// Проверка правильного подсчёта количества страниц
  /// </summary>
  [Theory]
  [InlineData(2, 3)]
  [InlineData(3, 2)]
  public async Task GetCars_ReturnsCorrectPagesCount(int pageSize, int expectedPages)
  {
    // Arrange
    var controller = new CarsController(_context, null!, _environment);

    // Act
    var result = await controller.GetCars(null, 1, pageSize);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var responseData = Assert.IsType<ResponseData<ListModel<Car>>>(okResult.Value);

    Assert.True(responseData.Success);
    Assert.NotNull(responseData.Data);
    Assert.Equal(expectedPages, responseData.Data.TotalPages);
  }

  /// <summary>
  /// Проверка возврата нужной страницы
  /// </summary>
  [Fact]
  public async Task GetCars_ReturnsCorrectPageData()
  {
    // Arrange
    var controller = new CarsController(_context, null!, _environment);
    int pageNo = 2;
    int pageSize = 3;

    // Act
    var result = await controller.GetCars(null, pageNo, pageSize);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var responseData = Assert.IsType<ResponseData<ListModel<Car>>>(okResult.Value);

    Assert.True(responseData.Success);
    Assert.NotNull(responseData.Data);
    Assert.Equal(pageNo, responseData.Data.CurrentPage);
    Assert.Equal(2, responseData.Data.Items.Count);
  }

  public void Dispose()
  {
    _context.Database.CloseConnection();
    _context.Dispose();
  }
}