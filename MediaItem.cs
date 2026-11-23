using System;

namespace Lab2Music.Models
{
    public abstract class MediaItem
    {
        private static int _itemsCreated = 0;
        public static int ItemsCreated => _itemsCreated;

        protected int _number;
        protected string _title;
        protected int _playCount;

        protected MediaItem(int number, string title)
        {
            if (number <= 0) throw new ArgumentException("Номер должен быть положительным.");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Название не может быть пустым.");

            _number = number;
            _title = title.Trim();
            _playCount = 0;
            _itemsCreated++;
        }

        public int Number => _number;
        public string Title => _title;
        public int PlayCount => _playCount;

        // Общий алгоритм для всех медиаэлементов. Запрещён к переопределению (sealed).
        public void Play(int times)
        {
            if (times <= 0) throw new ArgumentException("Количество прослушиваний > 0.");
            _playCount += times;
        }

        public void Play() => Play(1);

        // Абстрактный метод
        public abstract string GetDetails();

        public override string ToString() => $"[{_number}] {_title} (прослушиваний: {_playCount}) - {GetDetails()}";
    }
}
