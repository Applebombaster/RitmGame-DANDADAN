using GamePlay.Script;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UISettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider musicSlider;
    public Slider soundsSlider;
    public SpawnNote musicController;

    [Header("Video Settings")]
    public Slider blackoutSlider;
    public VideoPlayer videoPlayer;
    public Camera targetCamera;

    [Header("Toggle Settings")]
    public Button visualEffectsButton;
    public RectTransform visualEffectsTochka;
    public Button liveWallpaperButton;
    public RectTransform liveWallpaperTochka;

    [Header("References")]
    public LogicScript logicController;

    private bool visualEffectsEnabled = true;
    private bool liveWallpaperEnabled = true;

    void Start()
    {
        // Загрузка сохраненных состояний
        visualEffectsEnabled = PlayerPrefs.GetInt("VisualEffectsEnabled", 1) == 1;
        liveWallpaperEnabled = PlayerPrefs.GetInt("LiveWallpaperEnabled", 1) == 1;
        float blackoutValue = PlayerPrefs.GetFloat("BlackoutAlpha", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float soundsVolume = PlayerPrefs.GetFloat("SoundsVolume", 0.75f);

        // Настройка обработчиков
        visualEffectsButton.onClick.AddListener(ToggleVisualEffects);
        liveWallpaperButton.onClick.AddListener(ToggleLiveWallpaper);
        blackoutSlider.onValueChanged.AddListener(SetBlackoutAlpha);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundsSlider.onValueChanged.AddListener(SetSoundsVolume);

        // Применение начальных состояний
        UpdateToggleVisualEffects();
        UpdateToggleLiveWallpaper();
        UpdateVideoState();

        // Установка начальных значений
        blackoutSlider.value = blackoutValue;
        musicSlider.value = musicVolume;
        soundsSlider.value = soundsVolume;

        // Применяем значения
        SetBlackoutAlpha(blackoutValue);
        SetMusicVolume(musicVolume);
        SetSoundsVolume(soundsVolume);
    }

    // Визуальные эффекты
    public void ToggleVisualEffects()
    {
        visualEffectsEnabled = !visualEffectsEnabled;
        PlayerPrefs.SetInt("VisualEffectsEnabled", visualEffectsEnabled ? 1 : 0);
        UpdateToggleVisualEffects();

        if (logicController != null)
        {
            logicController.SetVisualEffectsEnabled(visualEffectsEnabled);
        }
    }

    private void UpdateToggleVisualEffects()
    {
        if (visualEffectsTochka == null) return;
        float xPos = visualEffectsEnabled ? 40f : -40f;
        visualEffectsTochka.anchoredPosition = new Vector2(xPos, visualEffectsTochka.anchoredPosition.y);
    }

    // Живые обои
    public void ToggleLiveWallpaper()
    {
        liveWallpaperEnabled = !liveWallpaperEnabled;
        PlayerPrefs.SetInt("LiveWallpaperEnabled", liveWallpaperEnabled ? 1 : 0);
        UpdateToggleLiveWallpaper();
        UpdateVideoState();
    }

    private void UpdateToggleLiveWallpaper()
    {
        if (liveWallpaperTochka == null) return;
        float xPos = liveWallpaperEnabled ? 40f : -40f;
        liveWallpaperTochka.anchoredPosition = new Vector2(xPos, liveWallpaperTochka.anchoredPosition.y);
    }

    private void UpdateVideoState()
    {
        if (videoPlayer == null) return;

        if (liveWallpaperEnabled)
        {
            if (!videoPlayer.isPlaying) videoPlayer.Play();
        }
        else
        {
            if (videoPlayer.isPlaying) videoPlayer.Pause();
        }
    }

    // Затемнение фона
    public void SetBlackoutAlpha(float alpha)
    {
        if (videoPlayer != null && targetCamera != null)
        {
            videoPlayer.targetCamera = targetCamera;
            videoPlayer.targetCameraAlpha = alpha;
        }
        PlayerPrefs.SetFloat("BlackoutAlpha", alpha);
    }

    // Громкость музыки
    public void SetMusicVolume(float volume)
    {
        if (musicController != null)
        {
            musicController.SetVolume(volume);
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Громкость звуков
    public void SetSoundsVolume(float volume)
    {
        if (logicController != null)
        {
            logicController.SetVolume(volume);
        }
        PlayerPrefs.SetFloat("SoundsVolume", volume);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}