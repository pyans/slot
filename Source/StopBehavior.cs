using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StopBehavior")]
public class StopBehavior : ScriptableObject
{
    //â~§äf[^

    public int id;      //â~§äID
    public string comment;  //Rg

    //ø«Ý}¿
    //ðuf[^
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

    //Dæø«ÝC
    public List<int> bringline_123 = new List<int>();
    public List<int> bringline_132 = new List<int>();
    public List<int> bringline_213 = new List<int>();
    public List<int> bringline_231 = new List<int>();
    public List<int> bringline_312 = new List<int>();
    public List<int> bringline_321 = new List<int>();
}
