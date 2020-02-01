using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "song config", menuName = "socknic/song settings")]
public class SongConfig : ScriptableObject
{
    public int bpm;
    public float approachRate;
    public float hp;
    public AudioClip song;
    public float time => song.length;

    public TextAsset songText;
}
