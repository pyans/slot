using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolDataBase", menuName = "MyScriptable/CreateSymbolDataBase")]
public class SymbolsDataBase : ScriptableObject
{

	[SerializeField]
	private List<SymbolsData> symbolLists = new List<SymbolsData>();

	//�@�}�����X�g��Ԃ�
	public List<SymbolsData> GetSymblosLists()
	{
		return symbolLists;
	}
}
