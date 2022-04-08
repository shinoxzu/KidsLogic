using KidsLogic.Models.DataBase;

namespace KidsLogic.Models;

public class LessonDataModel
{
    public Lesson Lesson { get; init; } = null!;
    public LessonPart LessonsPart { get; init; } = null!;
    
    public bool IsLastPage { get; init; }
    public string? ExerciseAnswer { get; init; }
}