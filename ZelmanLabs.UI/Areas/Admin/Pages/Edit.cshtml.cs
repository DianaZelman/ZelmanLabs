using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarShop.Domain.Entities;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Areas.Admin.Pages;

[Authorize(Policy = "admin")]
public class EditModel : PageModel
{
  private readonly ICarService _carService;
  private readonly ICategoryService _categoryService;

  public EditModel(ICarService carService, ICategoryService categoryService)
  {
    _carService = carService;
    _categoryService = categoryService;
  }

  [BindProperty]
  public Car Car { get; set; } = new();

  [BindProperty]
  public IFormFile? Image { get; set; }

  public SelectList? Categories { get; set; }

  public async Task<IActionResult> OnGetAsync(int id)
  {
    var response = await _carService.GetCarByIdAsync(id);
    if (!response.Success || response.Data == null)
    {
      return NotFound();
    }

    Car = response.Data;

    var categoriesResponse = await _categoryService.GetCategoryListAsync();
    if (categoriesResponse.Success && categoriesResponse.Data != null)
    {
      Categories = new SelectList(categoriesResponse.Data, "Id", "Name", Car.CategoryId);
    }

    return Page();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    if (!ModelState.IsValid)
    {
      var categoriesResponse = await _categoryService.GetCategoryListAsync();
      if (categoriesResponse.Success && categoriesResponse.Data != null)
      {
        Categories = new SelectList(categoriesResponse.Data, "Id", "Name", Car.CategoryId);
      }
      return Page();
    }

    await _carService.UpdateCarAsync(Car.Id, Car, Image);
    return RedirectToPage("./Index");
  }
}