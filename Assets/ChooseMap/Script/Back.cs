using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ �������� ��� ������������ ���� ��� ������ �� �������

public class Back : MonoBehaviour
{
    // ����� ��� �������� � ����� ������ �����
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // ���������, ��� ��� ����� ��������� � �����
    }

    public void Play()
    {
        SceneManager.LoadScene("GamePlay"); // ���������, ��� ��� ����� ��������� � �����
    }

}
