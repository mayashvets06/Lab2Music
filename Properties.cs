namespace Lab2Music.Models
{
    // Носитель
    public class Medium
    {
        public string Name { get; }
        public Medium(string name) { Name = name; }
    }

    // Уровень доступа
    public class AccessLevel
    {
        public string Name { get; }
        public AccessLevel(string name) { Name = name; }
    }

    // Возрастная маркировка
    public class AgeRating
    {
        public string Code { get; }
        public AgeRating(string code) { Code = code; }
    }
}
