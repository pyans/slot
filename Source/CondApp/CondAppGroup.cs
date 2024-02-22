using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CondAppGroup")]
public class CondAppGroup : ScriptableObject
{
    //条件装置群のタイプ
    public enum CondAppGroupTag
    {
        BELL = 1,
        REP = 2,
        CHERRY = 4,
        WTML = 8,
        RARE = 16,
        SR = 32,
        UR = 64,
        BONUS = 128,
        ORDER = 256
    };
    //条件装置群データ
    public string CondAppGroupName;
    public int typeofCondApp;                                         //条件装置群の役割(0…ハズレ、1…小役、2…レア、3…確定)
    [SerializeField]
    public List<CondApp> CondApps = new List<CondApp>();
    //停止順ごとの停止制御
    //各リストは停止制御を持つ
    public List<StopBehavior> stopBh_1st = new List<StopBehavior>();  //第一停止が左、中、右の3通り
    public List<StopBehavior> stopBh_2nd = new List<StopBehavior>();  //押し順６通り
    public List<StopBehavior> stopBh_3rd = new List<StopBehavior>();  //押し順６通り
    //特殊停止制御
    //特定のリールが特定の位置の場合に作動
    public List<SpecialStop> specialStops = new List<SpecialStop>();
}
