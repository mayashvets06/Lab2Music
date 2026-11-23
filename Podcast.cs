using System;
using System.Security.Policy;

namespace Lab2Music.Models
{
    public class Podcast : MediaItem
    {
        public int EpisodesCount { get; private set; }
        public Podcast(int number, string title, int episodes) : base(number, title)
        {
            EpisodesCount = episodes;
        }
        public override string GetDetails() => $"Подкаст, эпизодов: {EpisodesCount}";
    }

    public class Audiobook : MediaItem
    {
        public int BookmarkPosition { get; private set; }
        public Audiobook(int number, string title) : base(number, title) { BookmarkPosition = 0; }
        public void SetBookmark(int pos) => BookmarkPosition = pos;
        public override string GetDetails() => $"Аудиокнига, закладка на {BookmarkPosition} сек.";
    }

    public class RadioStream : MediaItem
    {
        public int Radio { get; private set; }
        public RadioStream(int number, string title, string url) : base(number, title) { }
        public override string GetDetails() => $"Радиопоток, ссылка {Radio}";
    }
}
