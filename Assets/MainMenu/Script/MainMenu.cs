using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Не забудь добавить это пространство имен для работы со сценами

public class MainMenu : MonoBehaviour
{
    // Метод для перехода к сцене выбора карты
    public void PlayGame()
    {
        SceneManager.LoadScene("ChooseMap"); // Убедитесь, что имя сцены совпадает с вашим
    }

    // Метод для перехода к сцене настроек
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); // Убедитесь, что имя сцены совпадает с вашим
    }

    // Метод для выхода из игры
    public void ExitGame()
    {
        Application.Quit(); // Завершает игру
        Debug.Log("Выход из игры"); // Для отладки, чтобы увидеть, что метод был вызван
    }
}