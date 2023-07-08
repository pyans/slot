using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery
{
    //抽選を行うクラス
    int randmax_2B = 65536;     //乱数の最大値
    int randmax_1B = 256;
    public LotteryData Data; //抽選データ
    public CondAppGroup lose; //外れデータ
    public List<SubLot> sublotlist; //汎用抽せんデータ
    public CondAppGroup MainLottery(int setting)
    {
        //乱数を生成
        int randnum = Random.Range(0, randmax_2B);
        
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

    public int SubLottery_1B(int index, SubLot sublot)
    {
        //汎用抽せん処理
        //結果保存バッファ
        int result = 0;
        //オーバーラン判定
        if(index >= sublot.sublotdatas.Count)
        {
            Debug.Log("Error:Sub Lot index is overrun.");
            return 0;
        }
        List<int> lotdata = sublot.sublotdatas[index].sublotdata;
        //抽せん
        //乱数を生成
        int randnum = Random.Range(0, randmax_1B);
        foreach (int lotvalue in lotdata)
        {
            //結果をセット
            result++;
            //乱数から抽選データの重みを減算
            randnum -= lotvalue;

            //負数になっていれば脱出
            if (randnum < 0)
            {
                return result;
            }
        }

        return 0;
    }

    //演出抽せん
    public int Efflottery_Lightoff(CondAppGroup hit, int zengame)
    {
        //抽せん選択
        int index = 0;
        int hittype = hit.typeofCondApp;

        if ((hittype & (int)CondAppGroup.CondAppGroupTag.UR) != 0)
        {
            return SubLottery_1B(2, sublotlist[1]); //特殊消灯処理
        }
        //その他消灯処理
        if ((hittype & (int)CondAppGroup.CondAppGroupTag.REP) != 0)
        {
            index = 1;
        }
        if ((hittype & (int)CondAppGroup.CondAppGroupTag.RARE) != 0)
        {
            index = 2;
        }
        if ((hittype & (int)CondAppGroup.CondAppGroupTag.SR) != 0)
        {
            index = 3;
        }

        //前兆中は抽せん変更
        if (zengame != 0) index += 4;
        //抽せん実行
        return SubLottery_1B(index, sublotlist[0]);
    }

    //ART抽せん
    public int CZlottery(CondAppGroup hit)
    {
        //通常時ARTへのCZ抽せん
        //抽せん選択
        int index = 0;
        int hittype = hit.typeofCondApp;

        if ((hittype & (int)(CondAppGroup.CondAppGroupTag.UR | CondAppGroup.CondAppGroupTag.BONUS)) != 0)
        {
            //確定役はret
            return 0;
        }
        if ((hittype & (int)CondAppGroup.CondAppGroupTag.REP) != 0)
        {
            //再遊技
            index = 1;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.RARE | CondAppGroup.CondAppGroupTag.CHERRY)) == 0)
        {
            //弱チェリー
            index = 2;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.RARE | CondAppGroup.CondAppGroupTag.WTML)) == 0)
        {
            //弱スイカ
            index = 3;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.SR | CondAppGroup.CondAppGroupTag.CHERRY)) == 0)
        {
            //強チェリー
            index = 4;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.SR | CondAppGroup.CondAppGroupTag.BELL)) == 0)
        {
            //チャンスベル
            index = 5;
        }

        //抽せん実行
        return SubLottery_1B(index, sublotlist[2]);
    }

    public (int,int) ZenChoulottery(int czkind, CondAppGroup hit)
    {
        //前兆ゲーム数抽せん
        int index = 1;
        if((hit.typeofCondApp & (int)CondAppGroup.CondAppGroupTag.BONUS) != 0)
        {
            //ボナ本前兆
            return (SubLottery_1B(0, sublotlist[3])-1, 0);
        }
        else
        {
            //cz種類により分岐
            switch (czkind)
            {
                case 1:
                    //CZ前兆
                    index = 2;
                    break;
                case 2:
                    //ART当確CZ前兆
                    index = 4;
                    break;
                default:
                    if((hit.typeofCondApp & (int)(CondAppGroup.CondAppGroupTag.RARE| CondAppGroup.CondAppGroupTag.SR)) != 0)
                    {
                        //レア役のガセ前兆
                        return (SubLottery_1B(1, sublotlist[3])-1, 0);
                    }
                    break;
            }
        }
        //CZの前兆ゲーム数を返す
        return (SubLottery_1B(index, sublotlist[3])-1, SubLottery_1B(index + 2, sublotlist[3]));
    }
}
