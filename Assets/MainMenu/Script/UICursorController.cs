using UnityEngine;
using UnityEngine.UI;

public class UICursorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Vector2 cursorOffset = new Vector2(10, -10);

    private void Awake()
    {
        // Делаем Canvas неуничтожаемым
        DontDestroyOnLoad(gameObject);

        // Находим компонент Image внутри префаба
        if (cursorImage == null)
        {
            cursorImage = GetComponentInChildren<Image>();
        }

        Cursor.visible = false;
    }

    private void Update()
    {
        // Обновляем позицию
        cursorImage.rectTransform.position = Input.mousePosition + (Vector3)cursorOffset;

        // Принудительно скрываем системный курсор каждый кадр
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }
}