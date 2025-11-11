using TMPro;
using UnityEngine;
using GamePlay.Script;

public class tableScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text percentText;
    public GameObject activeElements;

    public void SetRecord(int index, bool isActive)
    {
        // Активируем/деактивируем элементы
        activeElements.SetActive(isActive);

        if (!isActive) return;

        // Получаем рекорды для текущего уровня
        int[] records = Date.LevelRecords.ContainsKey(Date.CurrentLevel) ?
            Date.LevelRecords[Date.CurrentLevel] :
            new int[0];

        if (index >= records.Length) return;

        // Обновляем только активные записи
        scoreText.text = "Score: " + records[index].ToString("0000000");

        // Рассчитываем точность как процент от максимального счета
        if (Date.MaxScore > 0)
        {
            float percentage = (float)records[index] / Date.MaxScore;
            percentText.text = percentage.ToString("#0.##%");
        }
        else
        {
            percentText.text = "0%";
        }
    }
}