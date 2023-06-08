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
    public void SPActSelect(Status st, CondAppGroup hitminor, int specialid)
    {
        //���\�b�h���擾
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + specialid);
        //Invoke�Ń��\�b�h�Ăяo��
        mi.Invoke(this, new object[] { st, hitminor });
    }

    //�U���͎Z�o
    int CulcDamage(int atk)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        return (int)(atk * (1.0f + rate));
    }
    public void SPAct_0(Status st, CondAppGroup hitminor)
    {
        if (st.isenemy)
        {
            //�G�̍U��
            foreach (Character temp in TeamList)
            {
                if (temp != null && !temp.isdeath())
                {
                    temp.Damage(CulcDamage(st.attack));
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
                    temp.Damage(CulcDamage(st.attack));
                    break;

                }
            }
        }
    }

    public void SPAct_1(Status st, CondAppGroup hitminor)
    {
        //�����U��(��l���p)
        //�X�e�[�^�X�����p
        Status powup = st.StatusCopy();
        
        switch (hitminor.name)
        {
            case "BELL":
                //2��U��
                for (int i = 0; i < 2; i++) SPAct_0(st, hitminor);
                break;
            case "CHERRY_WEAK":
                //�P�̋��U��
                powup.attack *= 8;
                SPAct_0(powup, hitminor);
                break;
            case "WTML_WEAK":
                //�S�̍U��
                powup.attack *= 4;
                if (st.isenemy)
                {
                    //�G�̍U��
                    foreach (Character temp in TeamList)
                    {
                        if (temp != null && !temp.isdeath())
                        {
                            temp.Damage(CulcDamage(powup.attack));
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
                            temp.Damage(CulcDamage(powup.attack));
                        }
                    }
                }
                break;
            default:
                //�����̒ʏ�U��
                SPAct_0(st, hitminor);
                break;
        }
    }
}
