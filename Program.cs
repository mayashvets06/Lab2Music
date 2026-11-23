using System;
using Lab2Music.Models;
using Lab2Music.Services;

namespace Lab2Music
{
    internal static class Program
    {
        // Единый плейлист для работы из меню
        private static Playlist _playlist = new Playlist("Мой Микс", new SequentialRule());

        // Счетчик для автоматической выдачи номеров новым элементам
        private static int _nextNumber = 1;

        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Демо-данные при старте
            var t1 = new Track(_nextNumber++, "Bohemian Rhapsody", "Queen", 355);
            t1.AddGenre(new Genre("Rock"));
            _playlist.Add(t1);
            _playlist.Add(new Podcast(_nextNumber++, "Разбор полетов", 12));

            while (true)
            {
                PrintMenu();
                string choice = (Console.ReadLine() ?? "").Trim();

                try
                {
                    switch (choice)
                    {
                        case "1": AddTrack(); break;
                        case "2": AddPodcast(); break;
                        case "3": AddAudiobook(); break;
                        case "4": AddRadio(); break;
                        case "5": ShowAll(); break;
                        case "6": ChangeRule(); break;
                        case "7": PlayNext(); break;
                        case "8": RemoveByNumber(); break;
                        case "0": return;
                        default:
                            Console.WriteLine("Неизвестная команда. Попробуй ещё раз.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Ловим все ошибки (например, пустое название или отрицательную длительность)
                    Console.WriteLine("Ошибка: " + ex.GetType().Name + ": " + ex.Message);
                }

                Console.WriteLine("\nНажми Enter, чтобы продолжить...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        // МЕНЮ

        private static void PrintMenu()
        {
            Console.WriteLine("===== ПЛЕЙЛИСТ: ГЛАВНОЕ МЕНЮ =====");
            Console.WriteLine("1  — добавить музыкальный трек (Track)");
            Console.WriteLine("2  — добавить подкаст (Podcast)");
            Console.WriteLine("3  — добавить аудиокнигу (Audiobook)");
            Console.WriteLine("4  — добавить радиопоток (RadioStream)");
            Console.WriteLine("5  — показать все элементы");
            Console.WriteLine("6  — сменить правило выбора (ISelectionRule)");
            Console.WriteLine("7  — выбрать следующий элемент по правилу");
            Console.WriteLine("8  — удалить элемент по номеру");
            Console.WriteLine("0  — выход");
            Console.WriteLine();
            Console.Write("Твой выбор: ");
        }

        // ВВОД

        private static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            string s = (Console.ReadLine() ?? "").Trim();
            if (!int.TryParse(s, out int value))
                throw new ArgumentException("Нужно ввести целое число.");
            return value;
        }

        private static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return (Console.ReadLine() ?? "").Trim();
        }

        // КОМАНДЫ

        private static void AddTrack()
        {
            string title = ReadString("Название: ");
            string artist = ReadString("Исполнитель: ");
            int duration = ReadInt("Длительность в секундах (1..3600): ");
            string genreName = ReadString("Жанр (можно оставить пустым): ");

            // Упрощенный конструктор (Medium, Access, AgeRating по умолчанию)
            var track = new Track(_nextNumber, title, artist, duration);

            if (!string.IsNullOrWhiteSpace(genreName))
                track.AddGenre(new Genre(genreName));

            _playlist.Add(track);
            _nextNumber++;
            Console.WriteLine("Трек добавлен.");
        }

        private static void AddPodcast()
        {
            string title = ReadString("Название подкаста: ");
            int episodes = ReadInt("Количество эпизодов: ");

            var podcast = new Podcast(_nextNumber, title, episodes);
            _playlist.Add(podcast);
            _nextNumber++;
            Console.WriteLine("Подкаст добавлен.");
        }

        private static void AddAudiobook()
        {
            string title = ReadString("Название аудиокниги: ");

            var audiobook = new Audiobook(_nextNumber, title);
            _playlist.Add(audiobook);
            _nextNumber++;
            Console.WriteLine("Аудиокнига добавлена.");
        }

        private static void AddRadio()
        {
            string title = ReadString("Название радиостанции: ");
            string url = ReadString("Ссылка на поток (URL): ");

            var radio = new RadioStream(_nextNumber, title, url);
            _playlist.Add(radio);
            _nextNumber++;
            Console.WriteLine("Радиопоток добавлен.");
        }

        private static void ShowAll()
        {
            _playlist.PrintAll();
        }

        private static void ChangeRule()
        {
            Console.WriteLine("Выбери новое правило:");
            Console.WriteLine("1 — По порядку (Sequential)");
            Console.WriteLine("2 — По популярности (Popularity)");
            Console.WriteLine("3 — Случайный выбор (Random)");
            string choice = ReadString("Выбор: ");

            switch (choice)
            {
                case "1": _playlist.SelectionRule = new SequentialRule(); break;
                case "2": _playlist.SelectionRule = new PopularityRule(); break;
                case "3": _playlist.SelectionRule = new RandomRule(); break;
                default: Console.WriteLine("Неверный выбор. Правило не изменено."); return;
            }
            Console.WriteLine("Правило успешно изменено!");
        }

        private static void PlayNext()
        {
            MediaItem next = _playlist.Next();
            if (next == null)
            {
                Console.WriteLine("Плейлист пуст или не удалось выбрать элемент.");
                return;
            }

            // Спрашиваем, сколько раз прослушать (по умолчанию 1)
            int times = ReadInt($"Выбран '{next.Title}'. Сколько раз прослушать?: ");
            if (times <= 0) times = 1;

            // Увеличиваем счетчик прослушиваний
            _playlist.PlayCurrent(times);

            Console.WriteLine($"\nЭлемент: {next.Title}");
            Console.WriteLine($"Детали: {next.GetDetails()}");
            Console.WriteLine($"[Счётчик прослушиваний увеличен на {times}. Текущее значение: {next.PlayCount}]");
        }

        private static void RemoveByNumber()
        {
            int number = ReadInt("Номер элемента для удаления: ");
            bool ok = _playlist.Remove(number);
            Console.WriteLine(ok ? "Удалено." : "Элемент с таким номером не найден.");
        }
    }

    /*
     * ПАМЯТКА ПО АРХИТЕКТУРЕ (ЛР2):
     * 
     * 1. Связи: Playlist агрегирует MediaItem (передаются извне). Track композиционно содержит Duration.
     * 2. Иерархия: MediaItem (abstract) -> Track, Podcast, Audiobook, RadioStream.
     * 3. Интерфейс: ISelectionRule -> SequentialRule, PopularityRule, RandomRule.
     * 4. Композиция вместо наследования: Вместо PremiumTrack используются поля Medium, AccessLevel, AgeRating.
     */
}
