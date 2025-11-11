using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMover : MonoBehaviour
{
    public float moveDistance = 10f; // Максимальное расстояние перемещения
    public float moveSpeed = 2f; // Скорость движения
    private Vector2 startPosition;

    void Start()
    {
        // Сохраняем начальную позицию
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // Вычисляем новое смещение на основе синусоиды
        float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        // Обновляем позицию объекта
        transform.localPosition = new Vector2(startPosition.x, newY);
    }
}
