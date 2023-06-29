using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StopPosDecide
{
    public List<ReelScript> reels;      //���[�����

    int CulcReelNum(int order)
    {
        while(order/10 != 0)
        {
            order /= 10;
        }
        return order - 1;
    }

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
        //�ȉ��̏����Œ�~�����ݒ肵�Ă�����Q�Ƃ���
        //����~�̂݉������Ŕ���
        //  �E��Q��~����R��~��
        //�@�E�쓮�������u
        //  �E��~���[��
        
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
                //���ꐧ�䔻��
                if (condAppG.specialStops.Count == 0) break;
                if (reels is null || reels.Count < 3)
                {
                    Debug.Log("Error:No reel info in StopPosDecide.cs");
                    break;
                }
                //���퐧������𖞂���������
                foreach(SpecialStop spstop in condAppG.specialStops)
                {
                    bool breakflag = false;                                     //���[�v�E�o�t���O
                    if (!spstop.stoporder.Contains(stoporder)) continue;        //��~���y�ђ�~�񓷂��Ⴆ�΃X�L�b�v
                    foreach (int pos in spstop.pos_1streel)
                    {
                        if (reels[CulcReelNum(stoporder)].stoppos == pos)    //�X���C�h�ł������l�����Ē�~�\��ʒu�Ŕ���
                        {
                            //���퐧��������Ȃ��@�X�x���R�}�����㏑��
                            slide = spstop.stopBehavior.slide[reelpos];
                            breakflag = true;
                            break;
                        }
                    }
                    //�������v�Ȃ�ޏ�
                    if (breakflag) break;
                }
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
                //���ꐧ�䔻��
                if (condAppG.specialStops.Count == 0) break;
                if (reels is null || reels.Count < 3)
                {
                    Debug.Log("Error:No reel info in StopPosDecide.cs");
                    break;
                }
                //���퐧������𖞂���������
                foreach (SpecialStop spstop in condAppG.specialStops)
                {
                    bool breakflag = false;                                     //���[�v�E�o�t���O
                    if (!spstop.stoporder.Contains(stoporder)) continue;        //��~���y�ђ�~�񓷂��Ⴆ�΃X�L�b�v
                    foreach (int pos1 in spstop.pos_1streel)
                    {
                        if (spstop.pos_2ndreel.Count == 0)
                        {
                            //����~�݂̂������̏ꍇ
                            if (reels[CulcReelNum(stoporder)].stoppos == pos1)    //�X���C�h�ł������l�����Ē�~�\��ʒu�Ŕ���
                            {
                                //���퐧��������Ȃ��@�X�x���R�}�����㏑��
                                slide = spstop.stopBehavior.slide[reelpos];
                                breakflag = true;
                                break;
                            }
                        }
                        else
                        {
                            //�����������̏ꍇ
                            foreach (int pos2 in spstop.pos_2ndreel)
                            {
                                if (reels[CulcReelNum(stoporder)].stoppos == pos1 &&
                                    reels[CulcReelNum(stoporder % 100)].stoppos == pos2)    //�X���C�h�ł������l�����Ē�~�\��ʒu�Ŕ���
                                {
                                    //���퐧��������Ȃ��@�X�x���R�}�����㏑��
                                    slide = spstop.stopBehavior.slide[reelpos];
                                    breakflag = true;
                                    break;
                                }
                            }
                        }
                    }

                    //�������v�Ȃ�ޏ�
                    if (breakflag) break;
                }
                break;
            default:
                break;

        }
        return slide;
    }
}
