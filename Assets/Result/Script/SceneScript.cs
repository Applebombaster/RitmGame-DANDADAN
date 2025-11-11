using TMPro;
using UnityEngine;
using GamePlay.Script;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour
{
    [Header("Result Display")]
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text accuracyText;
    public TMP_Text perfectText;
    public TMP_Text goodText;
    public TMP_Text missText;
    public Image accuracyBar;

    [Header("History")]
    public TableSpawner historySpawner;

    private void Start()
    {
        // Отображаем основные результаты
        scoreText.text = Date.PreviousScore.ToString("0000000");
        comboText.text = "" + Date.Combo;

        // Рассчитываем точность
        float accuracy = 0f;
        if (Date.TotalNotes > 0)
        {
            accuracy = (Date.PerfectHits * 1f + Date.GoodHits * 0.5f) / Date.TotalNotes;
        }
        accuracyText.text = accuracy.ToString("#0.##%");

        // Заполняем полосу точности
        if (accuracyBar != null)
        {
            accuracyBar.fillAmount = accuracy;

            // Цвет в зависимости от точности
            if (accuracy >= 1.0f)
                accuracyBar.color = Color.yellow;
            else if (accuracy >= 0.75f)
                accuracyBar.color = Color.green;
            else if (accuracy >= 0.5f)
                accuracyBar.color = Color.blue;
            else if (accuracy >= 0.25f)
                accuracyBar.color = Color.magenta;
            else
                accuracyBar.color = Color.red;
        }

        // Отображаем статистику попаданий
        perfectText.text = Date.PerfectHits.ToString();
        goodText.text = Date.GoodHits.ToString();
        missText.text = Date.Misses.ToString();

        // Инициализируем историю рекордов
        if (historySpawner != null)
        {
            historySpawner.SpawnTables();
        }
    }

    public void ChooseScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChooseMap");
    }
}