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
    public void SPActSelect(Status st, CondAppGroup hitminor, int specialid)
    {
        //メソッドを取得
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + specialid);
        //Invokeでメソッド呼び出し
        mi.Invoke(this, new object[] { st, hitminor });
    }

    //攻撃力算出
    int CulcDamage(int atk)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        return (int)(atk * (1.0f + rate));
    }
    public void SPAct_0(Status st, CondAppGroup hitminor)
    {
        if (st.isenemy)
        {
            //敵の攻撃
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
            //味方の攻撃
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
        //強化攻撃(主人公用)
        //ステータス強化用
        Status powup = st.StatusCopy();
        
        switch (hitminor.name)
        {
            case "BELL":
                //2回攻撃
                for (int i = 0; i < 2; i++) SPAct_0(st, hitminor);
                break;
            case "CHERRY_WEAK":
                //単体強攻撃
                powup.attack *= 8;
                SPAct_0(powup, hitminor);
                break;
            case "WTML_WEAK":
                //全体攻撃
                powup.attack *= 4;
                if (st.isenemy)
                {
                    //敵の攻撃
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
                    //味方の攻撃
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
                //ただの通常攻撃
                SPAct_0(st, hitminor);
                break;
        }
    }
}
