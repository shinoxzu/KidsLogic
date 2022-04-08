using System.Security.Claims;
using KidsLogic.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidsLogic.Controllers;

[Authorize]
public class DictionaryController : Controller
{
    private readonly DataBaseContext _dataBase;

    public DictionaryController(DataBaseContext dataBase)
    {
        _dataBase = dataBase;
    }
    
    public async Task<IActionResult> Index()
    {
        long userId = long.Parse(User.FindFirst(ClaimTypes.Sid)?.Value!);
        User user = (await _dataBase.Users.FindAsync(userId))!;
        
        DictionaryWord[] dictionaryWords = await _dataBase.Dictionary
            .Where(w => w.GivenAtLessonId < user.NowLessonId)
            .ToArrayAsync();

        return View(dictionaryWords);
    }
}