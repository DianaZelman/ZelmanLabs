using Microsoft.EntityFrameworkCore;
using CarShop.Domain.Entities;

namespace CarShop.API.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

  public DbSet<Car> Cars { get; set; }

  public DbSet<Category> Categories { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

  }
}