using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ZelmanLabs.UI.Controllers;
using ZelmanLabs.UI.Services;
using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;

namespace CarShop.Tests;

public class CarControllerTests
{
  private readonly ICarService _carService;
  private readonly ICategoryService _categoryService;

  public CarControllerTests()
  {
    _carService = Substitute.For<ICarService>();
    _categoryService = Substitute.For<ICategoryService>();
  }

  /// <summary>
  /// Проверка, что список категорий сохраняется во ViewData
  /// </summary>
  [Fact]
  public async Task Index_ReturnsViewResult_WithCategoriesInViewData()
  {
    // Arrange
    var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Спорткары", NormalizedName = "sportcars" },
            new Category { Id = 2, Name = "Электромобили", NormalizedName = "electric" }
        };

    var carsResponse = new ResponseData<ListModel<Car>>
    {
      Success = true,
      Data = new ListModel<Car>
      {
        Items = new List<Car>(),
        CurrentPage = 1,
        TotalPages = 1
      }
    };

    _categoryService.GetCategoryListAsync().Returns(Task.FromResult(
        new ResponseData<List<Category>>
        {
          Success = true,
          Data = categories
        }
    ));

    _carService.GetCarListAsync(Arg.Any<string>(), Arg.Any<int>())
        .Returns(Task.FromResult(carsResponse));

    var controller = new CarController(_carService, _categoryService);

    // Act
    var result = await controller.Index(null);

    // Assert
    var viewResult = Assert.IsType<ViewResult>(result);
    var viewDataCategories = Assert.IsType<List<Category>>(viewResult.ViewData["categories"]);
    Assert.Equal(2, viewDataCategories.Count);
    Assert.Equal("Все", viewResult.ViewData["currentCategory"]);
  }

  /// <summary>
  /// Проверка, что название текущей категории передаётся во ViewData
  /// </summary>
  [Fact]
  public async Task Index_ReturnsViewResult_WithCurrentCategoryInViewData()
  {
    // Arrange
    var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Спорткары", NormalizedName = "sportcars" },
            new Category { Id = 2, Name = "Электромобили", NormalizedName = "electric" }
        };

    var carsResponse = new ResponseData<ListModel<Car>>
    {
      Success = true,
      Data = new ListModel<Car>
      {
        Items = new List<Car>(),
        CurrentPage = 1,
        TotalPages = 1
      }
    };

    _categoryService.GetCategoryListAsync().Returns(Task.FromResult(
        new ResponseData<List<Category>>
        {
          Success = true,
          Data = categories
        }
    ));

    _carService.GetCarListAsync(Arg.Any<string>(), Arg.Any<int>())
        .Returns(Task.FromResult(carsResponse));

    var controller = new CarController(_carService, _categoryService);

    // Act
    var result = await controller.Index("sportcars");

    // Assert
    var viewResult = Assert.IsType<ViewResult>(result);
    Assert.Equal("Спорткары", viewResult.ViewData["currentCategory"]);
  }

  /// <summary>
  /// Проверка, что при ошибке возвращается View с пустым списком
  /// </summary>
  [Fact]
  public async Task Index_WhenError_ReturnsViewWithEmptyList()
  {
    // Arrange
    string errorMessage = "Test error message";

    _categoryService.GetCategoryListAsync().Returns(Task.FromResult(
        new ResponseData<List<Category>>
        {
          Success = false,
          ErrorMessage = errorMessage
        }
    ));

    var controller = new CarController(_carService, _categoryService);

    // Act
    var result = await controller.Index(null);

    // Assert
    var viewResult = Assert.IsType<ViewResult>(result);
    var model = Assert.IsType<List<Car>>(viewResult.Model);
    Assert.Empty(model);
    Assert.Equal(1, viewResult.ViewData["CurrentPage"]);
    Assert.Equal(1, viewResult.ViewData["TotalPages"]);
  }
}