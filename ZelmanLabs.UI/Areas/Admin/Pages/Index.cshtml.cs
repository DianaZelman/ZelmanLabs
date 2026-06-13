using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CarShop.Domain.Entities;
using ZelmanLabs.UI.Services;

namespace ZelmanLabs.UI.Areas.Admin.Pages;

[Authorize(Policy = "admin")]
public class IndexModel : PageModel
{
  private readonly ICarService _carService;

  public IndexModel(ICarService carService)
  {
    _carService = carService;
  }

  public List<Car> Cars { get; set; } = new();
  public int CurrentPage { get; set; } = 1;
  public int TotalPages { get; set; } = 1;

  public async Task OnGetAsync(int pageNo = 1)
  {
    var response = await _carService.GetCarListAsync(null, pageNo);
    if (response.Success && response.Data != null)
    {
      Cars = response.Data.Items;
      CurrentPage = response.Data.CurrentPage;
      TotalPages = response.Data.TotalPages;
    }
  }
}