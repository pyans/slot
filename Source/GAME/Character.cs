using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected Status status;
    //戦闘用現在ステータス
    public int hp;
    public int attack;
    public int defence;
    public int waitcount;
    public int randmax = 256;
    public int posindex;

    //敵味方フラグ
    public bool isenemy;
    //特性、状態異常
    public int statusFlug;

    public void SetStatus(Status setstatus)
    {
        //そのままだとステータスの元データをコピーするので
        //コピーしたインスタンスを作成
        status = setstatus.StatusCopy();
        SetSprite();
        waitcount = status.firstwait;

        //初期化
        hp = setstatus.hp;
        attack = setstatus.attack;
        defence = setstatus.defence;
    }

    public Status GetStatus()
    {
        //外からステータスを参照
        return status;
    }

    public void SetSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = status.sprite;
    }

    public virtual MyAction DecideAction(CondAppGroup hitminor, List<MyAction> actionList)
    {
        //行動関数
        //特技使用率に応じて特技を使用
        if (Random.Range(0, randmax) < status.skillLot)
        {
            Debug.Log(status.name + " use skill!");
            return actionList.Find(n => n.id == status.skill.id);
        }
        else
        {
            //通常攻撃
            return actionList[0];
        }
    }

    public virtual Character DecideTarget(MyAction act, GameMaster context)
    {
        //タゲ選択（汎用）
        Character target = null;
        List<Character> member;

        if (isenemy) member = context.GetTeamList();
        else member = context.GetEnemyList();

        //if(member.Count != 0)target = member[0];

        return target;
    }

    public virtual bool Action(GameMaster context)
    {
        //モンスターの行動実行
        MyAction act = DecideAction(context.condappGroup, context.actionList.GetActionLists());
        Character target = DecideTarget(act, context);

        context.spact.SPActSelect(this, target, act);

        return true;
    }

    public void Damage(int damage)
    {
        
        //ダメージを受ける
        hp = Mathf.Max(0, hp - damage);

        //ダメージ表示
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().MyStart(damage);

        //HP0なら死亡
        if(hp <= 0)
        {
            statusFlug = statusFlug | (int)Status.BADSTATUS.DEATH;
        }
    }

    public void SetSPStatus(Status.BADSTATUS SPst)
    {
        //状態異常付与
        this.statusFlug = statusFlug | (int)(SPst);
        //Add
        AddStDisp((int)SPst);
    }

    public void koteiDamage(int damage)
    {
        //ダメージを受ける
        hp = Mathf.Max(0, hp - damage);

        //ダメージ表示
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().MyStart(damage);

        //HP0なら死亡
        if (hp <= 0)
        {
            statusFlug = statusFlug | (int)Status.BADSTATUS.DEATH;
        }
    }

    public bool isdeath()
    {
        //死亡確認
        return (statusFlug & (int)Status.BADSTATUS.DEATH) != 0;
    }

    public void heal(int healHP)
    {
        //回復
        if (isdeath()) return;  //死亡時は回復不可能
        hp = Mathf.Min(status.hp, hp + healHP);

        //回復量表示
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().HealStart(healHP);
    }

    //行動待機カウントの増減
    public void DecCount()
    {
        //カウント減算
        waitcount--;
    }

    public void ResetCount()
    {
        //カウントリセット
        waitcount = status.firstwait;
    }

    public void SetCount(int newcount)
    {
        //カウントを特定値に
        waitcount = newcount;
    }

    public void AddCount(int addcount)
    {
        //カウント増加
        waitcount += addcount;
    }

    void AddStDisp(int stid)
    {
        //状態異常アイコン表示
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/Icon/STDisp");
        GameObject newobj = Instantiate(obj, this.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        //待機時間を表示

        //未着手
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
