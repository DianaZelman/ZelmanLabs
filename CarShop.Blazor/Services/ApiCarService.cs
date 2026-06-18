using System.Net.Http.Json;
using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;

namespace CarShop.Blazor.Services;

public class ApiCarService : ICarService
{
  private readonly HttpClient _httpClient;
  private List<Car> _cars = new();
  private int _currentPage = 1;
  private int _totalPages = 1;

  public ApiCarService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public event Action? ListChanged;

  public IEnumerable<Car> Cars => _cars;

  public int CurrentPage => _currentPage;

  public int TotalPages => _totalPages;

  public async Task GetCars(int pageNo = 1, int pageSize = 3)
  {
    try
    {
      var queryParams = new List<string>
            {
                $"pageNo={pageNo}",
                $"pageSize={pageSize}"
            };

      var url = $"?{string.Join("&", queryParams)}";
      var response = await _httpClient.GetAsync(url);

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Car>>>();
        if (result != null && result.Success && result.Data != null)
        {
          _cars = result.Data.Items;
          _currentPage = result.Data.CurrentPage;
          _totalPages = result.Data.TotalPages;
          ListChanged?.Invoke();
        }
      }
      else
      {
        _cars = new List<Car>();
        _currentPage = 1;
        _totalPages = 1;
      }
    }
    catch (Exception)
    {
      _cars = new List<Car>();
      _currentPage = 1;
      _totalPages = 1;
    }
  }
}