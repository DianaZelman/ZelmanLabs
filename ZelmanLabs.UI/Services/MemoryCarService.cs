namespace ZelmanLabs.UI.Services
{
  public class MemoryCarService : ICarService
  {
    private List<Car> _cars;
    private List<Category> _categories;
    private readonly IConfiguration _config;

    public MemoryCarService(ICategoryService categoryService, IConfiguration config)
    {
      _config = config;
      var response = categoryService.GetCategoryListAsync().Result;
      if (response.Success && response.Data != null)
      {
        _categories = response.Data;
      }
      else
      {
        _categories = new List<Category>();
      }
      SetupData();
    }

    /// <summary>
    /// Инициализация списка автомобилей
    /// </summary>
    private void SetupData()
    {
      _cars = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    Name = "Tesla Cybertruck",
                    Description = "Электрический пикап с футуристическим дизайном из нержавеющей стали",
                    Price = 79990,
                    Horsepower = 845,
                    Image = "images/cybertruck.jpg",
                    CategoryId = _categories.Find(c => c.NormalizedName == "electric")?.Id ?? 2,
                    Category = _categories.Find(c => c.NormalizedName == "electric")
                },
                new Car
                {
                    Id = 2,
                    Name = "Ferrari F40",
                    Description = "Легендарный суперкар, созданный в честь 40-летия Ferrari",
                    Price = 1200000,
                    Horsepower = 478,
                    Image = "images/ferrari_f40.jpg",
                    CategoryId = _categories.Find(c => c.NormalizedName == "sportcars")?.Id ?? 1,
                    Category = _categories.Find(c => c.NormalizedName == "sportcars")
                },
                new Car
                {
                    Id = 3,
                    Name = "Lamborghini Aventador",
                    Description = "Итальянский суперкар с двигателем V12 и 770 лошадиными силами",
                    Price = 500000,
                    Horsepower = 770,
                    Image = "images/aventador.jpg",
                    CategoryId = _categories.Find(c => c.NormalizedName == "sportcars")?.Id ?? 1,
                    Category = _categories.Find(c => c.NormalizedName == "sportcars")
                },
                new Car
                {
                    Id = 4,
                    Name = "Toyota Supra A80",
                    Description = "Культовое японское купе с легендарным двигателем 2JZ-GTE",
                    Price = 75000,
                    Horsepower = 320,
                    Image = "images/supra.jpg",
                    CategoryId = _categories.Find(c => c.NormalizedName == "jdm")?.Id ?? 3,
                    Category = _categories.Find(c => c.NormalizedName == "jdm")
                },
                new Car
                {
                    Id = 5,
                    Name = "BMW M3 E30",
                    Description = "Классический немецкий спорткар 80-х годов",
                    Price = 60000,
                    Horsepower = 192,
                    Image = "images/bmw_m3_e30.jpg",
                    CategoryId = _categories.Find(c => c.NormalizedName == "classic")?.Id ?? 4,
                    Category = _categories.Find(c => c.NormalizedName == "classic")
                },
                new Car
                {
                    Id = 6,
                    Name = "Porsche 911 Turbo",
                    Description = "Икона автомобилестроения с турбированным оппозитным двигателем",
                    Price = 200000,
                    Horsepower = 572,
                    Image = "images/porsche_911.jpg",
                    CategoryId = _categories.Find(c => c.NormalizedName == "sportcars")?.Id ?? 1,
                    Category = _categories.Find(c => c.NormalizedName == "sportcars")
                }
            };
    }

    /// <summary>
    /// Получение списка автомобилей с фильтрацией по категории
    /// </summary>
    /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
    /// <param name="pageNo">номер страницы списка</param>
    /// <returns></returns>
    public Task<ResponseData<ListModel<Car>>> GetCarListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
      var result = new ResponseData<ListModel<Car>>();

      int pageSize = _config.GetValue<int>("ItemsPerPage");

      // Id категории для фильтрации
      int? categoryId = null;

      // Если требуется фильтрация, то найти Id категории с заданным NormalizedName
      if (categoryNormalizedName != null)
      {
        var category = _categories.Find(c => c.NormalizedName == categoryNormalizedName);
        if (category != null)
        {
          categoryId = category.Id;
        }
      }

      // Выбрать автомобили, отфильтрованные по Id категории
      var filteredCars = _cars
          .Where(c => categoryId == null || c.CategoryId == categoryId)
          .ToList();

      // вычисляем общее количество страниц
      int totalPages = (int)Math.Ceiling(filteredCars.Count / (double)pageSize);

      // выбираем только нужную страницу
      var pageItems = filteredCars
          .Skip((pageNo - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      // заполняем модель с пагинацией
      result.Data = new ListModel<Car>()
      {
        Items = pageItems,
        CurrentPage = pageNo,
        TotalPages = totalPages
      };

      // Если список пустой
      if (pageItems.Count == 0)
      {
        result.Success = false;
        result.ErrorMessage = "Нет автомобилей в выбранной категории";
      }
      else
      {
        result.Success = true;
      }

      return Task.FromResult(result);
    }

    // Остальные методы (заглушки для будущих лабораторных)

    public Task<ResponseData<Car>> GetCarByIdAsync(int id)
    {
      var car = _cars.FirstOrDefault(c => c.Id == id);
      var result = new ResponseData<Car>();

      if (car != null)
      {
        result.Data = car;
        result.Success = true;
      }
      else
      {
        result.Success = false;
        result.ErrorMessage = "Автомобиль не найден";
      }

      return Task.FromResult(result);
    }

    public Task UpdateCarAsync(int id, Car car, IFormFile? formFile)
    {
      throw new NotImplementedException();
    }

    public Task DeleteCarAsync(int id)
    {
      throw new NotImplementedException();
    }

    public Task<ResponseData<Car>> CreateCarAsync(Car car, IFormFile? formFile)
    {
      throw new NotImplementedException();
    }
  }
}