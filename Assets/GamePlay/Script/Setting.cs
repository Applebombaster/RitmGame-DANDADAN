using System.Collections.Generic;

namespace GamePlay.Script
{
    public static class Date
    {
        // Новая статистика
        public static int PerfectHits;
        public static int GoodHits;
        public static int Misses;
        public static int TotalNotes;
        public static float RadiusCircle = 10.0f;
        public static int[] Records = new int[5];
        public static int PreviousScore;
        public static int MaxScore = 100000;
        public static int Combo;
        public static string NameSong;
        public static string NameVideo;
        public static string CurrentLevel; // Для передачи уровня в tableScript
        // Инициализируем словарь при первом доступе
        private static Dictionary<string, int[]> _levelRecords;
        public static Dictionary<string, int[]> LevelRecords
        {
            get => _levelRecords ?? (_levelRecords = new Dictionary<string, int[]>());
            set => _levelRecords = value;
        }

    }
    public class SupportClass<T>
    {
        public T[] Item;

        public SupportClass(T[] item)
        {
            Item = item;
        }
    }
    
    public class NoteRecord<T>
    {
        public T[] Note;
        public string[] LongNote;
        public string[] Spinner;

        public NoteRecord(T[] note, string[] longNote, string[] spinner)
        {
            Note = note;
            LongNote = longNote;
            Spinner = spinner;
        }
    }
}