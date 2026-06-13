using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarShop.Domain.Entities;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Areas.Admin.Pages;

[Authorize(Policy = "admin")]
public class CreateModel : PageModel
{
  private readonly ICarService _carService;
  private readonly ICategoryService _categoryService;

  public CreateModel(ICarService carService, ICategoryService categoryService)
  {
    _carService = carService;
    _categoryService = categoryService;
  }

  [BindProperty]
  public Car Car { get; set; } = new();

  [BindProperty]
  public IFormFile? Image { get; set; }

  public SelectList? Categories { get; set; }

  public async Task<IActionResult> OnGetAsync()
  {
    var categoriesResponse = await _categoryService.GetCategoryListAsync();
    if (categoriesResponse.Success && categoriesResponse.Data != null)
    {
      Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
    }
    return Page();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    if (!ModelState.IsValid)
    {
      await OnGetAsync();
      return Page();
    }

    var response = await _carService.CreateCarAsync(Car, Image);

    if (response.Success)
    {
      return RedirectToPage("./Index");
    }

    ModelState.AddModelError("", response.ErrorMessage ?? "Ошибка при создании");
    await OnGetAsync();
    return Page();
  }
}