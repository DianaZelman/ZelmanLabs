using System.Net.Http.Json;
using CarShop.Domain.Entities;
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
        return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Car>>>()
            ?? new ResponseData<ListModel<Car>> { Success = false, ErrorMessage = "No data" };
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

  public async Task<ResponseData<Car>> GetCarByIdAsync(int id)
  {
    try
    {
      var response = await _httpClient.GetAsync($"{id}");
      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<ResponseData<Car>>();
        if (result != null && result.Success)
        {
          return result;
        }
      }
      return new ResponseData<Car> { Success = false, ErrorMessage = "Car not found" };
    }
    catch (Exception ex)
    {
      return new ResponseData<Car> { Success = false, ErrorMessage = ex.Message };
    }
  }

  public async Task<ResponseData<Car>> CreateCarAsync(Car car, IFormFile? formFile)
  {
    try
    {
      var response = await _httpClient.PostAsJsonAsync("", car);

      if (!response.IsSuccessStatusCode)
      {
        var errorText = await response.Content.ReadAsStringAsync();
        return new ResponseData<Car>
        {
          Success = false,
          ErrorMessage = $"Failed to create car: {response.StatusCode} - {errorText}"
        };
      }

      var result = await response.Content.ReadFromJsonAsync<ResponseData<Car>>();
      if (result == null || !result.Success || result.Data == null)
      {
        return new ResponseData<Car>
        {
          Success = false,
          ErrorMessage = "Invalid response from API"
        };
      }

      if (formFile != null)
      {
        var createdCar = result.Data;
        using var stream = formFile.OpenReadStream();
        var fileContent = new StreamContent(stream);
        var formData = new MultipartFormDataContent();
        formData.Add(fileContent, "image", formFile.FileName);

        var imageResponse = await _httpClient.PostAsync($"{createdCar.Id}/image", formData);

        if (imageResponse.IsSuccessStatusCode)
        {
          var imageResult = await imageResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
          if (imageResult != null && imageResult.ContainsKey("url"))
          {
            createdCar.Image = imageResult["url"];
            result.Data = createdCar;
          }
        }
      }

      return result;
    }
    catch (Exception ex)
    {
      return new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = $"Exception: {ex.Message}"
      };
    }
  }

  public async Task UpdateCarAsync(int id, Car car, IFormFile? formFile)
  {
    try
    {
      await _httpClient.PutAsJsonAsync($"{id}", car);

      if (formFile != null)
      {
        using var stream = formFile.OpenReadStream();
        var fileContent = new StreamContent(stream);
        var formData = new MultipartFormDataContent();
        formData.Add(fileContent, "image", formFile.FileName);

        await _httpClient.PostAsync($"{id}/image", formData);
      }
    }
    catch (Exception ex)
    {
      throw new Exception($"Error updating car: {ex.Message}");
    }
  }

  public async Task DeleteCarAsync(int id)
  {
    try
    {
      await _httpClient.DeleteAsync($"{id}");
    }
    catch (Exception ex)
    {
      throw new Exception($"Error deleting car: {ex.Message}");
    }
  }
}