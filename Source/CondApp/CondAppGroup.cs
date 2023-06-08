using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CondAppGroup")]
public class CondAppGroup : ScriptableObject
{
    //条件装置群データ

    public string CondAppGroupName;
    [SerializeField]
    public List<CondApp> CondApps = new List<CondApp>();
    //停止順ごとの停止制御
    //各リストは停止制御を持つ
    public List<StopBehavior> stopBh_1st = new List<StopBehavior>();  //第一停止が左、中、右の3通り
    public List<StopBehavior> stopBh_2nd = new List<StopBehavior>();  //押し順６通り
    public List<StopBehavior> stopBh_3rd = new List<StopBehavior>();  //押し順６通り

}
