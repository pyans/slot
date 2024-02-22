using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected Status status;
    public int waitcount;
    public int randmax = 256;
    public int posindex;

    public void SetStatus(Status setstatus)
    {
        //そのままだとステータスの元データをコピーするので
        //コピーしたインスタンスを作成
        status = setstatus.StatusCopy();
        SetSprite();
        waitcount = status.firstwait;
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

    public bool Action()
    {
        //行動関数
        //特技使用率に応じて特技を使用
        if (Random.Range(0, randmax) < status.skillLot)
        {
            Debug.Log(status.name + " use skill!");
            return true;
        }
        else
        {
            //通常攻撃
            return false;
        }
    }

    public void Damage(int damage)
    {
        //守備力による減算
        int lastdamage = Mathf.Max(0, damage - status.diffence);
        //ダメージを受ける
        status.hp -= lastdamage;

        //ダメージ表示
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().MyStart(lastdamage);

        //HP0なら死亡
        if(status.hp <= 0)
        {
            status.statusFlug = status.statusFlug | (int)Status.BADSTATUS.DEATH;
        }
    }

    public bool isdeath()
    {
        //死亡確認
        return (status.statusFlug & (int)Status.BADSTATUS.DEATH) != 0;
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
