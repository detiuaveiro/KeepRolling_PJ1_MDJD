using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "PageData", menuName = "ScriptableObjects/PageData", order = 1)]
public class PageData : ScriptableObject
{
    public string page_text;
    public VideoClip video_clip;
}
