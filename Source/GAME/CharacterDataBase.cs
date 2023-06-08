using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataBase", menuName = "MyScriptable/CreateCharacterDataBase")]
public class CharacterDataBase : ScriptableObject
{
	[SerializeField]
	private List<Status> statusList = new List<Status>();

	//��~���䃊�X�g��Ԃ�
	public List<Status> GetStatusLists()
	{
		return statusList;
	}
}
