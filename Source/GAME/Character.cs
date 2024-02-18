using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected Status status;
    //�퓬�p���݃X�e�[�^�X
    public int hp;
    public int attack;
    public int defence;
    public int waitcount;
    public int randmax = 256;
    public int posindex;

    //�G�����t���O
    public bool isenemy;
    //�����A��Ԉُ�
    public int statusFlug;

    public void SetStatus(Status setstatus)
    {
        //���̂܂܂��ƃX�e�[�^�X�̌��f�[�^���R�s�[����̂�
        //�R�s�[�����C���X�^���X���쐬
        status = setstatus.StatusCopy();
        SetSprite();
        waitcount = status.firstwait;

        //������
        hp = setstatus.hp;
        attack = setstatus.attack;
        defence = setstatus.defence;
    }

    public Status GetStatus()
    {
        //�O����X�e�[�^�X���Q��
        return status;
    }

    public void SetSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = status.sprite;
    }

    public virtual MyAction DecideAction(CondAppGroup hitminor, List<MyAction> actionList)
    {
        //�s���֐�
        //���Z�g�p���ɉ����ē��Z���g�p
        if (Random.Range(0, randmax) < status.skillLot)
        {
            Debug.Log(status.name + " use skill!");
            return actionList.Find(n => n.id == status.skill.id);
        }
        else
        {
            //�ʏ�U��
            return actionList[0];
        }
    }

    public virtual Character DecideTarget(MyAction act, GameMaster context)
    {
        //�^�Q�I���i�ėp�j
        Character target = null;
        List<Character> member;

        if (isenemy) member = context.GetTeamList();
        else member = context.GetEnemyList();

        //if(member.Count != 0)target = member[0];

        return target;
    }

    public virtual bool Action(GameMaster context)
    {
        //�����X�^�[�̍s�����s
        MyAction act = DecideAction(context.condappGroup, context.actionList.GetActionLists());
        Character target = DecideTarget(act, context);

        context.spact.SPActSelect(this, target, act);

        return true;
    }

    public void Damage(int damage)
    {
        
        //�_���[�W���󂯂�
        hp = Mathf.Max(0, hp - damage);

        //�_���[�W�\��
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().MyStart(damage);

        //HP0�Ȃ玀�S
        if(hp <= 0)
        {
            statusFlug = statusFlug | (int)Status.BADSTATUS.DEATH;
        }
    }

    public void SetSPStatus(Status.BADSTATUS SPst)
    {
        //��Ԉُ�t�^
        this.statusFlug = statusFlug | (int)(SPst);
        //Add
        AddStDisp((int)SPst);
    }

    public void koteiDamage(int damage)
    {
        //�_���[�W���󂯂�
        hp = Mathf.Max(0, hp - damage);

        //�_���[�W�\��
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().MyStart(damage);

        //HP0�Ȃ玀�S
        if (hp <= 0)
        {
            statusFlug = statusFlug | (int)Status.BADSTATUS.DEATH;
        }
    }

    public bool isdeath()
    {
        //���S�m�F
        return (statusFlug & (int)Status.BADSTATUS.DEATH) != 0;
    }

    public void heal(int healHP)
    {
        //��
        if (isdeath()) return;  //���S���͉񕜕s�\
        hp = Mathf.Min(status.hp, hp + healHP);

        //�񕜗ʕ\��
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().HealStart(healHP);
    }

    //�s���ҋ@�J�E���g�̑���
    public void DecCount()
    {
        //�J�E���g���Z
        waitcount--;
    }

    public void ResetCount()
    {
        //�J�E���g���Z�b�g
        waitcount = status.firstwait;
    }

    public void SetCount(int newcount)
    {
        //�J�E���g�����l��
        waitcount = newcount;
    }

    public void AddCount(int addcount)
    {
        //�J�E���g����
        waitcount += addcount;
    }

    void AddStDisp(int stid)
    {
        //��Ԉُ�A�C�R���\��
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/Icon/STDisp");
        GameObject newobj = Instantiate(obj, this.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        //�ҋ@���Ԃ�\��

        //������
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
