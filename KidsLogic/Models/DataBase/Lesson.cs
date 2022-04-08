using System.ComponentModel.DataAnnotations;

namespace KidsLogic.Models.DataBase;

public class Lesson
{
    [Key] 
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}