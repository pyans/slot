using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CondAppDataBase", menuName = "MyScriptable/Game/CreateStopDataBase")]
public class StopDataBase : ScriptableObject
{

	[SerializeField]
	private List<StopBehavior> stopList = new List<StopBehavior>();

	//停止制御リストを返す
	public List<StopBehavior> GetCondAppLists()
	{
		return stopList;
	}
}
