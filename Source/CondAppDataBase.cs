using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CondAppDataBase", menuName = "MyScriptable/CreateCondAppDataBase")]
public class CondAppDataBase : ScriptableObject
{

	[SerializeField]
	private List<CondApp> condappList = new List<CondApp>();

	//�@�A�C�e�����X�g��Ԃ�
	public List<CondApp> GetCondAppLists()
	{
		return condappList;
	}
}
