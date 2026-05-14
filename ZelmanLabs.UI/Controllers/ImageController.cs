
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZelmanLabs.UI.Data;

namespace ZelmanLabs.UI.Controllers;

public class ImageController(UserManager<ApplicationUser> um, IWebHostEnvironment env) : Controller
{
  public async Task<IActionResult> GetAvatar()
  {
    var user = await um.FindByNameAsync(User.Identity.Name);

    if (user.Avatar != null)
    {
      return File(user.Avatar, "image/*");

    }

    var root = env.WebRootPath;
    var path = Path.Combine(root, "images", "default-profile-picture.jpg");
    return File(path, "image/jpg");
  }
}