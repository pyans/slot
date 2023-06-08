using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create LotteryData")]
public class LotteryData : ScriptableObject
{
    //条件装置データ
    [System.SerializableAttribute]
    public class Lot
    {
        public CondAppGroup condAppGroup;
        public List<int> weight = new List<int>();
    }

    [SerializeField]
    public List<Lot> lotteryData = new List<Lot>();
    public CondAppDataBase effectiveCondApp;
    public int betlevel;
}
