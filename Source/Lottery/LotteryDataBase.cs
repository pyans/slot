using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LotteryDataBase", menuName = "MyScriptable/CreateLotteryDataBase")]
public class LotteryDataBase : ScriptableObject
{

	[SerializeField]
	private List<LotteryData> lotteryList = new List<LotteryData>();

	//抽せんリストを返す
	public List<LotteryData> GetLotteryLists()
	{
		return lotteryList;
	}
}
