using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int Lv = 1;
    private int EXP = 0;
    public lvupdata lvupdata;

    public void EXPPlus(int exp)
    {
        //経験値加算
        EXP += exp;
        //レベルアップ判定
        while(lvupdata.need_EXP[Lv - 1] < EXP)
        {
            //レベルアップ
            Lv++;
            //ステデータを更新
            this.GetStatus().attack += 1;
            this.GetStatus().hp += 4 + (Lv % 3);
            //実ステータスに反映
            hp += 4 + (Lv % 3);
            attack = this.GetStatus().attack;
        }
    }

    public int GetLevel()
    {
        return Lv;
    }
}
