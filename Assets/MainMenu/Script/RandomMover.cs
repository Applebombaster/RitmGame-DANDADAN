using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public float moveSpeed = 1f; // Скорость движения
    public float rotationSpeed = 50f; // Скорость вращения
    private Vector2 direction;

    void Start()
    {
        // Задаем случайное направление
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        // Двигаем объект
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Вращаем объект
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Получаем размеры канваса
        RectTransform canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // Получаем размеры объекта
        RectTransform rectTransform = GetComponent<RectTransform>();
        float objectWidth = rectTransform.rect.width / 2;
        float objectHeight = rectTransform.rect.height / 2;

        // Проверяем столкновение с границами канваса
        if (transform.localPosition.x < -canvasSize.x / 2 + objectWidth || transform.localPosition.x > canvasSize.x / 2 - objectWidth ||
            transform.localPosition.y < -canvasSize.y / 2 + objectHeight || transform.localPosition.y > canvasSize.y / 2 - objectHeight)
        {
            // Отталкиваем в случайном направлении
            direction = Random.insideUnitCircle.normalized;
            // Перемещаем объект немного внутрь канваса, чтобы избежать застревания
            transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, -canvasSize.x / 2 + objectWidth, canvasSize.x / 2 - objectWidth),
                                                   Mathf.Clamp(transform.localPosition.y, -canvasSize.y / 2 + objectHeight, canvasSize.y / 2 - objectHeight));
        }
    }
}