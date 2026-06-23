using UnityEngine;
using YG; // Пространство имен плагина

public class LevelUnlocker : MonoBehaviour
{
    // --- Конфигурация (заполни здесь свои ID!) ---
    [Header("Настройка")]
    [SerializeField] private string level3AdID = "ID_ТВОЕГО_РЕКЛАМНОГО_БЛОКА";
    [SerializeField] private string level4PurchaseID = "unlock_level_4"; // ID товара

    [Header("Ключи для сохранения в Player Stats")]
    [SerializeField] private string level3Key = "level_3_unlocked";
    [SerializeField] private string level4Key = "level_4_unlocked";

    // --- Событие для обновления UI ---
    public static System.Action OnLevelUnlocked;

    private void OnEnable()
    {
        // Подписываемся на события покупок
        YG2.onPurchaseSuccess += OnPurchaseSuccess;
        YG2.onPurchaseFailed += OnPurchaseFailed;
    }

    private void OnDisable()
    {
        // Отписываемся
        YG2.onPurchaseSuccess -= OnPurchaseSuccess;
        YG2.onPurchaseFailed -= OnPurchaseFailed;
    }

    // 1. Попытка разблокировать 3-й уровень через рекламу
    public void TryUnlockLevel3()
    {
        if (IsLevelUnlocked(3))
        {
            Debug.Log("Уровень 3 уже разблокирован!");
            return;
        }

        // --- ИСПРАВЛЕНО: Используем YG2.RewardedAdvShow ---
        // Метод принимает ID рекламного блока и callback-функцию
        YG2.RewardedAdvShow(level3AdID, () => {
            // Этот код выполнится, если игрок досмотрел рекламу до конца
            UnlockLevel(3);
            OnLevelUnlocked?.Invoke();
            Debug.Log("Уровень 3 разблокирован за просмотр рекламы!");
        });
    }

    // 2. Попытка разблокировать 4-й уровень через покупку
    public void TryUnlockLevel4()
    {
        if (IsLevelUnlocked(4))
        {
            Debug.Log("Уровень 4 уже разблокирован!");
            return;
        }

        // --- ИСПРАВЛЕНО: Используем YG2.BuyPayments ---
        // Метод принимает ID товара
        YG2.BuyPayments(level4PurchaseID);
        // Результат придет в обработчики onPurchaseSuccess / onPurchaseFailed
    }

    // --- Приватные методы ---

    private bool IsLevelUnlocked(int levelIndex)
    {
        string key = GetKeyForLevel(levelIndex);
        // YG2.GetState читает значение из облачного сохранения
        return YG2.GetState(key) == 1;
    }

    private void UnlockLevel(int levelIndex)
    {
        string key = GetKeyForLevel(levelIndex);
        YG2.SetState(key, 1);
    }

    private string GetKeyForLevel(int levelIndex)
    {
        if (levelIndex == 3) return level3Key;
        if (levelIndex == 4) return level4Key;
        return "";
    }

    // --- Обработчики событий покупок ---
    private void OnPurchaseSuccess(string purchasedID)
    {
        if (purchasedID == level4PurchaseID)
        {
            UnlockLevel(4);
            OnLevelUnlocked?.Invoke();
            Debug.Log("Уровень 4 успешно куплен!");
        }
    }

    private void OnPurchaseFailed(string purchasedID)
    {
        Debug.Log($"Покупка {purchasedID} не удалась.");
    }
}