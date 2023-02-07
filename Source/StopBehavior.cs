using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StopBehavior")]
public class StopBehavior : ScriptableObject
{
    //停止制御データ

    public int id;      //停止制御ID
    public string comment;  //コメント

    //引き込み図柄
    //条件装置データ
    //[System.SerializableAttribute]
    //public class PrioritySymbol
    //{
    //    public List<SymbolsData>List  = new List<SymbolsData>();

    //    public PrioritySymbol(List<SymbolsData> list)
    //    {
    //        List = list;
    //    }
    //}
    public List<SymbolsData> PrioritySymbol = new List<SymbolsData>();
    /*Effective Line ID:
         * 
         * 0:TOP 
         * 1:RIGHTDOWN
         * 2:CENTER
         * 3:RIGHTUP
         * 4:BOTTOM
         * 
         */

    //優先引き込みライン
    public List<int> bringline_123 = new List<int>();
    public List<int> bringline_132 = new List<int>();
    public List<int> bringline_213 = new List<int>();
    public List<int> bringline_231 = new List<int>();
    public List<int> bringline_312 = new List<int>();
    public List<int> bringline_321 = new List<int>();
}
