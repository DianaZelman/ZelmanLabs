using Microsoft.EntityFrameworkCore;
using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;

namespace CarShop.API.Data;

public static class DbInitializer
{
  public static async Task SeedData(WebApplication app)
  {
    var apiUri = "https://localhost:7002/";

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();

    if (!context.Categories.Any() && !context.Cars.Any())
    {
      var categories = new Category[]
      {
                new Category { Name = "Спорткары", NormalizedName = "sportcars" },
                new Category { Name = "Электромобили", NormalizedName = "electric" },
                new Category { Name = "JDM", NormalizedName = "jdm" },
                new Category { Name = "Классика", NormalizedName = "classic" }
      };

      await context.Categories.AddRangeAsync(categories);
      await context.SaveChangesAsync();

      var cars = new List<Car>
            {
                new Car
                {
                    Name = "Tesla Cybertruck",
                    Description = "Электрический пикап с футуристическим дизайном из нержавеющей стали",
                    Price = 79990,
                    Horsepower = 845,
                    Category = categories.First(c => c.NormalizedName == "electric"),
                    Image = apiUri + "Images/cybertruck.jpg"
                },
                new Car
                {
                    Name = "Ferrari F40",
                    Description = "Легендарный суперкар, созданный в честь 40-летия Ferrari",
                    Price = 1200000,
                    Horsepower = 478,
                    Category = categories.First(c => c.NormalizedName == "sportcars"),
                    Image = apiUri + "Images/ferrari_f40.jpg"
                },
                new Car
                {
                    Name = "Lamborghini Aventador",
                    Description = "Итальянский суперкар с двигателем V12 и 770 лошадиными силами",
                    Price = 500000,
                    Horsepower = 770,
                    Category = categories.First(c => c.NormalizedName == "sportcars"),
                    Image = apiUri + "Images/aventador.jpg"
                },
                new Car
                {
                    Name = "Toyota Supra A80",
                    Description = "Культовое японское купе с легендарным двигателем 2JZ-GTE",
                    Price = 75000,
                    Horsepower = 320,
                    Category = categories.First(c => c.NormalizedName == "jdm"),
                    Image = apiUri + "Images/supra.jpg"
                },
                new Car
                {
                    Name = "BMW M3 E30",
                    Description = "Классический немецкий спорткар 80-х годов",
                    Price = 60000,
                    Horsepower = 192,
                    Category = categories.First(c => c.NormalizedName == "classic"),
                    Image = apiUri + "Images/bmw_m3_e30.jpg"
                },
                new Car
                {
                    Name = "Porsche 911 Turbo",
                    Description = "Икона автомобилестроения с турбированным оппозитным двигателем",
                    Price = 200000,
                    Horsepower = 572,
                    Category = categories.First(c => c.NormalizedName == "sportcars"),
                    Image = apiUri + "Images/porsche_911.jpg"
                }
            };

      await context.Cars.AddRangeAsync(cars);
      await context.SaveChangesAsync();
    }
  }
}