using System;

namespace Lab2Music.Models
{
    public sealed class Genre
    {
        public string Name { get; }
        public Genre(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Жанр не может быть пустым.");
            Name = name.Trim();
        }
        public override string ToString() => Name;
    }
}
