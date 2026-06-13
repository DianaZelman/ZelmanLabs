using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarShop.Domain.Entities;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Areas.Admin.Pages;

[Authorize(Policy = "admin")]
public class DeleteModel : PageModel
{
  private readonly ICarService _carService;

  public DeleteModel(ICarService carService)
  {
    _carService = carService;
  }

  [BindProperty]
  public Car Car { get; set; } = new();

  public async Task<IActionResult> OnGetAsync(int id)
  {
    var response = await _carService.GetCarByIdAsync(id);
    if (!response.Success || response.Data == null)
    {
      return NotFound();
    }

    Car = response.Data;
    return Page();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    await _carService.DeleteCarAsync(Car.Id);
    return RedirectToPage("./Index");
  }
}