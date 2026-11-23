using System;

namespace Lab2Music.Models
{
    public class Track : MediaItem
    {
        public string Artist { get; }
        public Duration Duration { get; private set; } // Композиция 
        public Genre[] Genres { get; private set; }

        // Свойства вместо наследования 
        public Medium Medium { get; private set; }
        public AccessLevel AccessLevel { get; private set; }
        public AgeRating AgeRating { get; private set; }

        public Track(int number, string title, string artist, int durationSeconds,
                     Medium medium, AccessLevel access, AgeRating ageRating)
            : base(number, title)
        {
            if (string.IsNullOrWhiteSpace(artist)) throw new ArgumentException("Исполнитель не пуст.");
            if (durationSeconds < 1 || durationSeconds > 3600) throw new ArgumentException("Длительность от 1 до 3600.");

            Artist = artist.Trim();
            Duration = new Duration(durationSeconds); // Создаём часть внутри целого
            Genres = new Genre[0];
            Medium = medium;
            AccessLevel = access;
            AgeRating = ageRating;
        }

        // Упрощённый конструктор
        public Track(int number, string title, string artist, int durationSeconds)
            : this(number, title, artist, durationSeconds,
                   new Medium("Digital"), new AccessLevel("Free"), new AgeRating("0+"))
        { }

        public void AddGenre(Genre genre)
        {
            if (genre == null) return;
            var copy = new Genre[Genres.Length + 1];
            Array.Copy(Genres, copy, Genres.Length);
            copy[copy.Length - 1] = genre;
            Genres = copy;
        }

        public override string GetDetails()
        {
            string genres = Genres.Length > 0 ? string.Join(", ", (object[])Genres) : "без жанра";
            return $"Трек '{Artist}', длительность {Duration.ToFormattedString()}с, жанры: {genres}. " +
                   $"Носитель: {Medium.Name}, Доступ: {AccessLevel.Name}, Возраст: {AgeRating.Code}";
        }
    }
}
