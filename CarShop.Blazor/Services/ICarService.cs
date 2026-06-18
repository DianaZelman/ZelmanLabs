using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;

namespace CarShop.Blazor.Services;

public interface ICarService
{
  event Action ListChanged;
  IEnumerable<Car> Cars { get; }
  int CurrentPage { get; }
  int TotalPages { get; }
  Task GetCars(int pageNo = 1, int pageSize = 3);
}