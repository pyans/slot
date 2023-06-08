using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StopBehavior")]
public class StopBehavior : ScriptableObject
{
    //â~§äf[^

    public int id;      //â~§äID
    public string comment;  //Rg

    [SerializeField]
    public List<int> slide = new List<int>();
}
