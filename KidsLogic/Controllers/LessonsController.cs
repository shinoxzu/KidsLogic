using System.Security.Claims;
using KidsLogic.Models;
using KidsLogic.Models.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KidsLogic.Controllers;

[Authorize]
public class LessonsController : Controller
{
    private readonly DataBaseContext _dataBase;

    public LessonsController(DataBaseContext dataBase)
    {
        _dataBase = dataBase;
    }

    public async Task<IActionResult> Index()
    {
        long userid = long.Parse(User.FindFirst(ClaimTypes.Sid)?.Value!);
        User user = (await _dataBase.Users.FindAsync(userid))!;

        ViewData["NowLessonId"] = user.NowLessonId;
        return View(await _dataBase.Lessons.ToArrayAsync());
    }
    
    [HttpGet]
    [Route("[controller]/{lessonId:int}/{partId:int}")]
    public async Task<IActionResult> Index(int lessonId, int partId)
    {
        long userid = long.Parse(User.FindFirst(ClaimTypes.Sid)?.Value!);
        User user = (await _dataBase.Users.FindAsync(userid))!;

        if (user.NowLessonId < lessonId)
        {
            return Redirect("/Lessons");
        }

        Lesson? lesson = await _dataBase.Lessons.FindAsync(lessonId);
        var lessonParts = await _dataBase.LessonsParts
            .Where(l => l.LessonId == lessonId)
            .OrderBy(p => p.LessonId)
            .ThenBy(p => p.PartId)
            .ToArrayAsync();
        LessonPart? lessonPart = lessonParts.FirstOrDefault(l => l.PartId == partId);

        if (lesson is null || lessonPart is null)
        {
            return View("LessonNotReadyYet");
        }

        LessonDataModel model = new()
        {
            Lesson = lesson,
            LessonsPart = lessonPart,
            IsLastPage = lessonParts[^1].PartId == partId
        };

        return lessonPart.PartType switch
        {
            1 => View("DataLesson", model),
            2 => View("ExerciseLesson", model),
            
            _ => View("DataLesson", model)
        };
    }

    [HttpPost]
    [Route("[controller]/{lessonId:int}/{partId:int}")]
    public async Task<IActionResult> Index(int lessonId, int partId, string exerciseAnswer)
    {
        long userid = long.Parse(User.FindFirst(ClaimTypes.Sid)?.Value!);
        User user = (await _dataBase.Users.FindAsync(userid))!;

        if (user.NowLessonId < lessonId)
        {
            return Redirect("/Lessons");
        }

        Lesson? lesson = await _dataBase.Lessons.FindAsync(lessonId);
        var lessonParts = await _dataBase.LessonsParts
            .Where(l => l.LessonId == lessonId)
            .OrderBy(p => p.LessonId)
            .ThenBy(p => p.PartId)
            .ToArrayAsync();
        LessonPart? lessonPart = lessonParts.FirstOrDefault(l => l.PartId == partId);

        if (lesson is null || lessonPart is null)
        {
            return View("LessonNotReadyYet");
        }

        if (exerciseAnswer.Trim() == lessonPart.CorrectAnswer)
        {
            if (lessonParts[^1].PartId == partId)
            {
                if (user.NowLessonId == lessonId)
                {
                    user.NowLessonId += 1;
                    await _dataBase.SaveChangesAsync();
                }
                return Redirect("/Lessons");
            }

            return Redirect($"/Lessons/{lessonId}/{partId + 1}");
        }

        ModelState.AddModelError("", "Это неверный ответ. Попробуй еще разок!");
        LessonDataModel model = new()
        {
            Lesson = lesson,
            LessonsPart = lessonPart,
            IsLastPage = lessonParts[^1].PartId == partId
        };

        return View("ExerciseLesson", model);
    }
}