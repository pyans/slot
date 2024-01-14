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
        //経験値加算
        EXP += exp;
        //レベルアップ判定
        while(lvupdata.need_EXP[Lv - 1] < EXP)
        {
            //レベルアップ
            Lv++;
            //ステデータを更新
            this.GetStatus().attack += 1;
            this.GetStatus().hp += 4 + (Lv % 3);
            //実ステータスに反映
            hp += 4 + (Lv % 3);
            attack = this.GetStatus().attack;
        }
    }

    public override MyAction DecideAction(CondAppGroup hitminor, List<MyAction> actionList)
    {
        //行動関数（プレイヤー）
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

        //行動idと一致する行動を検索
        MyAction value = actionList.Find(n => n.id == actid);

        return value;
        
    }

    public override Character DecideTarget(MyAction action, GameMaster context)
    {
        //タゲ選択（プレイヤー）
        Character target = null;
        List<Character> member;

        if (isenemy) member = context.GetTeamList();
        else member = context.GetEnemyList();
        if (member.Count == 0) return null;

        return target;
    }

    public override bool Action(GameMaster context)
    {
        //行動実行
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
