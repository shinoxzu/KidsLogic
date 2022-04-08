namespace KidsLogic;

public class PasswordGenerator
{
    private static readonly string[] WordsToUse =
    {
        "самокат", "платон", "слово", "ключ", "котик", "акула",
        "страница", "тетрадь", "пузырь", "вода", "родник"
    };

    public static string Generate()
    {
        Random random = new();
        return WordsToUse[random.Next(WordsToUse.Length)] + random.Next();
    }
}