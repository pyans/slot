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
        //�o���l���Z
        EXP += exp;
        //���x���A�b�v����
        while(lvupdata.need_EXP[Lv - 1] < EXP)
        {
            //���x���A�b�v
            Lv++;
            //�X�e�f�[�^���X�V
            this.GetStatus().attack += 1;
            this.GetStatus().hp += 4 + (Lv % 3);
            //���X�e�[�^�X�ɔ��f
            hp += 4 + (Lv % 3);
            attack = this.GetStatus().attack;
        }
    }

    public int GetLevel()
    {
        return Lv;
    }
}
