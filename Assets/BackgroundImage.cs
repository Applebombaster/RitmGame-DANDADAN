using GamePlay.Script;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] backgroundSprites; // Массив спрайтов для всех уровней
    [SerializeField] private string[] spriteNames; // Имена, соответствующие Date.NameVideo

    void Start()
    {
        string imageName = Date.NameVideo;
        if (string.IsNullOrEmpty(imageName))
        {
            Debug.LogWarning("Имя картинки не задано в Date.NameVideo");
            return;
        }

        // Находим спрайт по имени
        Sprite selectedSprite = null;
        for (int i = 0; i < spriteNames.Length; i++)
        {
            if (spriteNames[i] == imageName)
            {
                selectedSprite = backgroundSprites[i];
                break;
            }
        }

        if (selectedSprite == null)
        {
            Debug.LogError($"Спрайт для {imageName} не найден в массиве!");
            return;
        }

        spriteRenderer.sprite = selectedSprite;
        FitToScreen();
        spriteRenderer.sortingOrder = -10;
    }

    private void FitToScreen()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        float scaleX = screenWidth / spriteWidth;
        float scaleY = screenHeight / spriteHeight;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}