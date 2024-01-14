using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //�L�����N�^�[�̃��X�g
    List<Character> CharacterList = new List<Character>();

    public List<Character> GetCharacterList()
    {
        return CharacterList;
    }

    //�G�y�і����̃��X�g
    List<Character> TeamList = new List<Character>();
    List<Character> EnemyList = new List<Character>();

    List<Status> PrepareMonsters = new List<Status>();

    public List<Character> GetEnemyList()
    {
        return EnemyList;
    }

    public List<Character> GetTeamList()
    {
        return TeamList;
    }

    public void AddReserveMonster(Status addmonster)
    {
        //�o�������X�^�[�̗\��
        PrepareMonsters.Add(addmonster);
    }

    public void RemoveReserveMonster()
    {
        //�o�������X�^�[�̏o���ƃf�[�^�N���A
        foreach (Status item in PrepareMonsters){
            //��̂��O����z�u�y�z�u�ł��Ȃ����͏��Łz
            AddCharacter(item, 0);
        }

        PrepareMonsters.Clear();
    }

    //�s���̃��X�g
    List<Character> ActorList = new List<Character>();

    //�v���C���[���i�Q�[���I�[�o�[����ɗp����j
    public Player player;

    //�L�����N�^�[�̃X�e�[�^�X���X�g
    public CharacterDataBase characterDataBase;
    List<Status> statusList;

    //�s�����X�g
    public SpecialActions spact;
    public ActionList actionList;

    //���I�������u
    public CondAppGroup condappGroup;

    //�}�V���{��
    public MachineControl context;

    //AT���
    ATMODE atstate = ATMODE.NML;     //AT���
    ATMODE newatstate = ATMODE.NML;     //��AT���
    //int zen_game = 0;       //�O���Q�[����

    //�w�i�X�N���v�g
    public BGControll bg;
    //�X�e�\��
    public BasicSTDisp stdisp;

    enum VideoGameState
    {
        STANDBY,
        EFFECT1,
        EFFECT2,
        EFFECT3,
        ACTION,
        EFFECT4,
        CLEANUP
    }

    VideoGameState videoGameState;

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
    //�������o��ʒu�I�t�Z�b�g
    private int posdata_offset = 9;

    //AT��Ԋ֘A
    public void SetATState(ATMODE newatmode)
    {
        newatstate = newatmode;
    }

/*    public void SetZenGame(int set_zen_game)
    {
        zen_game = set_zen_game;        //�O���Q�[�����ݒ�
    }*/

    bool isATmodeChange()
    {
        return atstate != newatstate;
    }

    int ApperePosDecide(int posindx, List<Character> clist)
    {
        int result = posindx;
        List<int> ng_pos = new List<int>();
        //�o���ꏊ�ݒ�
        foreach (Character temp in clist)
        {
            //���łɃL�����̂���ʒu�̓X�L�b�v
            ng_pos.Add(temp.posindex);
        }

        for (int i = 0; i < 9; i++)
        {
            if (ng_pos.Contains(result))
            {
                //���łɂ��̃|�W�V�����ɃL����������
                //���̏o���|�W�V����������
                result++;
                if (result > 8) result -= 9;
            }
            else
            {
                //�o���ꏊ����������
                return result;
            }
        }
        //�o�������E
        return -1;
    }

    void AddCharacter(Status status, int posindx)
    {
        //�o���ʒu
        int pos_plan;
        //�o���ʒu����
        if (posindx < 9)
        {
            //�G�̏ꍇ
            if ((pos_plan = ApperePosDecide(posindx, EnemyList))== -1)
            {
                //�o�������E
                Debug.Log("Can't spone monster.");
                return;
            }
        }
        else
        {
            //�����̏ꍇ
            if ((pos_plan = ApperePosDecide(posindx - 9, EnemyList) + 9) == -1)
            {
                //�o�������E
                Debug.Log("Can't spone friend.");
                return;
            }
        }
        //�L�����I�u�W�F�N�g�쐬
        GameObject prefab = Resources.Load<GameObject>("GAME/newChara");
        GameObject newChara = Instantiate(prefab, this.gameObject.transform);
        newChara.transform.Translate(new Vector3(0, 0, 9.0f));
        Character newchara = newChara.GetComponent<Character>();
        //�X�e�[�^�X������
        newchara.SetStatus(status);
        //�����ҋ@���Ԑݒ�
        newchara.SetCount(newchara.GetStatus().firstwait);
        //�ʒu�ݒ�
        newchara.posindex = pos_plan;

        //�L�������X�g�ɒ��荞��
        CharacterList.Add(newchara);
        
        if (posindx < 9)
        {
            //�G�̏ꍇ
            newchara.isenemy = true;
            EnemyList.Add(newchara);
            
        }
        else
        {
            //�����̏ꍇ
            newchara.isenemy = false;
            TeamList.Add(newchara);
        }

        newChara.transform.Translate(posdata[pos_plan]);
    }

    void AddPlayer()
    {
        //��l���I�u�W�F�N�g�쐬
        GameObject prefab = Resources.Load<GameObject>("GAME/newPlayer");
        GameObject newChara = Instantiate(prefab, this.transform);
        newChara.transform.Translate(new Vector3(0, 0, 10.0f));
        Player newchara = newChara.GetComponent<Player>();
        //�X�e�[�^�X������
        newchara.SetStatus(statusList[0]);
        //�����ҋ@���Ԑݒ�
        newchara.SetCount(newchara.GetStatus().firstwait);
        //�ʒu�ݒ�
        newchara.posindex = 10;
        //�L�������X�g�ɒ��荞��
        CharacterList.Add(newchara);
        //�����̏ꍇ
        TeamList.Add(newchara);
        newChara.transform.Translate(posdata[10]);
        //�v���C���[���ۑ�
        player = newchara;
    }

    // Start is called before the first frame update
    void Start()
    {
        statusList = characterDataBase.GetStatusLists();
        //�L�����N�^�[�͎�l���������X�^�[�̏��ɐ�ɓ���
        //AddCharacter(statusList[0], 1 + posdata_offset);
        AddPlayer();
        AddCharacter(statusList[1], 1);
        AddCharacter(statusList[2], 2);
        AddCharacter(statusList[3], 0);
        AddCharacter(statusList[4], 4);
        AddCharacter(statusList[1], 7);

        //����s���f�[�^�x�[�X���쐬
        spact = new SpecialActions(this);
        //������Ԑݒ�
        videoGameState = VideoGameState.STANDBY;

        stdisp.RenewST(player.GetLevel(), player.hp, player.GetStatus().hp);
    }

    public void LotBFGame(CondAppGroup hitminor, Lottery lottery)
    {
        //�V�Z�J�n��������
        //CZ������
        //czlot = lottery.CZlottery(hitminor);

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

        //�Q�[����ԍX�V
        videoGameState = VideoGameState.ACTION;
    }

    void DeathCheck()
    {
        //�o���l�擾
        int exp = 0;
        foreach(Character tmp in EnemyList.FindAll(s => s.isdeath()))
        {
            exp += tmp.GetStatus().exp;
        }
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
        else
        {
            //���x���A�b�v
            player.EXPPlus(exp);
        }
    }



    // Update is called once per frame
    void Update()
    {
        switch (videoGameState)
        {
            case VideoGameState.ACTION: 
                if (renewactor)
                {
                    // �s��
                    ActorList[0].Action(this);
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
                        
                        if (isATmodeChange())
                        {
                            switch (newatstate)
                            {
                                case ATMODE.CZ:
                                    //�{�X�o��
                                    AddCharacter(statusList[5], 4);
                                    //�w�i��ω�
                                    bg.BGChange(2);
                                    break;
                                case ATMODE.CZ_PND:
                                    //�w�i��ω�
                                    bg.BGChange(1);
                                    break;
                                case ATMODE.BNS_PND:
                                    //�G��S��
                                    spact.SPActSelect(player, null, actionList.GetActionLists()[3]);
                                    //���S�҂��폜
                                    DeathCheck();
                                    break;
                                case ATMODE.END:
                                    //AT�I��
                                    newatstate = ATMODE.NML;
                                    break;
                                default:
                                    //�w�i��ω�
                                    bg.BGChange(0);
                                    break;
                            }
                            atstate = newatstate;
                        }
                        videoGameState = VideoGameState.CLEANUP;
                        return;
                    }
                    renewactor = true;
                }
                break;
            case VideoGameState.CLEANUP:
                //�����X�^�[�o��
                RemoveReserveMonster();
                //�X�e�[�^�X���̍X�V
                stdisp.RenewST(player.GetLevel(), player.hp, player.GetStatus().hp);
                videoGameState = VideoGameState.STANDBY;
                //�p�`�X�����̓����\
                if (player.isdeath())
                {
                    //�Q�[���I�[�o�[����
                    atstate = ATMODE.END;
                    newatstate = ATMODE.END;
                    context.FinishVideoGame(ATMODE.END);
                }
                else if (atstate == ATMODE.CZ && EnemyList.Count == 0)
                {
                    //ART�˓�
                    atstate = ATMODE.ART;
                    newatstate = ATMODE.ART;
                    context.FinishVideoGame(ATMODE.ART);
                }
                else
                {
                    context.FinishVideoGame();
                }
                
                break;
            default:
                break;
        }


    }
}
