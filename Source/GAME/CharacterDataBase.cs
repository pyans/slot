using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase", menuName = "MyScriptable/CreateCharacterDataBase")]
public class CharacterDataBase : ScriptableObject
{
	[SerializeField]
	private List<Status> statusList = new List<Status>();

	//停止制御リストを返す
	public List<Status> GetStatusLists()
	{
		return statusList;
	}
}
