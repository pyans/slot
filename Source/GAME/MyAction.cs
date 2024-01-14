using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/Create Skills")]
public class MyAction : ScriptableObject
{
    public string actionName;
    public int id;
    public int targettype;
    // 0:敵から単体、1:味方から単体、2:敵全体、3:味方全体、4:敵味方全体
}
