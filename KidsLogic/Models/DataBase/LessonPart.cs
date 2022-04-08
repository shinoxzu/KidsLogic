using Microsoft.EntityFrameworkCore;

namespace KidsLogic.Models.DataBase;

[Keyless]
public class LessonPart
{
    public int LessonId { get; set; }
    public int PartId { get; set; }
    public int PartType { get; set; }
    public string Text { get; set; } = null!;
    public string ImagesUris { get; set; } = "";
    public string AnswerVariants { get; set; } = "";
    public string CorrectAnswer { get; set; } = "";
}