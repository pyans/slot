using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create Symbol")]
//project上でSymbolDataを新規作成できるようにする
public class SymbolsData : ScriptableObject
{
    public string symbolname;//シンボル
    public int id;//ID
}
