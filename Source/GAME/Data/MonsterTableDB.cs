using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/DataBase/Create MonsterTableDB")]
public class MonsterTableDB : ScriptableObject
{
    //å≥ÉfÅ[É^
    public List<MonsterTable> monsterTables_master = new List<MonsterTable>();

    public List<MonsterTable> GetMonsterTable()
    {
        return monsterTables_master;
    }
}
