using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpecialActions
{
    //LN^[ΜXg
    List<Character> CharacterList;
    //GyΡ‘ϋΜXg
    List<Character> TeamList;
    List<Character> EnemyList;

    public SpecialActions(List<Character> cl, List<Character> tl, List<Character> el)
    {
        //CX^XΆ¬AXgπRs[
        CharacterList = cl;
        TeamList = tl;
        EnemyList = el;
    }
    public void SPActSelect(Character chr, CondAppGroup hitminor, int specialid)
    {
        //\bhπζΎ
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + specialid);
        //InvokeΕ\bhΔΡo΅
        mi.Invoke(this, new object[] { chr, hitminor });
    }

    //UΝZo
    int CulcDamage(int atk)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        return (int)(atk * (1.0f + rate));
    }
    public void SPAct_0(Character chr, CondAppGroup hitminor)
    {
        if (chr.isenemy)
        {
            //GΜU
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
            //‘ϋΜU
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
        //­»U(εlφp)
        //»σκIΙUΝπγ°ι`Εΐ
        int tmp = chr.attack;

        switch (hitminor.name)
        {
            case "BELL":
                //2ρU
                for (int i = 0; i < 2; i++) SPAct_0(chr, hitminor);
                break;
            case "CHERRY_WEAK":
                //PΜ­U
                chr.attack *= 8;
                SPAct_0(chr, hitminor);
                chr.attack = tmp;
                break;
            case "WTML_WEAK":
                //SΜU
                chr.attack *= 4;
                if (chr.isenemy)
                {
                    //GΜU
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
                    //‘ϋΜU
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
                //½ΎΜΚνU
                SPAct_0(chr, hitminor);
                break;
        }
        //UΝπ³Ιί·
        chr.attack = tmp;
    }
}
