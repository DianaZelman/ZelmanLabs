namespace ZelmanLabs.UI.Services
{
  public interface ICategoryService
  {
    Task<ResponseData<List<Category>>> GetCategoryListAsync();
  }
}