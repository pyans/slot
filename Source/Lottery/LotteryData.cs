using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create LotteryData")]
public class LotteryData : ScriptableObject
{
    //条件装置抽せんデータ
    [System.SerializableAttribute]
    public class Lot
    {
        public CondAppGroup condAppGroup;
        public List<int> weight = new List<int>();
    }

    [SerializeField]
    public List<Lot> lotteryData = new List<Lot>();
    public CondAppDataBase effectiveCondApp;
    public CondApp FlagBonus;
    public CondApp ActiveCAD;
    public CondApp ActiveBonus;
    //public RTid rtid;
    
    public int betlevel;
}
