using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CondAppDataBase", menuName = "MyScriptable/DataBase/CreateStopDataBase")]
public class StopDataBase : ScriptableObject
{

	[SerializeField]
	private List<StopBehavior> stopList = new List<StopBehavior>();

	//��~���䃊�X�g��Ԃ�
	public List<StopBehavior> GetCondAppLists()
	{
		return stopList;
	}
}
