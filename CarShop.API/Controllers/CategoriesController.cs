using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop.Domain.Entities;
using CarShop.Domain.Modeles;
using CarShop.API.Data;

namespace CarShop.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
  private readonly AppDbContext _context;

  public CategoriesController(AppDbContext context)
  {
    _context = context;
  }

  // GET: api/categories
  [HttpGet]
  public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
  {
    var categories = await _context.Categories.ToListAsync();

    var response = new ResponseData<List<Category>>
    {
      Data = categories,
      Success = true,
      ErrorMessage = string.Empty
    };

    return Ok(response);
  }

  // GET: api/categories/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ResponseData<Category>>> GetCategory(int id)
  {
    var category = await _context.Categories.FindAsync(id);

    if (category == null)
    {
      return NotFound(new ResponseData<Category>
      {
        Success = false,
        ErrorMessage = $"Category with id {id} not found"
      });
    }

    return Ok(new ResponseData<Category>
    {
      Data = category,
      Success = true,
      ErrorMessage = string.Empty
    });
  }
}