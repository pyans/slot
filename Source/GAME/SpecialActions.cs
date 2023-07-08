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

    public SpecialActions(List<Character> cl, List<Character> tl, List<Character> el)
    {
        //�C���X�^���X�������A���X�g���R�s�[
        CharacterList = cl;
        TeamList = tl;
        EnemyList = el;
    }
    public void SPActSelect(Character chr, CondAppGroup hitminor, int specialid)
    {
        //���\�b�h���擾
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + specialid);
        //Invoke�Ń��\�b�h�Ăяo��
        mi.Invoke(this, new object[] { chr, hitminor });
    }

    //�U���͎Z�o
    int CulcDamage(int atk)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        return (int)(atk * (1.0f + rate));
    }
    public void SPAct_0(Character chr, CondAppGroup hitminor)
    {
        if (chr.isenemy)
        {
            //�G�̍U��
            foreach (Character temp in TeamList)
            {
                if (temp != null && !temp.isdeath())
                {
                    temp.Damage(CulcDamage(chr.attack));
                    break;
                }
            }
        }
        else
        {
            //�����̍U��
            foreach (Character temp in EnemyList)
            {
                if (temp != null && !temp.isdeath())
                {
                    temp.Damage(CulcDamage(chr.attack));
                    break;

                }
            }
        }
    }

    public void SPAct_1(Character chr, CondAppGroup hitminor)
    {
        //�����U��(��l���p)
        //����ꎞ�I�ɍU���͂��グ��`�Ŏ���
        int tmp = chr.attack;

        switch (hitminor.name)
        {
            case "BELL":
                //2��U��
                for (int i = 0; i < 2; i++) SPAct_0(chr, hitminor);
                break;
            case "CHERRY_WEAK":
                //�P�̋��U��
                chr.attack *= 8;
                SPAct_0(chr, hitminor);
                chr.attack = tmp;
                break;
            case "WTML_WEAK":
                //�S�̍U��
                chr.attack *= 4;
                if (chr.isenemy)
                {
                    //�G�̍U��
                    foreach (Character temp in TeamList)
                    {
                        if (temp != null && !temp.isdeath())
                        {
                            temp.Damage(CulcDamage(chr.attack));
                        }
                    }
                }
                else
                {
                    //�����̍U��
                    foreach (Character temp in EnemyList)
                    {
                        if (temp != null && !temp.isdeath())
                        {
                            temp.Damage(CulcDamage(chr.attack));
                        }
                    }
                }
                chr.attack = tmp;
                break;
            default:
                //�����̒ʏ�U��
                SPAct_0(chr, hitminor);
                break;
        }
        //�U���͂����ɖ߂�
        chr.attack = tmp;
    }
}
