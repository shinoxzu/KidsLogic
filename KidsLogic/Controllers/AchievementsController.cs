using System.Security.Claims;
using KidsLogic.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidsLogic.Controllers;

[Authorize]
public class AchievementsController : Controller
{
    private readonly DataBaseContext _dataBase;

    public AchievementsController(DataBaseContext dataBase)
    {
        _dataBase = dataBase;
    }
    
    public async Task<IActionResult> Index()
    {
        long userId = long.Parse(User.FindFirst(ClaimTypes.Sid)?.Value!);
        User user = (await _dataBase.Users.FindAsync(userId))!;
        
        Achievement[] achievements = await _dataBase.Achievements
            .Where(w => w.GivenAtLessonId < user.NowLessonId)
            .ToArrayAsync();
        
        return View(achievements);
    }
}