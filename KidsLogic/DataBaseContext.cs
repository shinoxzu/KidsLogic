using KidsLogic.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace KidsLogic;

public sealed class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Achievement> Achievements { get; set; } = null!;
    public DbSet<DictionaryWord> Dictionary { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<LessonPart> LessonsParts { get; set; } = null!;
}