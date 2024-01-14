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
    //親オブジェクト
    GameMaster context;

    public SpecialActions(GameMaster context_p)
    {
        //インスタンス生成時、リストを取得
        CharacterList = context_p.GetCharacterList();
        TeamList = context_p.GetTeamList();
        EnemyList = context_p.GetEnemyList();
        //親オブジェクトの取得
        context = context_p;
    }
    public void SPActSelect(Character chr, Character target, MyAction special)
    {
        //メソッドを取得
        MethodInfo mi = (this.GetType()).GetMethod("SPAct_" + special.id);
        //Invokeでメソッド呼び出し
        mi.Invoke(this, new object[] { chr, target });
    }

    //攻撃力算出
    int CulcDamage(int atk)
    {
        float rate = Random.Range(-0.25f, 0.25f);
        return (int)(atk * (1.0f + rate));
    }

    Character AutoTarget(Character chr, Character pretarget)
    {
        if (false/*混乱なら*/)
        {

        }
        else if (false/*挑発状態なら*/)
        {

        }
        else if (pretarget != null && !pretarget.isdeath())
        {
            //元のターゲットを攻撃
            return pretarget;
        }
        else
        {
            //オートターゲット
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
        //通常攻撃
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack*2));
        
    }

    public void SPAct_1(Character chr, Character target)
    {
        //強化攻撃(主人公用)
        //通常攻撃
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack*8));
    }

    public void SPAct_2(Character chr, Character target)
    {
        //強化攻撃(主人公用)

        //通常攻撃
        foreach (Character tmp in EnemyList)
        {
            if (tmp != null && !tmp.isdeath())
            {
                tmp.Damage(CulcDamage(chr.attack * 4));
            }
        }
    }

    public void SPAct_3(Character chr, Character target)
    {
        //強化攻撃(主人公強レア用)
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack * 16));
    }

    public void SPAct_4(Character chr, Character target)
    {
        //回復(主人公用)
        chr.heal(chr.GetStatus().hp / 4);

        //通常攻撃
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.Damage(CulcDamage(chr.attack));
    }

    public void SPAct_16(Character chr, Character target)
    {
        //状態異常(毒)
        Character tgt = AutoTarget(chr, target);
        if (tgt == null) return;
        tgt.SetSPStatus(Status.BADSTATUS.POISON);
    }


    public void SPAct_64(Character chr, Character target)
    {
        //告知
        //未実装
        if (context.condappGroup.isBONUS())
        {
            Character tgt = AutoTarget(chr, target);
            if (tgt == null) return;
            tgt.koteiDamage(777);
        }
    }

    public void SPAct_255(Character chr, Character target)
    {
        //敵全滅攻撃(主人公用)
        //味方の攻撃
        foreach (Character tmp in EnemyList)
        {
            if (tmp != null && !tmp.isdeath())
            {
                tmp.koteiDamage(9999);
            }
        }
    }

}

