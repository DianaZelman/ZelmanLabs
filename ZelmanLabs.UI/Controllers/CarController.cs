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
      var categoriesResponse = await categoryService.GetCategoryListAsync();

      if (!categoriesResponse.Success || categoriesResponse.Data == null)
      {
        ViewData["categories"] = new List<Category>();
        ViewData["categoryDict"] = new Dictionary<int, string>();
        ViewData["currentCategory"] = "Все";
        ViewData["CurrentPage"] = 1;
        ViewData["TotalPages"] = 1;
        return View(new List<Car>());
      }

      ViewData["categories"] = categoriesResponse.Data;
      ViewData["categoryDict"] = categoriesResponse.Data.ToDictionary(c => c.Id, c => c.Name);

      if (!string.IsNullOrEmpty(category))
      {
        var currentCat = categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category);
        ViewData["currentCategory"] = currentCat?.Name ?? "Все";
      }
      else
      {
        ViewData["currentCategory"] = "Все";
      }

      var response = await carService.GetCarListAsync(category, pageNo);

      if (response.Success && response.Data != null)
      {
        ViewData["CurrentPage"] = response.Data.CurrentPage;
        ViewData["TotalPages"] = response.Data.TotalPages;
        return View(response.Data.Items);
      }

      ViewData["CurrentPage"] = 1;
      ViewData["TotalPages"] = 1;
      return View(new List<Car>());
    }
  }
}