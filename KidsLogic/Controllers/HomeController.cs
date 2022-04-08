using System.Security.Claims;
using KidsLogic.Models.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidsLogic.Controllers;

public class HomeController : Controller
{
    private readonly DataBaseContext _dataBase;

    public HomeController(DataBaseContext dataBase)
    {
        _dataBase = dataBase;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            long userId = long.Parse(User.FindFirst(ClaimTypes.Sid)?.Value!);
            User user = (await _dataBase.Users.FindAsync(userId))!;

            ViewData["NowLessonId"] = user.NowLessonId;

            return View( await _dataBase.Lessons.ToArrayAsync());
        }
        else
        {
            return View("About");
        }
    }
}