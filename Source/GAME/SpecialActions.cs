using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpecialActions
{
    //キャラクターのリスト
    List<Character> CharacterList;
    //敵及び味方のリスト
    List<Character> TeamList;
    List<Character> EnemyList;

    public SpecialActions(List<Character> cl, List<Character> tl, List<Character> el)
    {
        //インスタンス生成時、リストをコピー
        CharacterList = cl;
        TeamList = tl;
        EnemyList = el;
    }
    public void SPActSelect(Character chr, CondAppGroup hitminor, int specialid)
    {
        //メソッドを取得
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + specialid);
        //Invokeでメソッド呼び出し
        mi.Invoke(this, new object[] { chr, hitminor });
    }

    //攻撃力算出
    int CulcDamage(int atk)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        return (int)(atk * (1.0f + rate));
    }
    public void SPAct_0(Character chr, CondAppGroup hitminor)
    {
        if (chr.isenemy)
        {
            //敵の攻撃
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
            //味方の攻撃
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
        //強化攻撃(主人公用)
        //現状一時的に攻撃力を上げる形で実装
        int tmp = chr.attack;

        switch (hitminor.name)
        {
            case "BELL":
                //2回攻撃
                for (int i = 0; i < 2; i++) SPAct_0(chr, hitminor);
                break;
            case "CHERRY_WEAK":
                //単体強攻撃
                chr.attack *= 8;
                SPAct_0(chr, hitminor);
                chr.attack = tmp;
                break;
            case "WTML_WEAK":
                //全体攻撃
                chr.attack *= 4;
                if (chr.isenemy)
                {
                    //敵の攻撃
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
                    //味方の攻撃
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
                //ただの通常攻撃
                SPAct_0(chr, hitminor);
                break;
        }
        //攻撃力を元に戻す
        chr.attack = tmp;
    }
}
