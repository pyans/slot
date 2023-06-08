using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery
{
    //���I���s���N���X
    public int randmax = 65536;     //�����̍ő�l
    public LotteryData Data; //���I�f�[�^
    public CondAppGroup lose; //�O��f�[�^
    public CondAppGroup MainLottery(int setting)
    {
        //�����𐶐�
        int randnum = Random.Range(0, randmax);
        
        foreach (LotteryData.Lot lot in Data.lotteryData)
        {
            //�������璊�I�f�[�^�̏d�݂����Z
            randnum -= lot.weight[setting];

            //�����ɂȂ��Ă���ΒE�o
            if(randnum < 0)
            {
                return lot.condAppGroup;
            }
        }

        //�ǂ�����I���Ȃ���ΊO��
        return lose;
    }
}
