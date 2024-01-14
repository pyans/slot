using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/Create MonsterTable")]
public class MonsterTable : ScriptableObject
{
    // Start is called before the first frame update
    public string comment;
    public List<Status> hit_monster = new List<Status>();
}
