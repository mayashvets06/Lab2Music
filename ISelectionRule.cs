using System;
using System.Collections.Generic;
using Lab2Music.Models;

namespace Lab2Music.Services
{
    public interface ISelectionRule
    {
        MediaItem SelectNext(List<MediaItem> items, MediaItem current);
    }

    // 1. По порядку
    public class SequentialRule : ISelectionRule
    {
        public MediaItem SelectNext(List<MediaItem> items, MediaItem current)
        {
            if (items.Count == 0) return null;
            if (current == null) return items[0];
            int index = items.FindIndex(x => x.Number == current.Number);
            return items[(index + 1) % items.Count];
        }
    }

    // 2. По популярности (самый прослушиваемый)
    public class PopularityRule : ISelectionRule
    {
        public MediaItem SelectNext(List<MediaItem> items, MediaItem current)
        {
            if (items.Count == 0) return null;
            MediaItem best = items[0];
            foreach (var item in items)
            {
                if (item.PlayCount > best.PlayCount) best = item;
            }
            return best;
        }
    }

    // 3. Случайный (без повторов)
    public class RandomRule : ISelectionRule
    {
        private Random _rand = new Random();
        public MediaItem SelectNext(List<MediaItem> items, MediaItem current)
        {
            if (items.Count == 0) return null;
            if (items.Count == 1) return items[0];

            MediaItem next;
            do
            {
                next = items[_rand.Next(items.Count)];
            } while (next.Number == current?.Number);

            return next;
        }
    }
}
