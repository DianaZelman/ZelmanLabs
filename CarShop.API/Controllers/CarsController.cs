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
  private readonly IWebHostEnvironment _env;

  public CarsController(AppDbContext context, IConfiguration configuration, IWebHostEnvironment env)
  {
    _context = context;
    _configuration = configuration;
    _env = env;
  }

  // GET: api/cars?category=sportcars&pageNo=1&pageSize=3
  [HttpGet]
  public async Task<ActionResult<ResponseData<ListModel<Car>>>> GetCars(
      string? category,
      int pageNo = 1,
      int pageSize = 3)
  {
    var result = new ResponseData<ListModel<Car>>();

    var query = _context.Cars
        .Include(c => c.Category)
        .AsQueryable();

    if (!string.IsNullOrEmpty(category))
    {
      query = query.Where(c => c.Category != null && c.Category.NormalizedName == category);
    }

    var totalItems = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

    if (pageNo > totalPages && totalPages > 0)
    {
      pageNo = totalPages;
    }

    var items = await query
        .Skip((pageNo - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

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

  // POST: api/cars - создание автомобиля
  [HttpPost]
  public async Task<ActionResult<ResponseData<Car>>> CreateCar([FromBody] Car car)
  {
    try
    {
      if (car.CategoryId > 0)
      {
        var category = await _context.Categories.FindAsync(car.CategoryId);
        if (category != null)
        {
          car.Category = category;
        }
      }

      await _context.Cars.AddAsync(car);
      await _context.SaveChangesAsync();

      return Ok(new ResponseData<Car>
      {
        Data = car,
        Success = true
      });
    }
    catch (Exception ex)
    {
      return BadRequest(new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = ex.Message
      });
    }
  }

  // PUT: api/cars/5 - обновление автомобиля
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateCar(int id, [FromBody] Car car)
  {
    if (id != car.Id)
    {
      return BadRequest(new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = "ID mismatch"
      });
    }

    var existingCar = await _context.Cars.FindAsync(id);
    if (existingCar == null)
    {
      return NotFound(new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = "Car not found"
      });
    }

    // Обновляем поля
    existingCar.Name = car.Name;
    existingCar.Description = car.Description;
    existingCar.Price = car.Price;
    existingCar.Horsepower = car.Horsepower;
    existingCar.CategoryId = car.CategoryId;

    try
    {
      await _context.SaveChangesAsync();
      return Ok(new ResponseData<Car>
      {
        Data = existingCar,
        Success = true
      });
    }
    catch (Exception ex)
    {
      return BadRequest(new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = ex.Message
      });
    }
  }

  // DELETE: api/cars/5 - удаление автомобиля
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCar(int id)
  {
    var car = await _context.Cars.FindAsync(id);
    if (car == null)
    {
      return NotFound(new ResponseData<Car>
      {
        Success = false,
        ErrorMessage = "Car not found"
      });
    }

    // Удаляем файл изображения, если есть
    if (!string.IsNullOrEmpty(car.Image))
    {
      var imagePath = Path.Combine(_env.WebRootPath, "Images", Path.GetFileName(car.Image));
      if (System.IO.File.Exists(imagePath))
      {
        System.IO.File.Delete(imagePath);
      }
    }

    _context.Cars.Remove(car);
    await _context.SaveChangesAsync();

    return Ok(new ResponseData<Car>
    {
      Success = true
    });
  }

  // POST: api/cars/{id}/image - сохранение изображения
  [HttpPost("{id}/image")]
  public async Task<IActionResult> SaveImage(int id, IFormFile image)
  {
    var car = await _context.Cars.FindAsync(id);
    if (car == null)
    {
      return NotFound(new { error = "Car not found" });
    }

    if (image == null || image.Length == 0)
    {
      return BadRequest(new { error = "No image file" });
    }

    var imagesPath = Path.Combine(_env.WebRootPath, "Images");
    if (!Directory.Exists(imagesPath))
    {
      Directory.CreateDirectory(imagesPath);
    }

    // Случайное имя файла
    var randomName = Path.GetRandomFileName();
    var extension = Path.GetExtension(image.FileName);
    var fileName = Path.ChangeExtension(randomName, extension);
    var filePath = Path.Combine(imagesPath, fileName);

    // Сохраняем файл
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
      await image.CopyToAsync(stream);
    }

    // Формируем URL
    var host = $"{Request.Scheme}://{Request.Host}";
    var imageUrl = $"{host}/Images/{fileName}";

    // Удаляем старое изображение, если есть
    if (!string.IsNullOrEmpty(car.Image))
    {
      var oldImagePath = Path.Combine(_env.WebRootPath, "Images", Path.GetFileName(car.Image));
      if (System.IO.File.Exists(oldImagePath))
      {
        System.IO.File.Delete(oldImagePath);
      }
    }

    car.Image = imageUrl;
    await _context.SaveChangesAsync();

    return Ok(new { url = imageUrl });
  }
}