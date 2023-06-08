using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //�L�����N�^�[�̃��X�g
    List<Character> CharacterList = new List<Character>();
    //�G�y�і����̃��X�g
    List<Character> TeamList = new List<Character>();
    List<Character> EnemyList = new List<Character>();
    //�s���̃��X�g
    List<Character> ActorList = new List<Character>();

    //�v���C���[���i�Q�[���I�[�o�[����ɗp����j
    public Character player;

    //�L�����N�^�[�̃X�e�[�^�X���X�g
    public CharacterDataBase characterDataBase;
    List<Status> statusList;
    //�s�����X�g
    public SpecialActions spact;

    //���I�������u
    CondAppGroup condappGroup;

    //�L�����o��ʒu�f�[�^
    List<Vector3> posdata = new List<Vector3>()
    {
        new Vector3(3.0f,2.0f,-0.4f),
        new Vector3(2.0f,0.0f,-0.5f),
        new Vector3(1.0f,-2.0f,-0.6f),
        new Vector3(6.0f,2.0f,-0.4f),
        new Vector3(5.0f,0.0f,-0.5f),
        new Vector3(4.0f,-2.0f,-0.6f),
        new Vector3(9.0f,2.0f,-0.4f),
        new Vector3(8.0f,0.0f,-0.5f),
        new Vector3(7.0f,-2.0f,-0.6f),
        new Vector3(-3.0f,2.0f,-0.4f),
        new Vector3(-2.0f,0.0f,-0.5f),
        new Vector3(-1.0f,-2.0f,-0.6f),
        new Vector3(-6.0f,2.0f,-0.4f),
        new Vector3(-5.0f,0.0f,-0.5f),
        new Vector3(-4.0f,-2.0f,-0.6f),
        new Vector3(-9.0f,2.0f,-0.4f),
        new Vector3(-8.0f,0.0f,-0.5f),
        new Vector3(-7.0f,-2.0f,-0.6f),
    };

    //�s���ҍX�V�t���O
    bool renewactor;

    private int posdata_offset = 9;

    void AddCharacter(Status status, int posindx)
    {
        //�L�����I�u�W�F�N�g�쐬
        GameObject prefab = Resources.Load<GameObject>("GAME/newChara");
        GameObject newChara = Instantiate(prefab, this.gameObject.transform);
        Character newchara = newChara.GetComponent<Character>();
        //�X�e�[�^�X������
        newchara.SetStatus(status);

        //�L�������X�g�ɒ��荞��
        CharacterList.Add(newchara);
        
        if (status.isenemy)
        {
            //�G�̏ꍇ
            EnemyList.Add(newchara);
        }
        else
        {
            //�����̏ꍇ
            TeamList.Add(newchara);
        }

        newChara.transform.Translate(posdata[posindx]);
    }

    // Start is called before the first frame update
    void Start()
    {
        statusList = characterDataBase.GetStatusLists();
        //�L�����N�^�[�͎�l���������X�^�[�̏��ɐ�ɓ���
        AddCharacter(statusList[0], 1 + posdata_offset);
        AddCharacter(statusList[1], 1);
        AddCharacter(statusList[2], 2);
        AddCharacter(statusList[3], 0);
        AddCharacter(statusList[4], 4);
        AddCharacter(statusList[1], 7);

        //��l���̏���ۑ�
        player = CharacterList[0];

        //����s���f�[�^�x�[�X���쐬
        spact = new SpecialActions(CharacterList, TeamList, EnemyList);
    }

    public void UpdateGame(CondAppGroup hitminor)
    {
        //�^�[���I��������
        //�������u���X�V
        condappGroup = hitminor;

        //�s���ҋ@�J�E���^�X�V
        foreach (Character temp in CharacterList)
        {
            //����ł�����X�L�b�v
            if (temp.isdeath()) continue;
            //�s���҂�����-1
            temp.DecCount();
            if(temp.waitcount <= 0)
            {
                //�s�����X�g�ɒǉ�
                ActorList.Add(temp);
                //�J�E���g���Z�b�g
                temp.ResetCount();
            }
        }

        //�s������L������������X�V
        if(ActorList.Count != 0)
        {
            renewactor = true;
        }
    }

    void DeathCheck()
    {
        //���ׂẴ��X�g���玀�҂��폜
        TeamList.RemoveAll(s => s.isdeath());
        EnemyList.RemoveAll(s => s.isdeath());
        ActorList.RemoveAll(s => s.isdeath());

        //Characterlist���玀�񂾃L�������폜
        foreach(Character temp in CharacterList)
        {
            if (temp.isdeath())
            {
                //�Q�[���I�u�W�F�N�g���f�B���C���č폜
                Destroy(temp.gameObject, 0.5f);
            }
        }
        //�f�B���C���ɃL�������X�g����폜
        CharacterList.RemoveAll(s => s.isdeath());
        //�v���C���[���S
        if (player == null || player.isdeath())
        {
            //gameover����
            Debug.Log("You are dead.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (renewactor)
        {
            // �s��
            if (ActorList[0].Action())
            {
                //���Z�g�p
                spact.SPActSelect(ActorList[0].GetStatus(), condappGroup, ActorList[0].GetStatus().skill.id);
            }
            else
            {
                //�ʏ�U��
                spact.SPAct_0(ActorList[0].GetStatus(), condappGroup);
            }
            // �s���������͍̂폜
            ActorList.RemoveAt(0);
            //���S�҂��폜
            DeathCheck();

            // �s���ҍX�V
            renewactor = false;
        }
        else
        {
            //�s���҂����Ȃ��Ȃ�����
            if (ActorList.Count == 0)
            {
                //���̃^�[����
                ActorList.Clear();
                return;
            }
            renewactor = true;
        }


    }
}
