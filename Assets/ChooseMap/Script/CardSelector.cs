using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Linq;
using GamePlay.Script;

public class CardSelector : MonoBehaviour
{
    [Header("Records Display")]
    public RectTransform recordsPanel; // Изменили на RectTransform
    public GameObject recordItemPrefab;
    public RectTransform recordsContainer; // Изменили на RectTransform
    public float spacing = 105f;

    private List<GameObject> currentRecords = new List<GameObject>();

    // Для отладки
    public bool alwaysShowTestData = false;

    private GameObject currentCard;
    private string currentLevelName;

    public Dictionary<string, string> nameVideo = new Dictionary<string, string>()
    {
        { "Card", "Phone1" },
        { "Card (1)", "Phone2" },
        { "Card (2)", "Phone3" },
        { "Card (3)", "Phone4" }
    };

    public GameObject cardPrefab;
    public Transform content;
    public string[] cardNames;
    public ScrollRect scrollRect;

    [Header("Background Settings")] public GameObject mapBackground;
    public VideoPlayer videoBackground;

    private int _firstCardIndex = 0;
    // Добавляем переменные для хранения оригинальных параметров
    private Vector3 originalScale;
    private Vector3 originalPosition;


    void Start()
    {
        // Фиксируем положение панели рекордов
        if (recordsPanel != null)
        {
            // Пример: левая часть экрана
            recordsPanel.anchorMin = new Vector2(0, 0.5f);
            recordsPanel.anchorMax = new Vector2(0, 0.5f);
            recordsPanel.pivot = new Vector2(0, 0.5f);
            recordsPanel.anchoredPosition = new Vector2(50, 0);
        }

        LoadCards();
    }

    void LoadCards()
    {
        if (cardPrefab == null)
        {
            Debug.LogError("Card Prefab �� ��������!");
            return;
        }

        if (content == null)
        {
            Debug.LogError("Content �� ��������!");
            return;
        }

        var i = 0;
        foreach (var cardName in cardNames)
        {
            GameObject card = Instantiate(cardPrefab, content);

            // ИСПРАВЛЕНИЕ: используем TMP_Text вместо Text
            TMP_Text cardText = card.GetComponentInChildren<TMP_Text>();
            if (cardText != null)
            {
                cardText.text = cardName;
            }
            else
            {
                Debug.LogWarning("��������� Text �� ������ �� ��������: " + cardName);
            }

            Button button = card.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnCardSelected(cardName));
            }
            EventTrigger eventTrigger = card.AddComponent<EventTrigger>();
            EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
            entryPointerEnter.eventID = EventTriggerType.PointerEnter;
            entryPointerEnter.callback.AddListener((data) => { OnPointerEnter(card); });
            eventTrigger.triggers.Add(entryPointerEnter);

            EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
            entryPointerExit.eventID = EventTriggerType.PointerExit;
            entryPointerExit.callback.AddListener((data) => { OnPointerExit(card); });
            eventTrigger.triggers.Add(entryPointerExit);
            i++;
        }
        CenterScrollView();
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            scrollRect.verticalNormalizedPosition += scrollInput;
        }
    }

    void CenterScrollView()
    {
        if (content.childCount > 0)
        {
            RectTransform firstCardRect = content.GetChild(0).GetComponent<RectTransform>();

            float contentHeight = content.GetComponent<RectTransform>().rect.height;
            float cardHeight = firstCardRect.rect.height;

            float targetPosition = (cardHeight / 2) - (contentHeight / 2);
            float scrollPosition = firstCardRect.anchoredPosition.y + targetPosition;

            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, scrollPosition);
        }
    }

    void OnCardSelected(string cardName)
    {
        Debug.Log("������� �����: " + cardName);
    }

    void OnPointerEnter(GameObject card)
    {
        // Сохраняем оригинальные параметры
        originalScale = card.transform.localScale;
        originalPosition = card.transform.localPosition;

        // Увеличиваем карточку (используем мировые координаты)
        card.transform.localScale = new Vector3(1.7f, 1.7f, 1f);

        // Рассчитываем смещение в мировых координатах
        Vector3 worldOffset = new Vector3(-180f, 0f, 0f);
        card.transform.position += worldOffset;

        int cardIndex = card.transform.GetSiblingIndex();
        if (cardIndex == _firstCardIndex)
        {
            // if (mapBackground != null) mapBackground.SetActive(false);
            // if (videoBackground != null)
            // {
                // videoBackground.gameObject.SetActive(true);
                // videoBackground.Play();
                // videoBackground.clip = Resources.Load<VideoClip>(nameVideo[gameObject.name]);
            // }
        }
        // Получаем название уровня
        TMP_Text cardText = card.GetComponentInChildren<TMP_Text>();
        if (cardText != null)
        {
            currentLevelName = cardText.text;
            currentCard = card;

            // Логирование для отладки
            Debug.Log($"Pointer entered: {currentLevelName}");

            // Показываем рекорды
            ShowRecordsForLevel(currentLevelName);
        }
    }

    void OnPointerExit(GameObject card)
    {
        // Восстанавливаем оригинальные параметры
        card.transform.localScale = originalScale;
        card.transform.localPosition = originalPosition;

        int cardIndex = card.transform.GetSiblingIndex();
        if (cardIndex == _firstCardIndex)
        {
            // if (videoBackground != null)
            {
                // videoBackground.Stop();
                // videoBackground.gameObject.SetActive(false);
            }

            // if (mapBackground != null) mapBackground.SetActive(true);
        }
        if (recordsPanel != null)
        {
            recordsPanel.gameObject.SetActive(false);
        }
    }

    private void ShowRecordsForLevel(string levelName)
    {
        // Убираем позиционирование относительно карточки
        // Просто активируем панель в фиксированном месте

        // Очищаем предыдущие записи
        ClearRecords();

        // Загружаем рекорды
        int[] records = LoadRecordsForLevel(levelName);

        // Создаем элементы таблицы
        float containerHeight = 0;

        for (int i = 0; i < 5; i++)
        {
            GameObject recordItem = Instantiate(recordItemPrefab, recordsContainer);
            currentRecords.Add(recordItem);

            // Позиционирование
            RectTransform rect = recordItem.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -i * spacing);

            // Настройка записи
            tableScript table = recordItem.GetComponent<tableScript>();
            if (table != null)
            {
                Date.CurrentLevel = levelName;
                bool isActive = records != null && i < records.Length && records[i] > 0;
                table.SetRecord(i, isActive);

                if (isActive) containerHeight += spacing;
            }
        }

        // Обновляем размер контейнера
        recordsContainer.sizeDelta = new Vector2(recordsContainer.sizeDelta.x, containerHeight);

        // Активируем панель
        recordsPanel.gameObject.SetActive(true);
    }
    private void ClearRecords()
    {
        foreach (GameObject record in currentRecords)
        {
            Destroy(record);
        }
        currentRecords.Clear();
    }

    /* private void ShowTestRecords()
    {
        for (int i = 0; i < recordTexts.Length; i++)
        {
            if (recordTexts[i] != null)
            {
                recordTexts[i].text = $"{i + 1}. {Random.Range(100000, 999999)}";
                recordTexts[i].gameObject.SetActive(true);
            }
        }
        recordsPanel.SetActive(true);
    }
    */
    private int[] LoadRecordsForLevel(string levelName)
    {
        // Если словарь не инициализирован
        if (Date.LevelRecords == null)
        {
            Debug.Log("LevelRecords is null - initializing");
            Date.LevelRecords = new Dictionary<string, int[]>();
        }

        // Проверка наличия уровня
        if (!Date.LevelRecords.ContainsKey(levelName))
        {
            Debug.Log($"No records found for: {levelName}");
            return new int[0];
        }

        return Date.LevelRecords[levelName];
    }
    // Добавьте этот метод в любой скрипт, который запускается при старте игры
    void InitializeLevelRecords()
    {
        // Список всех уровней
        string[] allLevels = { "Sunset_butttttttt", "Rainy Days", "Upbeat Inspiration", "Tribes" };

        // Загружаем существующие записи
        string allRecordsJson = PlayerPrefs.GetString("AllLevelRecords");
        Dictionary<string, int[]> records = new Dictionary<string, int[]>();

        if (!string.IsNullOrEmpty(allRecordsJson))
        {
            try
            {
                LevelRecords saveData = JsonUtility.FromJson<LevelRecords>(allRecordsJson);
                if (saveData != null)
                {
                    records = saveData.ToDictionarySafe();
                }
            }
            catch { /* Игнорируем ошибки */ }
        }

        // Добавляем отсутствующие уровни
        foreach (string level in allLevels)
        {
            if (!records.ContainsKey(level))
            {
                records[level] = new int[5];
            }
        }

        // Сохраняем обратно
        LevelRecords saveDataNew = new LevelRecords(records);
        string newJson = JsonUtility.ToJson(saveDataNew, true);
        PlayerPrefs.SetString("AllLevelRecords", newJson);
        PlayerPrefs.Save();

        // Обновляем глобальное состояние
        Date.LevelRecords = records;
    }
}