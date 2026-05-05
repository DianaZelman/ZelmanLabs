using Microsoft.AspNetCore.Mvc;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Controllers
{
  public class CarController(ICarService carService, ICategoryService categoryService) : Controller
  {
    public async Task<IActionResult> Index(string? category)
    {
      var categoriesResponse = await categoryService.GetCategoryListAsync();
      if (categoriesResponse.Success && categoriesResponse.Data != null)
      {
        ViewData["categories"] = categoriesResponse.Data;
        ViewData["categoryDict"] = categoriesResponse.Data.ToDictionary(c => c.Id, c => c.Name);
      }

      ViewData["currentCategory"] = category ?? "Все";

      var response = await carService.GetCarListAsync(category);

      if (response.Success && response.Data != null)
      {
        return View(response.Data.Items);
      }

      return View(new List<Car>());
    }
  }
}