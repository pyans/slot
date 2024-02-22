using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubLotDataBase", menuName = "MyScriptable/DataBase/Create SubLotList")]
public class SubLotDataBase : ScriptableObject
{
    //汎用抽せんデータベース
    [SerializeField]
    private List<SubLot> sublotList = new List<SubLot>();

    public List<SubLot> GetSubLotList()
    {
        return sublotList;
    }
}
