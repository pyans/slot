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

    public bool Action()
    {
        //�s���֐�
        //���Z�g�p���ɉ����ē��Z���g�p
        if (Random.Range(0, randmax) < status.skillLot)
        {
            Debug.Log(status.name + " use skill!");
            return true;
        }
        else
        {
            //�ʏ�U��
            return false;
        }
    }

    public void Damage(int damage)
    {
        //����͂ɂ�錸�Z
        int lastdamage = Mathf.Max(0, damage - defence);
        //�_���[�W���󂯂�
        hp = Mathf.Max(0, hp - lastdamage);

        //�_���[�W�\��
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/DamageEff");
        GameObject newobj = Instantiate(obj, this.transform);
        newobj.GetComponent<DamageEff>().MyStart(lastdamage);

        //HP0�Ȃ玀�S
        if(hp <= 0)
        {
            statusFlug = statusFlug | (int)Status.BADSTATUS.DEATH;
        }
    }

    public bool isdeath()
    {
        //���S�m�F
        return (statusFlug & (int)Status.BADSTATUS.DEATH) != 0;
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
