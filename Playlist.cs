using System;
using System.Collections.Generic;
using System.Linq;
using Lab2Music.Models;

namespace Lab2Music.Services
{
    public class Playlist
    {
        private readonly List<MediaItem> _items = new List<MediaItem>();
        private MediaItem _currentItem; // Запоминаем, что выбрали в прошлый раз

        public string Name { get; }
        public ISelectionRule SelectionRule { get; set; }

        public Playlist(string name, ISelectionRule rule)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя плейлиста пустое.");
            Name = name;
            SelectionRule = rule;
        }

        public void Add(MediaItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_items.Any(x => x.Number == item.Number)) throw new Exception("Элемент с таким номером уже есть.");
            _items.Add(item);
        }

        public bool Remove(int number)
        {
            var item = _items.FirstOrDefault(x => x.Number == number);
            if (item != null)
            {
                _items.Remove(item);
                return true;
            }
            return false;
        }

        // Полиморфный метод
        public MediaItem Next()
        {
            // Передаем текущий элемент в правило выбора
            _currentItem = SelectionRule.SelectNext(_items, _currentItem);
            return _currentItem;
        }

        // Проиграть текущий выбранный элемент
        public void PlayCurrent(int times = 1)
        {
            if (_currentItem == null)
                throw new InvalidOperationException("Сначала выберите элемент (пункт 7).");

            _currentItem.Play(times); // Вызывается метод из базового класса MediaItem
        }

        public void PrintAll()
        {
            Console.WriteLine($"\n=== Плейлист: {Name} ===");
            foreach (var item in _items)
                Console.WriteLine(item.ToString()); // Вызывается переопределенный метод наследников
        }
    }
}
