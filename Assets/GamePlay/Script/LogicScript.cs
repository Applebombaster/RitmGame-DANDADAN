using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

namespace GamePlay.Script
{
    public class LogicScript : MonoBehaviour
    {
        // Новая статистика
        private int perfectHits = 0;   // 100 очков
        private int goodHits = 0;      // 50 очков
        private int misses = 0;        // Промахи

        public TMP_Text scoreText;
        public TMP_Text comboText;
        public GameObject progressBar;
        public AudioSource audioSource;
        public AudioClip missSound;

        // ����������� ���� (�����)
        [Header("Effects")] public GameObject score100Prefab;
        public GameObject score50Prefab;
        public GameObject missXPrefab;
        public List<GameObject> starPrefabs = new List<GameObject>();

        [Header("References")] public Transform shieldTransform;
        private GameObject currentEffect;
        private int score = 0;
        private int combo = 0;
        private int maxCombo = 0;
        private bool visualEffectsEnabled = true; // ���� ��������� ���������� �������� (���������)
        // Добавляем константу для максимального количества записей
        private const int maxRecords = 5;


        private void Awake()
        {
            visualEffectsEnabled = PlayerPrefs.GetInt("VisualEffectsEnabled", 1) == 1;
            if (shieldTransform == null)
            {
                var shield = GameObject.FindGameObjectWithTag("Shield");
                if (shield)
                    shieldTransform = shield.transform;
            }
        }

        // �������� ����� ���������� ����� (�������������)
        public void AddScore(float distance)
        {
            if (distance < 0.7f)
            {
                score += 100 + combo++;
                ShowScoreEffect(score100Prefab);
                CreateStars();
                perfectHits++; // Новая статистика
            }
            else if (distance < 2.6f)
            {
                if (combo > maxCombo)
                    maxCombo = combo;
                combo = 0;
                score += 50;
                ShowScoreEffect(score50Prefab);
                goodHits++; // Новая статистика
            }
            else
            {
                ShowMissEffect();
                misses++; // Новая статистика
            }

            UpdateScore(); // �������� �����
        }

        public void EffectSpinner(int countRotate, bool isEnd)
        {
            Debug.Log(countRotate);
            //МЕСТО ДЛЯ АНИМАЦИИ ИЗМЕНЕНИЯ ИКСОВ   
            if (isEnd)
            {
                score += (countRotate * 100);
                UpdateScore();
            }
        }

        // ���������: ����� ��� ����������� ������� �������
        public void ShowMissEffect()
        {
            ReplaceEffect(missXPrefab);
            if (audioSource != null && missSound != null)
            {
                audioSource.PlayOneShot(missSound);
            }
        }

        private void ShowScoreEffect(GameObject prefab)
        {
            ReplaceEffect(prefab);
        }

        private void ReplaceEffect(GameObject newEffectPrefab)
        {
            if (currentEffect != null)
            {
                Destroy(currentEffect);
            }

            var center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10));
            currentEffect = Instantiate(newEffectPrefab, center, Quaternion.identity);

            StartCoroutine(DestroyAfterDelay(currentEffect, 0.5f));
        }
        private IEnumerator DestroyAfterDelay(GameObject effect, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (effect != null)
            {
                Destroy(effect);
            }
        }

        // ���������: ��������� ��������� ������ (���������� ����������)
        public void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
        public void SetVisualEffectsEnabled(bool enabled)
        {
            visualEffectsEnabled = enabled;
        }

        private void CreateStars()
        {
            if (!visualEffectsEnabled) return; // �������� �����

            if (shieldTransform == null) return;

            var starCount = UnityEngine.Random.Range(1, 12);
            var shieldPos = shieldTransform.position;

            for (int i = 0; i < starCount; i++)
            {
                if (starPrefabs.Count == 0) break;

                GameObject starPrefab = starPrefabs[UnityEngine.Random.Range(0, starPrefabs.Count)];
                GameObject star = Instantiate(starPrefab, shieldPos, Quaternion.identity);
                star.AddComponent<StarEffect>();
            }
        }

        public void EndSong()
        {
            if (combo > maxCombo)
                maxCombo = combo;

            // Сохраняем статистику
            Date.PerfectHits = perfectHits;
            Date.GoodHits = goodHits;
            Date.Misses = misses;
            Date.TotalNotes = perfectHits + goodHits + misses;
            
            // Сохраняем результат
            Date.PreviousScore = score;
            Date.Combo = maxCombo;

            // Обновляем рекорды
            LoadRecords();
            UpdateRecords(score);
            SaveRecords();

            SceneManager.LoadScene("Result");
        }

        private void UpdateRecords(int newScore)
        {
            if (string.IsNullOrEmpty(Date.NameSong))
            {
                Debug.LogError("Level name is not set!");
                return;
            }

            // Гарантируем наличие записи для уровня
            if (!Date.LevelRecords.ContainsKey(Date.NameSong))
            {
                Date.LevelRecords[Date.NameSong] = new int[5];
            }

            // Получаем текущие записи
            List<int> recordsList = Date.LevelRecords[Date.NameSong].ToList();
            new List<int>();

            // Добавляем новый результат
            recordsList.Add(newScore);

            // Сортируем и берем топ-5
            var sortedRecords = recordsList
                .OrderByDescending(r => r)
                .Take(maxRecords)
                .ToList();

            // Дополняем нулями если нужно
            while (sortedRecords.Count < maxRecords)
            {
                sortedRecords.Add(0);
            }

            // Обновляем словарь
            Date.LevelRecords[Date.NameSong] = sortedRecords.ToArray();
        }

        public void UpdateProgressBar(float value)
        {
            if (progressBar != null)
            {
                Slider slider = progressBar.GetComponent<Slider>();
                if (slider != null)
                {
                    slider.value = value;
                }
            }
        }

        // �������� ����� ���������� UI ����� (��� ���������)
        private void UpdateScore()
        {
            if (scoreText != null)
                scoreText.text = score.ToString("0000000");

            if (comboText != null)
                comboText.text = "X" + combo;
        }

        private void LoadRecords()
        {
            string allRecordsJson = PlayerPrefs.GetString("AllLevelRecords");

            // Всегда инициализируем словарь
            Date.LevelRecords = new Dictionary<string, int[]>();

            if (!string.IsNullOrEmpty(allRecordsJson))
            {
                try
                {
                    LevelRecords saveData = JsonUtility.FromJson<LevelRecords>(allRecordsJson);
                    if (saveData != null)
                    {
                        Date.LevelRecords = saveData.ToDictionarySafe();
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error loading records: {e.Message}");
                }
            }

            // Гарантируем что текущий уровень есть в словаре
            if (!string.IsNullOrEmpty(Date.NameSong) &&
                !Date.LevelRecords.ContainsKey(Date.NameSong))
            {
                Date.LevelRecords[Date.NameSong] = new int[5];
            }
        }

        private void SaveRecords()
        {
            // Убедитесь что словарь инициализирован
            if (Date.LevelRecords == null)
            {
                Debug.LogWarning("LevelRecords is null during save!");
                Date.LevelRecords = new Dictionary<string, int[]>();
            }

            // Создаем объект для сохранения
            LevelRecords saveData = new LevelRecords(Date.LevelRecords);

            // Сериализуем
            string allRecordsJson = JsonUtility.ToJson(saveData, true);

            // Логируем для отладки
            Debug.Log($"Saving JSON: {allRecordsJson}");
            Debug.Log($"Keys count: {saveData.Keys?.Length}, Values count: {saveData.Values?.Length}");

            PlayerPrefs.SetString("AllLevelRecords", allRecordsJson);
            PlayerPrefs.Save();
        }

        [System.Serializable]
        public class LevelRecords
        {
            public string[] Keys;
            public SupportClass[] Values; // Исправлено!

            [System.Serializable]
            public class SupportClass
            {
                public int[] Item;

                public SupportClass() { }

                public SupportClass(int[] array)
                {
                    Item = array;
                }
            }

            public LevelRecords()
            {
                Keys = new string[0];
                Values = new SupportClass[0];
            }

            public LevelRecords(Dictionary<string, int[]> dict)
            {
                if (dict == null)
                {
                    Keys = new string[0];
                    Values = new SupportClass[0];
                    return;
                }

                Keys = dict.Keys.ToArray();
                Values = dict.Values.Select(v => new SupportClass(v)).ToArray();
            }

            public Dictionary<string, int[]> ToDictionarySafe()
            {
                var result = new Dictionary<string, int[]>();
                if (Keys == null || Values == null)
                    return result;

                int count = Mathf.Min(Keys.Length, Values.Length);
                for (int i = 0; i < count; i++)
                {
                    if (Keys[i] != null && Values[i] != null)
                    {
                        result[Keys[i]] = Values[i].Item;
                    }
                }
                return result;
            }
        }
    }
}