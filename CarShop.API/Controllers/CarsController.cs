using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;
using CarShop.API.Data;

namespace CarShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
  private readonly AppDbContext _context;
  private readonly IConfiguration _configuration;

  public CarsController(AppDbContext context, IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }

  // GET: api/cars?category=sportcars&pageNo=1&pageSize=3
  [HttpGet]
  public async Task<ActionResult<ResponseData<ListModel<Car>>>> GetCars(
      string? category,
      int pageNo = 1,
      int pageSize = 3)
  {
    var result = new ResponseData<ListModel<Car>>();

    // Загружаем машины с категориями
    var query = _context.Cars
        .Include(c => c.Category)
        .AsQueryable();

    // Фильтрация по категории
    if (!string.IsNullOrEmpty(category))
    {
      query = query.Where(c => c.Category != null && c.Category.NormalizedName == category);
    }

    // Подсчет общего количества
    var totalItems = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

    if (pageNo > totalPages && totalPages > 0)
    {
      pageNo = totalPages;
    }

    // Получаем данные для страницы
    var items = await query
        .Skip((pageNo - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    // Формируем результат
    result.Data = new ListModel<Car>
    {
      Items = items,
      CurrentPage = pageNo,
      TotalPages = totalPages
    };

    if (items.Count == 0)
    {
      result.Success = false;
      result.ErrorMessage = "No cars found in this category";
    }

    return Ok(result);
  }

  // GET: api/cars/5
  [HttpGet("{id}")]
  public async Task<ActionResult<ResponseData<Car>>> GetCar(int id)
  {
    var car = await _context.Cars
        .Include(c => c.Category)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (car == null)
    {
      return NotFound(new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = $"Car with id {id} not found"
      });
    }

    return Ok(new ResponseData<Car>
    {
      Data = car,
      Success = true
    });
  }
}