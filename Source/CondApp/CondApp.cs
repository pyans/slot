using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CondApp")]
public class CondApp : ScriptableObject
{
    //�������u�f�[�^
    [System.SerializableAttribute]
    public class SymPatternList
    {
        public List<SymbolsData> List = new List<SymbolsData>();

        public SymPatternList(List<SymbolsData> list)
        {
            List = list;
        }
    }

    //�������u�^�C�v
    public enum CondAppType
    {
        BONUS,
        JAC,
        REPLAY,
        WIN,
        LOSE
    }

    //�}���p�^�[���̃��X�g
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
