using System.Net.Http.Json;
using CarShop.Domain.Modeles;

namespace ZelmanLabs.UI.Services;

public class ApiCategoryService : ICategoryService
{
  private readonly HttpClient _httpClient;

  public ApiCategoryService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
  {
    try
    {
      var response = await _httpClient.GetAsync("");

      if (response.IsSuccessStatusCode)
      {
        return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
      }

      return new ResponseData<List<Category>>
      {
        Success = false,
        ErrorMessage = $"Ошибка API: {response.StatusCode}"
      };
    }
    catch (Exception ex)
    {
      return new ResponseData<List<Category>>
      {
        Success = false,
        ErrorMessage = ex.Message
      };
    }
  }
}