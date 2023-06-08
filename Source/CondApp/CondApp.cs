using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CondApp")]
public class CondApp : ScriptableObject
{
    //条件装置データ
    [System.SerializableAttribute]
    public class SymPatternList
    {
        public List<SymbolsData> List = new List<SymbolsData>();

        public SymPatternList(List<SymbolsData> list)
        {
            List = list;
        }
    }

    //条件装置タイプ
    public enum CondAppType
    {
        BONUS,
        JAC,
        REPLAY,
        WIN,
        LOSE
    }

    //図柄パターンのリスト
    [SerializeField]
    public List<SymPatternList> symPattern = new List<SymPatternList>();
    public string condappName;
    public CondAppType condappType;
    public List<int> payoutList = new List<int>();
    public int maxJac;
    public int maxMinorGame;
    public int maxjacwin;
    public int maxjacgame;
}
