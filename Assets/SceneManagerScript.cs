using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using GamePlay.Script;
public class SceneManagerScript : MonoBehaviour
{
    public Dictionary<string, string> nameVideo = new Dictionary<string, string>()
    {
        { "Sunset_butttttttt", "Phone1" },
        { "Rainy Days", "Phone2" },
        { "Upbeat Inspiration", "Phone3" },
        { "Tribes", "Phone4" }
    };
    public void SwitchScreen(string nameSong)
    {
        // Инициализируем если нужно
        if (Date.LevelRecords == null)
        {
            Date.LevelRecords = new Dictionary<string, int[]>();
        }

        // Гарантируем запись для уровня
        if (!Date.LevelRecords.ContainsKey(nameSong))
        {
            Date.LevelRecords[nameSong] = new int[5];
        }

        Date.NameSong = nameSong;
        Date.NameVideo = nameVideo.ContainsKey(nameSong) ? nameVideo[nameSong] : "";
        SceneManager.LoadScene("GamePlay");
    }
}
