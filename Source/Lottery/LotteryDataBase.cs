using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LotteryDataBase", menuName = "MyScriptable/CreateLotteryDataBase")]
public class LotteryDataBase : ScriptableObject
{

	[SerializeField]
	private List<LotteryData> lotteryList = new List<LotteryData>();

	//�����񃊃X�g��Ԃ�
	public List<LotteryData> GetLotteryLists()
	{
		return lotteryList;
	}
}
