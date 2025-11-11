using System.Collections;
using System.Collections.Generic;
using GamePlay.Script;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    public VideoPlayer video;

    public void Start()
    {
        video.clip = Resources.Load<VideoClip>(Date.NameVideo);
    }
}
