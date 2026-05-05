namespace CarShop.Domain.Entities
{
  public class Car
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Horsepower { get; set; }
    public string? Image { get; set; }

    // Внешний ключ для связи с категорией
    public int CategoryId { get; set; }

    // Навигационное свойство
    public Category? Category { get; set; }
  }
}