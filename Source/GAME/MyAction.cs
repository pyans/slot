using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/Create Skills")]
public class MyAction : ScriptableObject
{
    public string actionName;
    public int id;
    public int targettype;
    // 0:�G����P�́A1:��������P�́A2:�G�S�́A3:�����S�́A4:�G�����S��
}
