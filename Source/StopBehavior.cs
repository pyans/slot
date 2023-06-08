using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StopBehavior")]
public class StopBehavior : ScriptableObject
{
    //��~����f�[�^

    public int id;      //��~����ID
    public string comment;  //�R�����g

    [SerializeField]
    public List<int> slide = new List<int>();
}
