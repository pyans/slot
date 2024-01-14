using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ATMODE
{
    NML,
    CZ_PND,
    CZ,
    BNS_PND,
    BNS,
    ART,
    UWNS,
    SPBNS,
    USPBNS,
    END
}

public class MachineControl : MonoBehaviour
{
    // Start is called before the first frame update

    //Inspector�ɕ����f�[�^��\�����邽�߂̃N���X
    [System.SerializableAttribute]
    public class EffectiveLineList
    {
        //�L�����C��
        public List<int> List = new List<int>();

        public EffectiveLineList(List<int> list)
        {
            List = list;
        }
    }

    //UI�̃I�u�W�F�N�g���
    [System.SerializableAttribute]
    public class UISettings
    {
        //UI�e�L�X�g
        public Text TextMedal;
        public Text TextGameNum;
        public Text TextPayout;
        public Text TextGameMode;
        public Text TextLastJAC;
    }

    //��~����X�N���v�g
    public StopPosDecide spDecide = new StopPosDecide();
    //���I�X�N���v�g
    public Lottery lotteryCode = new Lottery();
    public LotteryDataBase lotteryDataBase;
    public List<LotteryData> lotteryDataList;
    public MonsterTableDB monsterTableDBs;
    public SubLotDataBase subLotDataBase;
    public CondAppGroup lose;
    public SymbolsData metaSymbol;
    public CondAppDataBase condAppDataBase;
    public List<CondApp> condApps;      //�쓮�\���̂���������u(�f�o�b�O�̂��߂Ɏc���Ă��邪�ŏI�I�ɏ���)

    //���o�X�N���v�g
    public MovieController movieController;

    //���[���������
    public int lightoff = 0;

    //GOGO�����v
    public Lamp lamp;
    //UI���
    public UISettings uiSettings;
    //�Q�[����
    public int gameNumber = 0;

    //Inspector�ɕ\�������
    [SerializeField]
    private List<EffectiveLineList> effectiveline = new List<EffectiveLineList>();
    public int windowsize;
    public int setting;
    public int medal;
    public int bet;
    public int payout;
    public int payoutmax = 15;
    //JAC���Q�[�����Ǘ�
    int jacRest = 0;
    int jacWinCount = 0;
    int jacGameCount = 0;
    //RT
    public RTinfo RT1;
    //���[�����
    public List<ReelScript> reel = new List<ReelScript>();
    //SE���X�g
    public Sounds Sounds;
    //�ȒP�ȉ��y�Đ�
    public AudioSource audioSource;

    //�������o�b�t�@
    public int stoporder = 0;
    
    //�쓮�������u���
    public CondAppGroup activeCondAppGroup; //�쓮�������u
    public CondApp FlagBonus = null;        //��������
    public CondApp ActiveCAD = null;      //�쓮����A
    public CondApp ActiveBonus = null;      //�쓮����

    //���v���C�t���O
    bool isreplay = false;

    //�Q�[���t���[
    enum GameState
    {
        SETUP,
        INSERT,
        READY,
        PRE_EFF,
        START,
        GAME,
        RESULT,
        POST_EFF,
        PAY,
    }

    GameState state;    //�V�Z�@�̏��

    //AT���
    ATMODE atstate;     //AT���
    ATMODE nextatstate;     //��AT���
    int zen_game1;       //�O���Q�[����1
    int zen_game2;       //�O���Q�[����2

    //�V�Z���
    enum GameMode
    {
        NORMAL = 0,
        BIG_GAME = 1,
        JAC_GAME = 2
    }

    GameMode gameMode;

    //�Q�[���X�N���v�g
    public GameMaster gameMaster;

    //�f�o�b�O�p�ϐ�
    public CondAppGroup force_activeCondAppGroup; //�����쓮�������u

    public ATMODE GetATMODE()
    {
        return atstate;
    }

    public void SetATMODE(ATMODE newstate)
    {
        atstate = newstate;
    }

    public void InputOrder(int reelnum)
    {
        //��~�����X�V(�������K���Ȃ̂Ńo�O�ɒ���)
        stoporder *= 10;
        stoporder += reelnum;
    }

    //UI�̍X�V
    public void TextUpdate()
    {
        //��{���
        uiSettings.TextMedal.text = string.Format("�������_��:{0:D}��", medal);
        uiSettings.TextPayout.text = string.Format("�����o��:{0:D}��", payout);
        string temp;
        int temp2 = gameNumber;
        //�{�[�i�X���̏��
        if (gameMode == GameMode.BIG_GAME || gameMode == GameMode.JAC_GAME)
        {
            if (gameMode == GameMode.BIG_GAME)
            {
                uiSettings.TextGameMode.text = "BIG CHANCE";
                temp = "�c�菬���Q�[��:";
            }
            else
            {
                uiSettings.TextGameMode.text = "JAC GAME";
                temp = "JAC�Q�[��:";
                temp2 = jacWinCount;
            }
            uiSettings.TextLastJAC.text = string.Format("JAC:{0:D}/3��", jacRest);
            
        }
        else
        {
            temp = "�O��{�[�i�X����:";
        }
        uiSettings.TextGameNum.text = string.Format(temp+"{0:D}��]", temp2);
    }

    //���y�Đ�
    void musicControll(bool isstart)
    {
        if (isstart) audioSource.Play();
        else audioSource.Stop();
    }

    //���ʉ��Đ�
    public void SEControll(int seid)
    {
        audioSource.PlayOneShot((Sounds.GetSEs())[seid]);
    }

    //�e���p�C�`�F�b�N
    public int isTenpai()
    {
        //��2��~�ȊO�͏����X�L�b�v
        if (stoporder < 10 || stoporder > 100) return 0;
        //�L�����C�����`�F�b�N
        foreach (EffectiveLineList line in effectiveline)
        {
            foreach (CondApp condapp in condApps)
            {
                //�{�[�i�X�ȊO�̓X�L�b�v
                if (condapp.condappType != CondApp.CondAppType.BONUS) continue;
                CondApp winCondApp = null;
                SymbolsData sym;
                foreach (CondApp.SymPatternList symlist in condapp.symPattern)
                {
                    //�}����v�t���O
                    bool iscacth = true;
                    for (int i = 0; i < windowsize; i++)
                    {
                        //��]���̃��[���͏������X�L�b�v
                        if (reel[i].GetRollState()) continue;
                        //�}�����m�F
                        sym = reel[i].reelsymbol[reel[i].posculc(reel[i].reelpos - 1 + line.List[i])];
                        //���^�V���{���͔�����X�L�b�v
                        if (symlist.List[i] == metaSymbol)
                        {
                            continue;
                        }
                        //�}�������肵�Ă���}���g�ݍ��킹�ƈႦ�Ύ��̐}���g�ݍ��킹��
                        if (sym != symlist.List[i])
                        {
                            iscacth = false;
                            break;
                        }
                    }

                    if (iscacth)
                    {
                        //�}����������Ă���΃��[�v�E�o
                        winCondApp = condapp;
                        break;
                    }
                }

                if (winCondApp is not null)
                {
                    //�{�[�i�X�e���p�C
                    return 1;
                }
            }
        }
        return 0;
    }

    //�{�[�i�X�e�L�X�g�̗L�����y�і�����
    public void SetBonusText(bool isdisp)
    {
        uiSettings.TextGameMode.enabled = isdisp;
        uiSettings.TextLastJAC.enabled = isdisp;
    }

    //������f�[�^�̕ύX
    void ChangeLottery(GameMode gameMode, CondApp FlagBonus, CondApp ActiveCAD, CondApp ActiveBonus, RTinfo rt = null)
    {
        foreach (LotteryData lotdata in lotteryDataList)
        {
            if (FlagBonus == lotdata.FlagBonus 
                && ActiveCAD == lotdata.ActiveCAD 
                && ActiveBonus == lotdata.ActiveBonus)
            {
                //RT���̔���
                if (rt != null && lotdata.RTinfo != rt) continue;
                lotteryCode.Data = lotdata;
                return;
            }
        }
    }

    void ChangeMode(GameMode newgameMode)
    {
        //�Q�[�����[�h�A������f�[�^�����ꊇ�X�V
        gameMode = newgameMode;
        ChangeLottery(newgameMode, FlagBonus, ActiveCAD, ActiveBonus);
        condAppDataBase = lotteryCode.Data.effectiveCondApp;
        condApps = condAppDataBase.GetCondAppLists();
        //�e�L�X�g�̕\���ύX
        if(newgameMode == GameMode.BIG_GAME || newgameMode == GameMode.JAC_GAME){
            SetBonusText(true);
        }else if(newgameMode == GameMode.NORMAL)
        {
            SetBonusText(false);
            //BGM��~
            musicControll(false);
        }
    }

    void LotForGame()
    {
        //������I�t�Z�b�g����
        int index = 0;
        if (activeCondAppGroup.isRAREtype())
        {
            index = 1;
        }
        //�����񌋉ʂ�1�Ȃ烂���X�^�[�o��
        if (lotteryCode.SubLottery_1B(index, subLotDataBase.GetSubLotList()[4]) == 1)
        {
            //�����X�^�[��������
            int apparenum = lotteryCode.SubLottery_1B(index, subLotDataBase.GetSubLotList()[5]);
            for (; apparenum > 0; apparenum--)
            {
                //�o�������X�^�[�𒊑I
                int result = lotteryCode.SubLottery_1B(index, subLotDataBase.GetSubLotList()[6]);
                //�o���e�[�u�����璊����
                Status hit = monsterTableDBs.GetMonsterTable()[0].hit_monster[result];
                gameMaster.AddReserveMonster(hit);
            }
        }
    }

    void Start()
    {
        //�����ݒ�
        state = GameState.INSERT;
        gameMode = GameMode.NORMAL;
        //��~����p�����ݒ�
        spDecide.reels = reel;      //���[������������
        //���I�f�[�^�ݒ�
        lotteryDataList = lotteryDataBase.GetLotteryLists();
        lotteryCode.lose = lose;
        lotteryCode.sublotlist = subLotDataBase.GetSubLotList();
        //�V�Z��Ԑݒ�
        ChangeMode(GameMode.NORMAL);
        lotteryCode.Data = lotteryDataList[7];//RTDEBUG
        //�e�L�X�g�̍X�V
        TextUpdate();
    }


    public void FinishVideoGame(ATMODE ATstate)
    {
        //�Q�[�������I��
        atstate = ATstate;
        nextatstate = ATstate;
        state = GameState.INSERT;
    }

    public void FinishVideoGame()
    {
        //�Q�[�������I��
        state = GameState.INSERT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.INSERT:
                //���_������
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    
                    if (medal >= lotteryCode.Data.betlevel)
                    {
                        //�K�萔���̃��_���𓊓�
                        Debug.Log("Bet " + lotteryCode.Data.betlevel);
                        medal -= lotteryCode.Data.betlevel;
                        bet = lotteryCode.Data.betlevel;
                        state = GameState.READY;

                    }
                    else
                    {
                        //���_��������Ȃ��ꍇ�A50���ǉ�
                        Debug.Log("Insert 1,000 Yen.");
                        Debug.Log("Medal +50");
                        medal += 50;
                        //�K�萔���̃��_���𓊓�
                        Debug.Log("Bet " + lotteryCode.Data.betlevel);
                        medal -= lotteryCode.Data.betlevel;
                        bet = lotteryCode.Data.betlevel;
                        state = GameState.READY;

                        //SE�Đ�
                        SEControll(5);
                    }
                    
                }

                //�ėV�Z�̏ꍇ
                if (isreplay) state = GameState.READY;

                //�V�Z�X�^�[�g��ԂȂ�
                if (state == GameState.READY)
                {
                    payout = 0;

                    //���[���_��
                    foreach (ReelScript tmp in reel)
                    {
                        tmp.ReelLight.turnON();
                        lightoff = 0;
                    }

                    //SE�Đ�
                    SEControll(0);
                }
               
                break;
            case GameState.READY:
                //�������I�Ȃ�
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    stoporder = 0;                          //��~�����Z�b�g    
                    //�������I
                    activeCondAppGroup = lotteryCode.MainLottery(setting);
                    if(force_activeCondAppGroup != null)
                    {
                        //�f�o�b�O:���I�������u�̏�������
                        activeCondAppGroup = force_activeCondAppGroup;
                    }
                    //�Q�[�������̒��I
                    //gameMaster.LotBFGame(activeCondAppGroup, lotteryCode);
                    switch (atstate)
                    {
                        case ATMODE.NML:
                            //ART������
                            int czhit = 0;
                            if (!activeCondAppGroup.isBONUS()) czhit = lotteryCode.CZlottery(activeCondAppGroup);
                            if (nextatstate == ATMODE.NML)
                            {
                                //�O���ݒ�
                                (zen_game1, zen_game2) = lotteryCode.ZenChoulottery(czhit, activeCondAppGroup);
                                if (czhit != 0)
                                {
                                    //CZ�O����
                                    Debug.Log("CZ���I:���e:" + czhit);
                                    nextatstate = ATMODE.CZ_PND;
                                }
                                else if(activeCondAppGroup.isBONUS())
                                {
                                    //�{�i���I
                                    Debug.Log("BB���I!");
                                    nextatstate = ATMODE.BNS_PND;
                                }
                                if (zen_game1 != 0) Debug.Log("�O���˓��I�F�Q�[�����F" + zen_game1);

                                //�Q�[��������
                                LotForGame();
                            }

                            //����������
                            lightoff = lotteryCode.Efflottery_Lightoff(activeCondAppGroup, zen_game1);
                            break;
                        case ATMODE.BNS_PND:
                            movieController.MoviePlay(0);
                            break;
                        case ATMODE.BNS:
                            //�𕨒�
                            break;
                        case ATMODE.ART:
                            //�i�r�\��
                            movieController.NabiSet(activeCondAppGroup);    //�i�r�̃Z�b�g
                            break;
                        default:
                            break;
                    }
                    
                    
                    //�Q�[�����Ǘ�
                    switch (gameMode)
                    {
                        case GameMode.NORMAL:
                            //�ʏ펞�̓Q�[��������
                            gameNumber++;
                            break;
                        case GameMode.BIG_GAME:
                            //�����Q�[�����̓Q�[�������Z
                            gameNumber--;
                            break;
                        case GameMode.JAC_GAME:
                            //JAC���V�Z�����Z
                            jacGameCount--;
                            break;

                    }
                    
                    state = GameState.START;
                }
                break;
            case GameState.START:
                //���[����]�y�ђ�~����
                foreach (ReelScript a_reel in reel)
                {
                    a_reel.SetRollState(true);
                    a_reel.PermitStop();
                }
                //SE�Đ�
                SEControll(1);
                //�V�Z����Ԃ�
                state = GameState.GAME;
                break;
            case GameState.GAME:

                //�񓷒�~
                if (Input.GetKeyDown("z") || Input.GetButtonDown("Fire3"))
                {
                    if (reel[0].IsStopOK())
                    {
                        Debug.Log("1st Stop");
                        //��������
                        if (lightoff > 0)
                        {
                            lightoff--;
                            reel[0].isturnoff = true;
                        }
                        
                        InputOrder(1);
                        reel[0].PrepareForStop();
                        movieController.NabiUpdate(stoporder, activeCondAppGroup);        //�i�r�̍X�V
                    }
                }

                if (Input.GetKeyDown("x") || Input.GetButtonDown("Fire2"))
                {
                    if (reel[1].IsStopOK())
                    {
                        Debug.Log("2nd Stop");
                        //��������
                        if (lightoff > 0)
                        {
                            lightoff--;
                            reel[1].isturnoff = true;
                        }
                        InputOrder(2);
                        reel[1].PrepareForStop();
                        movieController.NabiUpdate(stoporder, activeCondAppGroup);        //�i�r�̍X�V
                    }
                }

                if (Input.GetKeyDown("c") || Input.GetButtonDown("Fire1"))
                {
                    if (reel[2].IsStopOK())
                    {
                        //��������
                        if (lightoff > 0)
                        {
                            lightoff--;
                            reel[2].isturnoff = true;
                        }
                        Debug.Log("3rd Stop");
                        InputOrder(3);
                        reel[2].PrepareForStop();
                        movieController.NabiUpdate(stoporder, activeCondAppGroup);        //�i�r�̍X�V
                    }
                }

                //�V�Z�I������
                bool isgameend = true;
                foreach (ReelScript a_reel in reel)
                {
                    //��]���̉񓷂��Ȃ����
                    if (a_reel.GetRollState()) isgameend = false;
                }
                //�V�Z�I������
                if (isgameend)
                {
                    Debug.Log("Game End.");
                    state = GameState.RESULT;
                }
                break;
            case GameState.RESULT:
                //�ėV�Z�̉���
                isreplay = false;
                //�L�����C�����`�F�b�N
                foreach (EffectiveLineList line in effectiveline)
                {
                    //�n�Y���������u�̏ꍇ�X�L�b�v
                    if (activeCondAppGroup.CondApps.Count == 0) continue;
                    foreach(CondApp condapp in activeCondAppGroup.CondApps)
                    {
                        CondApp winCondApp = null;
                        SymbolsData sym;
                        foreach (CondApp.SymPatternList symlist in condapp.symPattern)
                        {
                            //�}����v�t���O
                            bool iscacth = true;
                            for (int i = 0; i< windowsize; i++)
                            {
                                //�}�����m�F
                                sym = reel[i].reelsymbol[reel[i].posculc(reel[i].reelpos - 1 + line.List[i])];
                                //���^�V���{���͔�����X�L�b�v
                                if (symlist.List[i] == metaSymbol)
                                {
                                    continue;
                                }
                                //�}�������肵�Ă���}���g�ݍ��킹�ƈႦ�Ύ��̐}���g�ݍ��킹��
                                if (sym != symlist.List[i])
                                {
                                    iscacth = false;
                                    break;
                                }
                            }

                            if (iscacth)
                            {
                                //�}����������Ă���΃��[�v�E�o
                                winCondApp = condapp;
                                break;
                            }
                        }

                        if(winCondApp is not null)
                        {
                            //�������������܂̏ꍇ
                            if (winCondApp.condappType == CondApp.CondAppType.WIN)
                            {
                                //�����o�������Z
                                payout += winCondApp.payoutList[(int)gameMode];
                            }
                            //�����������ėV�Z�̏ꍇ
                            else if (winCondApp.condappType == CondApp.CondAppType.REPLAY)
                            {
                                //�ėV�Z�t���O��ON
                                isreplay = true;
                             }
                            //������������A�̏ꍇ
                            else if (winCondApp.condappType == CondApp.CondAppType.BONUS)
                            {
                                //�쓮��A�t���O�ݒ�
                                ActiveCAD = winCondApp;
                                //�𕨃t���O������
                                FlagBonus = null;
                                //�����Q�[�����[�h�Ɉڍs
                                ChangeMode(GameMode.BIG_GAME);
                                //JAC�Q�[�����A�����Q�[������ݒ�
                                jacRest = winCondApp.maxJac;
                                gameNumber = winCondApp.maxMinorGame;
                                //�����o�������Z(�ʏ펞�����Ȃ���)
                                payout += winCondApp.payoutList[(int)GameMode.NORMAL];
                                //BGM�Đ�
                                musicControll(true);
                            }
                            //�����������𕨂̏ꍇ
                            else if (winCondApp.condappType == CondApp.CondAppType.JAC)
                            {
                                //�쓮�𕨃t���O�ݒ�
                                ActiveBonus = winCondApp;
                                //JAC�Q�[�����[�h�Ɉڍs
                                jacRest--;
                                ChangeMode(GameMode.JAC_GAME);
                                //JAC�Q�[�����J�E���^������
                                jacGameCount = 12;
                                jacWinCount = 8;
                            }

                            if (FlagBonus == null && winCondApp.gotoRT != null)
                            {
                                //d�_�@�}����RT�J��
                                ChangeLottery(gameMode, FlagBonus, ActiveCAD, ActiveBonus, winCondApp.gotoRT);
                            }
                        }
                    }
                }

                //�𕨃`�F�b�N
                //�ʏ펞�̂ݖ�A�t���O�����Ńt���O��ۑ�
                if (gameMode == GameMode.NORMAL && activeCondAppGroup != lose)
                {
                    foreach (CondApp condapp in activeCondAppGroup.CondApps)
                    {
                        //�𕨃^�C�v���쓮�����ꍇ
                        if (condapp.condappType == CondApp.CondAppType.BONUS)
                        {
                            //���I�����𕨂�ۑ�
                            FlagBonus = condapp;
                            //�������ֈڍs
                            ChangeLottery(gameMode, FlagBonus, ActiveCAD, ActiveBonus);
                            //�y�J�点��
                            lamp.Peka();
                        }
                    }
                }

                //RT�J�ڃ`�F�b�N
                if(activeCondAppGroup.isBELL() && payout == 0)
                {
                    //d�_�@�}����RT�J��(�������x�����ڂ�)
                    ChangeLottery(gameMode, FlagBonus, ActiveCAD, ActiveBonus, RT1);
                }

                //�����o�������̏�����`�F�b�N
                if (payout > payoutmax)
                {
                    payout = payoutmax;
                }
                //�����o����
                state = GameState.PAY;
                break;
            case GameState.PAY:
                //�����o��
                medal += payout;
                //�V�Z�I�����Q�[�����Ǘ�
                switch (gameMode)
                {
                    case GameMode.BIG_GAME:
                        if(gameNumber <= 0)
                        {
                            //�����Q�[���I���Œʏ펞��
                            gameNumber = 0;
                            ActiveCAD = null;
                            ChangeMode(GameMode.NORMAL);
                            //�y�J������
                            lamp.DisPeka();
                        }
                        break;
                    case GameMode.JAC_GAME:
                        //JAC�����܉񐔌��Z
                        if (payout > 0) jacWinCount--;
                        //��0�ŃQ�[�����[�h�J��
                        if(jacGameCount <= 0 || jacWinCount <= 0)
                        {
                            //�c��JAC��0�Œʏ펞��
                            if(jacRest <= 0)
                            {
                                gameNumber = 0;
                                ActiveCAD = null;
                                ActiveBonus = null;
                                ChangeMode(GameMode.NORMAL);
                                //�y�J������
                                lamp.DisPeka();
                            }
                            //�����łȂ���Ώ����Q�[���ɕ��A
                            else
                            {
                                ActiveBonus = null;
                                ChangeMode(GameMode.BIG_GAME);
                            }
                        }
                        break;

                }
                //�O�����Z
                if (zen_game1 > 0)
                {
                    zen_game1--;
                    if (zen_game1 == 0)
                    {
                        //��ԍX�V
                        atstate = nextatstate;
                        //����Ԑݒ�
                        switch (nextatstate)
                        {
                            case ATMODE.CZ_PND:
                                nextatstate = ATMODE.CZ;
                                zen_game1 = zen_game2;
                                break;
                            case ATMODE.BNS_PND:
                                nextatstate = ATMODE.BNS;
                                zen_game1 = 0;
                                break;
                            default:
                                nextatstate = ATMODE.NML;
                                zen_game1 = 0;
                                break;
                        }
                        //�Q�[�����ɒʒB
                        gameMaster.SetATState(atstate);
                    }
                }
                
                //�Q�[���X�V
                gameMaster.UpdateGame(activeCondAppGroup);
                
                //���_�������ҋ@��
                state = GameState.SETUP;
                break;
            default:
                //�܂�����������Ă��Ȃ�
                break;
        }

        //���X�V
        TextUpdate();
    }
}
