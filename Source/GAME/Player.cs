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

    public override MyAction DecideAction(CondAppGroup hitminor, List<MyAction> actionList)
    {
        //�s���֐��i�v���C���[�j
        int actid = 0;
        if (hitminor.isRAREtype())
        {
            if (hitminor.isUR())
            {

            }
            else if (hitminor.isSR())
            {
                actid = 3;
            }
            else if (hitminor.isWTML())
            {
                actid = 2;
            }
            else
            {
                actid = 1;
            }
        }
        else if (hitminor.isREP())
        {
            actid = 4;
        }

        //�s��id�ƈ�v����s��������
        MyAction value = actionList.Find(n => n.id == actid);

        return value;
        
    }

    public override Character DecideTarget(MyAction action, GameMaster context)
    {
        //�^�Q�I���i�v���C���[�j
        Character target = null;
        List<Character> member;

        if (isenemy) member = context.GetTeamList();
        else member = context.GetEnemyList();
        if (member.Count == 0) return null;

        return target;
    }

    public override bool Action(GameMaster context)
    {
        //�s�����s
        MyAction act = DecideAction(context.condappGroup, context.actionList.GetActionLists());
        Character targets = DecideTarget(act, context);

        context.spact.SPActSelect(this, targets, act);

        return true;
    }

    public int GetLevel()
    {
        return Lv;
    }
}
