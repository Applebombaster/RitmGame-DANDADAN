using UnityEngine;
using System.Collections;

// ��������� ����� ������ ��� �������� �����
public class StarEffect : MonoBehaviour
{
    public float lifeTime = 1f; // ����� ����� �������
    private Vector3 randomDirection; // ����������� ��������

    void Start()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        transform.localScale = Vector3.zero;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float timer = 0;

        // �������� � ������� ������� �����
        while (timer < lifeTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / lifeTime);
            transform.position += randomDirection * 2f * Time.deltaTime;

            // ��������
            transform.Rotate(0, 0, 180 * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // ����������� ������� ����� ���������� ��������
        Destroy(gameObject);
    }
}