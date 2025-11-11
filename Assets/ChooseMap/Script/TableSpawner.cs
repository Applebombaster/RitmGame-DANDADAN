using UnityEngine;
using GamePlay.Script;
using System.Linq;
using System.Collections.Generic;

public class TableSpawner : MonoBehaviour
{
    public GameObject tablePrefab;
    public float spacing = 10f;
    private const int maxRecords = 5;

    void Start()
    {
        // Устанавливаем текущий уровень
        Date.CurrentLevel = Date.NameSong;

        LoadRecords();
    }

    // Делаем метод публичным для вызова из SceneScript
    public void SpawnTables()
    {
        // Очищаем существующие записи
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var parentRectTransform = GetComponent<RectTransform>();

        // Получаем рекорды для текущего уровня
        int[] records = new int[0];
        if (Date.LevelRecords != null && Date.LevelRecords.ContainsKey(Date.CurrentLevel))
        {
            records = Date.LevelRecords[Date.CurrentLevel];
        }

        // Создаем таблицы для всех записей
        for (int i = 0; i < 5; i++)
        {
            var position = new Vector3(0, -i * spacing, 0);
            var table = Instantiate(tablePrefab, parentRectTransform);
            table.GetComponent<RectTransform>().anchoredPosition = position;

            // Передаем индекс записи и флаг активности
            bool isActive = i < records.Length && records[i] > 0;
            table.GetComponent<tableScript>().SetRecord(i, isActive);
        }
    }

    private void LoadRecords()
    {
        // Загружаем все рекорды
        string allRecordsJson = PlayerPrefs.GetString("AllLevelRecords");
        if (!string.IsNullOrEmpty(allRecordsJson))
        {
            try
            {
                LevelRecords saveData = JsonUtility.FromJson<LevelRecords>(allRecordsJson);
                if (saveData != null)
                {
                    // Исправленная строка: используем ToDictionarySafe()
                    Date.LevelRecords = saveData.ToDictionarySafe();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading records: {e.Message}");
            }
        }
        else
        {
            Date.LevelRecords = new Dictionary<string, int[]>();
        }
    }
}