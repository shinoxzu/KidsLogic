using KidsLogic.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace KidsLogic.ViewComponents;

public class LessonItem : ViewComponent
{
    public IViewComponentResult Invoke(Lesson? lesson = null, bool isActive = true)
    {
        if (lesson is null)
        {
            return View("EmptyLessonItem");
        }
        else if (isActive)
        {
            return View("ActiveLessonItem", lesson);
        }
        return View("ClosedLessonitem", lesson);
    }
}