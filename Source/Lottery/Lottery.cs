using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery
{
    //抽選を行うクラス
    public int randmax = 65536;     //乱数の最大値
    public LotteryData Data; //抽選データ
    public CondAppGroup lose; //外れデータ
    public CondAppGroup MainLottery(int setting)
    {
        //乱数を生成
        int randnum = Random.Range(0, randmax);
        
        foreach (LotteryData.Lot lot in Data.lotteryData)
        {
            //乱数から抽選データの重みを減算
            randnum -= lot.weight[setting];

            //負数になっていれば脱出
            if(randnum < 0)
            {
                return lot.condAppGroup;
            }
        }

        //どれも当選しなければ外れ
        return lose;
    }
}
