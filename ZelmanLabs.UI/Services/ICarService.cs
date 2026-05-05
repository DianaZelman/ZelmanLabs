namespace ZelmanLabs.UI.Services
{
  public interface ICarService
  {
    Task<ResponseData<ListModel<Car>>> GetCarListAsync(string? categoryNormalizedName, int pageNo = 1);

    Task<ResponseData<Car>> GetCarByIdAsync(int id);

    Task UpdateCarAsync(int id, Car car, IFormFile? formFile);

    Task DeleteCarAsync(int id);

    Task<ResponseData<Car>> CreateCarAsync(Car car, IFormFile? formFile);
  }
}