using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CondAppDataBase", menuName = "MyScriptable/Game/CreateStopDataBase")]
public class StopDataBase : ScriptableObject
{

	[SerializeField]
	private List<StopBehavior> stopList = new List<StopBehavior>();

	//’â~§ŒäƒŠƒXƒg‚ğ•Ô‚·
	public List<StopBehavior> GetCondAppLists()
	{
		return stopList;
	}
}
