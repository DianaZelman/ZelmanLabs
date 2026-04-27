using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZelmanLabs.UI.Models;

namespace ZelmanLabs.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<ListDemo> _listData = new()
        {
            new ListDemo { Id = 1, Name = "Item 1" },
            new ListDemo { Id = 2, Name = "Item 2" },
            new ListDemo { Id = 3, Name = "Item 3" }
        };

        //TODO: change logic
        [Authorize(Policy = "admin")]
        public IActionResult Index()
        {
            ViewData["text"] = "Лабораторная работа №2";

            SelectList data = new SelectList(_listData, "Id", "Name");

            return View(data);
        }
    }
}