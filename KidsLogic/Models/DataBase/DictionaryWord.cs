using System.ComponentModel.DataAnnotations;

namespace KidsLogic.Models.DataBase;

public class DictionaryWord
{
    [Key] 
    public int Id { get; set; }
    public int GivenAtLessonId { get; set; }
    public string Word { get; set; } = null!;
    public string Definition { get; set; } = null!;
}