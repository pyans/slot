using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StopPosDecide
{

    public Dictionary<int, int> tableIdDict = new Dictionary<int, int>()
    {
        {12, 0},
        {13, 1},
        {21, 2},
        {23, 3},
        {31, 4},
        {32, 5},
        {123, 0},
        {132, 1},
        {213, 2},
        {231, 3},
        {312, 4},
        {321, 5},
    };

    public int decideStop(int stoporder, int reelpos, CondAppGroup condAppG)
    {
        //��~����
        int slide = 0;      //�X�x���R�}��
        int table_id;   //��~����e�[�u��

        //MEMO:
        //����̓��[������Ƀe�[�u�������p����
        //�쓮�����������u�̑g�ݍ��킹���Ƃɒ�~�����ݒ肵�Ă�����Q�Ƃ���
        
        switch (stoporder)
        {
            case 1:
            case 2:
            case 3:
                //����~
                //���[���̌��݈ʒu����X�x���e�[�u�����Q��
                slide = condAppG.stopBh_1st[stoporder-1].slide[reelpos];
                break;
            case 12:
            case 13:
            case 21:
            case 23:
            case 31:
            case 32:
                //����~
                //����~�ɂ���ĎQ�Ƃ��ς��
                table_id = tableIdDict[stoporder];
                slide = condAppG.stopBh_2nd[table_id].slide[reelpos];
                break;
            case 123:
            case 132:
            case 213:
            case 231:
            case 312:
            case 321:
                //��O��~
                //����~�Ƃ��قǕς��Ȃ�
                table_id = tableIdDict[stoporder];
                slide = condAppG.stopBh_3rd[table_id].slide[reelpos];
                break;
            default:
                break;

        }
        return slide;
    }
}
