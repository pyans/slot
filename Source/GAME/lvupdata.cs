using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/Create LVUPData")]
public class lvupdata : ScriptableObject
{
    //主人公の成長データ
    public List<int> need_EXP = new List<int>();
}
