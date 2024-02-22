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
            Status temp = this.GetStatus();
            temp.attack += 1;
            temp.hp += 4 + (Lv % 3);
            this.SetStatus(temp);
        }
    }

    public int GetLevel()
    {
        return Lv;
    }
}
