using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Effect/Create LightPattern")]

public class LightPattern : ScriptableObject
{
    //条件装置データ
    [System.SerializableAttribute]
    public class OneLightPattern
    {
        public List<int> pattern = new List<int>();
        public float sec = 1.0f / 60.0f;
    }

    [SerializeField]
    List<OneLightPattern> onepattern = new List<OneLightPattern>();
    public string comment;
    public bool isrepeat = false;       //パターンを繰り返すか
    public bool isclear = false;        //更新時消灯を解除するか
    public float repeatsec = 0;

    public List<OneLightPattern> GetPatterns()
    {
        return onepattern;
    }
}
