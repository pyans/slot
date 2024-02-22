using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create sublottery")]
public class SubLot : ScriptableObject
{
    //条件装置データ
    [System.SerializableAttribute]
    public class OneLot
    {
        public string comment;  //コメント
        public List<int> sublotdata = new List<int>();  //抽せんデータ
    }

    public string lotname;      //抽せん名
    public string comment;      //コメント
    [SerializeField]
    public List<OneLot> sublotdatas = new List<OneLot>();   //抽せんデータのリスト
}
