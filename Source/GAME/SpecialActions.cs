using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpecialActions
{
    //�L�����N�^�[�̃��X�g
    List<Character> CharacterList;
    //�G�y�і����̃��X�g
    List<Character> TeamList;
    List<Character> EnemyList;
    //�e�I�u�W�F�N�g
    GameMaster context;

    public SpecialActions(GameMaster context_p)
    {
        //�C���X�^���X�������A���X�g���擾
        CharacterList = context_p.GetCharacterList();
        TeamList = context_p.GetTeamList();
        EnemyList = context_p.GetEnemyList();
        //�e�I�u�W�F�N�g�̎擾
        context = context_p;
    }
    public void SPActSelect(Character chr, Character target, MyAction special)
    {
        //���\�b�h���擾
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + special.id);
        //Invoke�Ń��\�b�h�Ăяo��
        mi.Invoke(this, new object[] { chr, target });
    }

    //�U���͎Z�o
    int CulcDamage(int atk, int def)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        int tempdamage = Mathf.Max((int)(atk * (1.0f + rate) - (def / 2)), 0);
        if (tempdamage == 0)
        {
            //0�_�����̓_���ϊ�
            tempdamage = Random.Range(0, 100) % 2 == 0 ? 1 : 0;
        }
        return tempdamage;
    }

    Character AutoTarget(Character chr, Character pretarget)
    {
        if (false/*�����Ȃ�*/)
        {

        }
        else if (false/*������ԂȂ�*/)
        {

        }
        else if (pretarget != null && !pretarget.isdeath())
        {
            //���̃^�[�Q�b�g���U��
            return pretarget;
        }
        else
        {
            //�I�[�g�^�[�Q�b�g
            if (chr.isenemy)
            {
                foreach(Character temp in TeamList){
                    if(temp != null && !temp.isdeath())
                    {
                        return temp;
                    }
                }
            }
            else
            {
                foreach (Character temp in EnemyList)
                {
                    if (temp != null && !temp.isdeath())
                    {
                        return temp;
                    }
                }
            }
        }

        return null;
    }
    public void SPAct_0(Character chr, Character target)
    {
        //�ʏ�U��
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack, tgt.defence));
        
    }

    public void SPAct_1(Character chr, Character target)
    {
        //�����U��(��l����`�F�p)
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack * 4, 0));
    }

    public void SPAct_2(Character chr, Character target)
    {
        //�����U��(��l���X�C�J�p)
        //�S�̍U��
        foreach (Character tmp in EnemyList)
        {
            if (tmp != null && !tmp.isdeath())
            {
                tmp.Damage(CulcDamage(chr.attack * 2, tmp.defence));
            }
        }
    }

    public void SPAct_3(Character chr, Character target)
    {
        //�����U��(��l�������A�p)
        foreach (Character tmp in EnemyList)
        {
            if (tmp != null && !tmp.isdeath())
            {
                tmp.Damage(CulcDamage(chr.attack * 4, 0));
            }
        }
    }

    public void SPAct_4(Character chr, Character target)
    {
        //��(��l���p)
        chr.heal(chr.GetStatus().hp / 4);

        //�ʏ�U��
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack, 0));
    }

    public void SPAct_5(Character chr, Character target)
    {
        //����
        context.RemoveCharacter(chr);
    }

    public void SPAct_16(Character chr, Character target)
    {
        //��Ԉُ�(��)
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.SetSPStatus(Status.BADSTATUS.POISON);
    }


    public void SPAct_64(Character chr, Character target)
    {
        //���m
        //������
        if (context.condappGroup.isBONUS())
        {
            Character tgt = AutoTarget(chr, target);
            if (tgt == null) return;
            tgt.koteiDamage(777);
        }
    }

    public void SPAct_255(Character chr, Character target)
    {
        //�G�S�ōU��(��l���p)
        //�����̍U��
        foreach (Character tmp in EnemyList)
        {
            if (tmp != null && !tmp.isdeath())
            {
                tmp.koteiDamage(9999);
            }
        }
    }

}

