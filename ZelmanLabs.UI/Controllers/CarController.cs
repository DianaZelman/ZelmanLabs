using Microsoft.AspNetCore.Mvc;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Controllers
{
  public class CarController(ICarService carService, ICategoryService categoryService) : Controller
  {
    [Route("Catalog")]
    [Route("Catalog/{category}")]
    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
      // получаем список категорий
      var categoriesResponse = await categoryService.GetCategoryListAsync();
      if (categoriesResponse.Success && categoriesResponse.Data != null)
      {
        ViewData["categories"] = categoriesResponse.Data;
        ViewData["categoryDict"] = categoriesResponse.Data.ToDictionary(c => c.Id, c => c.Name);
      }

      // определяем имя текущей категории для отображения
      if (!string.IsNullOrEmpty(category))
      {
        var currentCat = categoriesResponse.Data?.FirstOrDefault(c => c.NormalizedName == category);
        ViewData["currentCategory"] = currentCat?.Name ?? "Все";
      }
      else
      {
        ViewData["currentCategory"] = "Все";
      }

      // Получаем список автомобилей с пагинацией
      var response = await carService.GetCarListAsync(category, pageNo);

      if (response.Success && response.Data != null)
      {
        // передаём данные пагинации через ViewData
        ViewData["CurrentPage"] = response.Data.CurrentPage;
        ViewData["TotalPages"] = response.Data.TotalPages;

        // передаём только список автомобилей для текущей страницы
        return View(response.Data.Items);
      }

      // если ошибка - передаём пустой список и страницу 1
      ViewData["CurrentPage"] = 1;
      ViewData["TotalPages"] = 1;
      return View(new List<Car>());
    }
  }
}