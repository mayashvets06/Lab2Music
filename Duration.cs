using System;

namespace Lab2Music.Models
{
    public class Duration
    {
        public int TotalSeconds { get; }

        public Duration(int seconds)
        {
            if (seconds < 0) throw new ArgumentException("Длительность не может быть отрицательной.");
            TotalSeconds = seconds;
        }

        public string ToFormattedString() => $"{TotalSeconds / 60}:{TotalSeconds % 60:D2}";
    }
}
