using System.Net.Http.Json;
using CarShop.Domain;
using CarShop.Domain.Modeles;

namespace ZelmanLabs.UI.Services;

public class ApiCarService : ICarService
{
  private readonly HttpClient _httpClient;

  public ApiCarService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<ResponseData<ListModel<Car>>> GetCarListAsync(string? categoryNormalizedName, int pageNo = 1)
  {
    try
    {
      var queryParams = new List<string>();

      if (!string.IsNullOrEmpty(categoryNormalizedName))
      {
        queryParams.Add($"category={categoryNormalizedName}");
      }
      queryParams.Add($"pageNo={pageNo}");

      var url = $"?{string.Join("&", queryParams)}";
      var response = await _httpClient.GetAsync(url);

      if (response.IsSuccessStatusCode)
      {
        return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Car>>>();
      }

      return new ResponseData<ListModel<Car>>
      {
        Success = false,
        ErrorMessage = $"Ошибка API: {response.StatusCode}"
      };
    }
    catch (Exception ex)
    {
      return new ResponseData<ListModel<Car>>
      {
        Success = false,
        ErrorMessage = ex.Message
      };
    }
  }

  // Заглушки для неиспользуемых методов
  public Task<ResponseData<Car>> GetCarByIdAsync(int id)
      => throw new NotImplementedException();

  public Task UpdateCarAsync(int id, Car car, IFormFile? formFile)
      => throw new NotImplementedException();

  public Task DeleteCarAsync(int id)
      => throw new NotImplementedException();

  public Task<ResponseData<Car>> CreateCarAsync(Car car, IFormFile? formFile)
      => throw new NotImplementedException();
}