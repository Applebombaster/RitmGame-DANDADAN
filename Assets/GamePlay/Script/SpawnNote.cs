using System.Linq;
using UnityEngine;


namespace GamePlay.Script
{
    public class SpawnNote : MonoBehaviour
    {
        [Header("References")] public AudioSource audioSource;
        public LogicScript logic;

        [Header("Settings")] private float time;
        private float timeStart;

        public GameObject note;
        public GameObject longNote;
        public GameObject spinner;
        public GameObject spinnerAnim;

        private float[] noteTiming;
        private (float start, float end)[] longNoteTiming;
        private (float start, float end)[] spinnerTiming;
        private string listJson;
        private float dspSongTime;
        private float songPosition;
        private float songPositionPause;
        private float songLength;
        private int[] index = { 0, 0, 0 };

        private void Start()
        {
            audioSource.clip = Resources.Load<AudioClip>(Date.NameSong);
            listJson = Resources.Load<TextAsset>($"{Date.NameSong}_timing").ToString();
            TimingArrayNormal();
            timeStart = 0;
            
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            dspSongTime = (float)AudioSettings.dspTime;
            songLength = audioSource.clip.length;
            audioSource.Play();
            audioSource.time = timeStart;
            // Date.MaxScore = timingList.Length * 200 + timingList.Length / 2 * timingList.Length;
        }

        private void Update()
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime);
            var progress = (audioSource.time - timeStart) / (audioSource.clip.length - timeStart);
            logic.UpdateProgressBar(progress);

            SpawnNotes();
            if (songPosition > songLength)
                logic.EndSong();
        }

        private void SpawnNotes()
        {
            if (index[0] < noteTiming.Length && songPosition > noteTiming[index[0]] - 10.0f / 6.0f)
            {
                Instantiate(note);
                index[0]++;
            }

            if (index[1] < longNoteTiming.Length && songPosition > longNoteTiming[index[1]].start - 10.0f / 6.0f)
            {
                Instantiate(longNote);
                GameObject.FindGameObjectWithTag("LongNote").GetComponent<LongNoteScript>().timeLive =
                    longNoteTiming[index[1]].end - longNoteTiming[index[1]].start;
                index[1]++;
            }

            if (index[2] < spinnerTiming.Length && songPosition > spinnerTiming[index[2]].start - 10.0f / 6.0f)
            {
                Instantiate(spinner);
                Instantiate(spinnerAnim);
                GameObject.FindGameObjectWithTag("Spinner").GetComponent<SpinnerScript>().timeALive =
                    spinnerTiming[index[2]].end - spinnerTiming[index[2]].start;
                index[2]++;
            }
        }

        public void SetVolume(float volume)
        {
            if (audioSource)
            {
                audioSource.volume = volume;
            }
        }

        public void Pause(bool flag)
        {
            if (flag)
            {
                songPositionPause = songPosition;
                dspSongTime = float.MaxValue;
            }
            else
                dspSongTime = (float)AudioSettings.dspTime - songPositionPause;
        }

        private void TimingArrayNormal()
        {
            var timing = JsonUtility.FromJson<NoteRecord<float>>(listJson);
            noteTiming = timing.Note;
            longNoteTiming = ParsingTiming(timing.LongNote);
            spinnerTiming = ParsingTiming(timing.Spinner);
        }

        private (float start, float end)[] ParsingTiming(string[] timings)
        {
            return timings.Select(x =>
            {
                var a = x.Split()
                    .Select(float.Parse)
                    .ToArray();
                return (a[0], a[1]);
            }).ToArray();
        }
    }
}