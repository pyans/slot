using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataBase", menuName = "MyScriptable/AudioDataBase")]
public class Sounds : ScriptableObject
{
    public List<AudioClip> SEList = new List<AudioClip>();
    public List<AudioClip> BGMList = new List<AudioClip>();

    public List<AudioClip> GetBGMs()
    {
        return BGMList;
    }

    public List<AudioClip> GetSEs()
    {
        return SEList;
    }
}
