
namespace ZelmanLabs.UI.Services
{
  public class MemoryCategoryService : ICategoryService
  {
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
      var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Спорткары", NormalizedName = "sportcars" },
                new Category { Id = 2, Name = "Электромобили", NormalizedName = "electric" },
                new Category { Id = 3, Name = "JDM", NormalizedName = "jdm" },
                new Category { Id = 4, Name = "Классика", NormalizedName = "classic" }
            };

      var result = new ResponseData<List<Category>> { Data = categories };
      return Task.FromResult(result);
    }
  }
}