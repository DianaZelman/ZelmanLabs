using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarShop.Domain.Entities;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Areas.Admin.Pages;

[Authorize(Policy = "admin")]
public class DetailsModel : PageModel
{
  private readonly ICarService _carService;

  public DetailsModel(ICarService carService)
  {
    _carService = carService;
  }

  public Car Car { get; set; } = new();

  public async Task OnGetAsync(int id)
  {
    var response = await _carService.GetCarByIdAsync(id);
    if (response.Success && response.Data != null)
    {
      Car = response.Data;
    }
  }
}