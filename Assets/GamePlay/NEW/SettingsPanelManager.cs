using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SettingsPanelManager : MonoBehaviour
{
    [Header("Main References")]
    public GameObject settingsPanel;
    public AudioSource menuMusicSource;

    [Header("Audio Settings")]
    public Slider musicSlider;
    public Slider soundsSlider;

    [Header("Toggle Settings")]
    public Button visualEffectsButton;
    public RectTransform visualEffectsTochka;
    public Button liveWallpaperButton;
    public RectTransform liveWallpaperTochka;

    [Header("Navigation")]
    public Button backButton;
    public Button settingsButton; // Кнопка в главном меню

    private bool visualEffectsEnabled = true;
    private bool liveWallpaperEnabled = true;

    void Start()
    {
        // Загрузка сохраненных состояний
        LoadSettings();

        // Настройка обработчиков
        settingsButton.onClick.AddListener(OpenSettings);
        backButton.onClick.AddListener(CloseSettings);
        visualEffectsButton.onClick.AddListener(ToggleVisualEffects);
        liveWallpaperButton.onClick.AddListener(ToggleLiveWallpaper);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundsSlider.onValueChanged.AddListener(SetSoundsVolume);

        // Применение начальных состояний
        UpdateToggleVisualEffects();
        UpdateToggleLiveWallpaper();
    }

    private void LoadSettings()
    {
        // Загрузка значений
        visualEffectsEnabled = PlayerPrefs.GetInt("VisualEffectsEnabled", 1) == 1;
        liveWallpaperEnabled = PlayerPrefs.GetInt("LiveWallpaperEnabled", 1) == 1;
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float soundsVolume = PlayerPrefs.GetFloat("SoundsVolume", 0.75f);

        musicSlider.value = musicVolume;
        soundsSlider.value = soundsVolume;

        SetMusicVolume(musicVolume);
        SetSoundsVolume(soundsVolume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        // Обновляем настройки при открытии
        LoadSettings();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        PlayerPrefs.Save(); // Сохраняем настройки
    }

    public void ToggleVisualEffects()
    {
        visualEffectsEnabled = !visualEffectsEnabled;
        PlayerPrefs.SetInt("VisualEffectsEnabled", visualEffectsEnabled ? 1 : 0);
        UpdateToggleVisualEffects();
    }

    private void UpdateToggleVisualEffects()
    {
        if (visualEffectsTochka == null) return;
        float xPos = visualEffectsEnabled ? 40f : -40f;
        visualEffectsTochka.anchoredPosition = new Vector2(xPos, visualEffectsTochka.anchoredPosition.y);
    }

    public void ToggleLiveWallpaper()
    {
        liveWallpaperEnabled = !liveWallpaperEnabled;
        PlayerPrefs.SetInt("LiveWallpaperEnabled", liveWallpaperEnabled ? 1 : 0);
        UpdateToggleLiveWallpaper();
    }

    private void UpdateToggleLiveWallpaper()
    {
        if (liveWallpaperTochka == null) return;
        float xPos = liveWallpaperEnabled ? 40f : -40f;
        liveWallpaperTochka.anchoredPosition = new Vector2(xPos, liveWallpaperTochka.anchoredPosition.y);
    }

    public void SetMusicVolume(float volume)
    {
        if (menuMusicSource != null)
        {
            menuMusicSource.volume = volume;
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSoundsVolume(float volume)
    {
        // Для звуков UI (щелчки кнопок)
        PlayerPrefs.SetFloat("SoundsVolume", volume);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}