using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery
{
    //���I���s���N���X
    int randmax_2B = 65536;     //�����̍ő�l
    int randmax_1B = 256;
    public LotteryData Data; //���I�f�[�^
    public CondAppGroup lose; //�O��f�[�^
    public List<SubLot> sublotlist; //�ėp������f�[�^
    public CondAppGroup MainLottery(int setting)
    {
        //�����𐶐�
        int randnum = Random.Range(0, randmax_2B);
        
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

    public int SubLottery_1B(int index, SubLot sublot)
    {
        //�ėp�����񏈗�
        //���ʕۑ��o�b�t�@
        int result = 0;
        //�I�[�o�[��������
        if(index >= sublot.sublotdatas.Count)
        {
            Debug.Log("Error:Sub Lot index is overrun.");
            return 0;
        }
        List<int> lotdata = sublot.sublotdatas[index].sublotdata;
        //������
        //�����𐶐�
        int randnum = Random.Range(0, randmax_1B);
        foreach (int lotvalue in lotdata)
        {
            //���ʂ��Z�b�g
            result++;
            //�������璊�I�f�[�^�̏d�݂����Z
            randnum -= lotvalue;

            //�����ɂȂ��Ă���ΒE�o
            if (randnum < 0)
            {
                return result;
            }
        }

        return 0;
    }

    //���o������
    public int Efflottery_Lightoff(CondAppGroup hit, int zengame)
    {
        //������I��
        int index = 0;
        int hittype = hit.typeofCondApp;

        if ((hittype & (int)CondAppGroup.CondAppGroupTag.UR) != 0)
        {
            return SubLottery_1B(2, sublotlist[1]); //�����������
        }
        //���̑���������
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

        //�O�����͒�����ύX
        if (zengame != 0) index += 4;
        //��������s
        return SubLottery_1B(index, sublotlist[0]);
    }

    //ART������
    public int CZlottery(CondAppGroup hit)
    {
        //�ʏ펞ART�ւ�CZ������
        //������I��
        int index = 0;
        int hittype = hit.typeofCondApp;

        if ((hittype & (int)(CondAppGroup.CondAppGroupTag.UR | CondAppGroup.CondAppGroupTag.BONUS)) != 0)
        {
            //�m�����ret
            return 0;
        }
        if ((hittype & (int)CondAppGroup.CondAppGroupTag.REP) != 0)
        {
            //�ėV�Z
            index = 1;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.RARE | CondAppGroup.CondAppGroupTag.CHERRY)) == 0)
        {
            //��`�F���[
            index = 2;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.RARE | CondAppGroup.CondAppGroupTag.WTML)) == 0)
        {
            //��X�C�J
            index = 3;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.SR | CondAppGroup.CondAppGroupTag.CHERRY)) == 0)
        {
            //���`�F���[
            index = 4;
        }
        if ((hittype ^ (int)(CondAppGroup.CondAppGroupTag.SR | CondAppGroup.CondAppGroupTag.BELL)) == 0)
        {
            //�`�����X�x��
            index = 5;
        }

        //��������s
        return SubLottery_1B(index, sublotlist[2]);
    }

    public (int,int) ZenChoulottery(int czkind, CondAppGroup hit)
    {
        //�O���Q�[����������
        int index = 1;
        if((hit.typeofCondApp & (int)CondAppGroup.CondAppGroupTag.BONUS) != 0)
        {
            //�{�i�{�O��
            return (SubLottery_1B(0, sublotlist[3])-1, 0);
        }
        else
        {
            //cz��ނɂ�蕪��
            switch (czkind)
            {
                case 1:
                    //CZ�O��
                    index = 2;
                    break;
                case 2:
                    //ART���mCZ�O��
                    index = 4;
                    break;
                default:
                    if((hit.typeofCondApp & (int)(CondAppGroup.CondAppGroupTag.RARE| CondAppGroup.CondAppGroupTag.SR)) != 0)
                    {
                        //���A���̃K�Z�O��
                        return (SubLottery_1B(1, sublotlist[3])-1, 0);
                    }
                    break;
            }
        }
        //CZ�̑O���Q�[������Ԃ�
        return (SubLottery_1B(index, sublotlist[3])-1, SubLottery_1B(index + 2, sublotlist[3]));
    }
}
