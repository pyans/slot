using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StopBehavior")]
public class StopBehavior : ScriptableObject
{
    //停止制御データ

    public int id;      //停止制御ID
    public string comment;  //コメント

    [SerializeField]
    public List<int> slide = new List<int>();
}
