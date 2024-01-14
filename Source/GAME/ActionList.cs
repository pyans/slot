using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDataBase", menuName = "MyScriptable/DataBase/CreateActionDataBase")]
public class ActionList : ScriptableObject
{
	[SerializeField]
	private List<MyAction> actionList = new List<MyAction>();

	//’â~§ŒäƒŠƒXƒg‚ğ•Ô‚·
	public List<MyAction> GetActionLists()
	{
		return actionList;
	}
}
