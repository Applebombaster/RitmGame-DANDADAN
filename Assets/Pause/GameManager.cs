using UnityEngine;
using TMPro;
using GamePlay.Script;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public TMP_Text pauseText;
    private bool isPaused = false;
    public AudioSource musicSource;
    private SpawnNote spawnNote;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        pauseText.gameObject.SetActive(true);
        Invoke("HidePauseText", 4f);
        spawnNote = GameObject.FindGameObjectWithTag("SpawnNote").GetComponent<SpawnNote>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // ������������� �����
        musicSource.Pause(); // ���������������� ������
        pauseMenuPanel.SetActive(true); // ���������� ���� �����
        ControlScript.isPause = true;
        spawnNote.Pause(true);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // ���������� �����
        musicSource.UnPause(); // ������������ ������
        pauseMenuPanel.SetActive(false); // �������� ���� �����
        spawnNote.Pause(false);
        ControlScript.isPause = false;
    }

    void HidePauseText()
    {
        pauseText.gameObject.SetActive(false); // �������� �����
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // ���������� �����
        // ����� �������� ������ ��� ����������� ������
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        // ����� �������� ������ ��� ������ � ������� ����
        // ��������, �������� �������� ����
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChooseMap"); // �������� �� ��� ����� ����� �������� ����
    }
}