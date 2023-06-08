using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CondAppDataBase", menuName = "MyScriptable/CreateCondAppDataBase")]
public class CondAppDataBase : ScriptableObject
{

	[SerializeField]
	private List<CondApp> condappList = new List<CondApp>();

	//　アイテムリストを返す
	public List<CondApp> GetCondAppLists()
	{
		return condappList;
	}
}
