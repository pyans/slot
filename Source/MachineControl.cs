using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineControl : MonoBehaviour
{
    // Start is called before the first frame update

    //Inspector�ɕ����f�[�^��\�����邽�߂̃N���X
    [System.SerializableAttribute]
    public class EffectiveLineList
    {
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
    public CondAppGroup lose;
    public SymbolsData metaSymbol;
    public CondAppDataBase condAppDataBase;
    public List<CondApp> condApps;

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
    //���[�����
    public List<ReelScript> reel = new List<ReelScript>();
    //SE���X�g
    public Sounds Sounds;
    //�ȒP�ȉ��y�Đ�
    public AudioSource audioSource;

    public int stoporder = 123;
    
    public CondAppGroup activeCondAppGroup;
    public CondAppGroup FlagBonus = null;

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

    GameState state;

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

    
    void musicControll(bool isstart)
    {
        if (isstart) audioSource.Play();
        else audioSource.Stop();
    }

    //�{�[�i�X�e�L�X�g�̗L�����y�і�����
    public void SetBonusText(bool isdisp)
    {
        uiSettings.TextGameMode.enabled = isdisp;
        uiSettings.TextLastJAC.enabled = isdisp;
    }

    void ChangeMode(GameMode newgameMode)
    {
        //�Q�[�����[�h�A������f�[�^�����ꊇ�X�V
        gameMode = newgameMode;
        lotteryCode.Data = lotteryDataList[(int)newgameMode];
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

    void Start()
    {
        //�����ݒ�
        state = GameState.INSERT;
        gameMode = GameMode.NORMAL;
        //���I�f�[�^�ݒ�
        lotteryDataList = lotteryDataBase.GetLotteryLists();
        lotteryCode.Data = lotteryDataList[(int)gameMode];
        lotteryCode.lose = lose;
        //�V�Z��Ԑݒ�
        ChangeMode(GameMode.NORMAL);
        //�e�L�X�g�̍X�V
        TextUpdate();
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
                        payout = 0;

                        //SE�Đ�
                        audioSource.PlayOneShot((Sounds.GetSEs())[0]);
                    }
                    
                }

                //�ėV�Z�̏ꍇ
                if (isreplay)
                {
                    state = GameState.READY;
                    payout = 0;
                }
                break;
            case GameState.READY:
                //�������I�Ȃ�
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    //�������I
                    activeCondAppGroup = lotteryCode.MainLottery(setting);
                    //�{�[�i�X���������n�Y�����������ꍇ
                    if (FlagBonus != null && activeCondAppGroup == lose)
                    {
                        //�n�Y���������u����������
                        activeCondAppGroup = FlagBonus;
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
                    stoporder = 0;
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
                audioSource.PlayOneShot((Sounds.GetSEs())[1]);
                //�V�Z����Ԃ�
                state = GameState.GAME;
                break;
            case GameState.GAME:
                //�V�Z��
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
                //��������������u�̈ꎞ�ۑ��o�b�t�@
                List<CondApp> tempCApp = condApps;
                //�L�����C�����`�F�b�N
                foreach (EffectiveLineList line in effectiveline)
                {
                    foreach(CondApp condapp in tempCApp)
                    {
                        //�n�Y���������u�̏ꍇ�������X�L�b�v
                        if (condapp.condappType == CondApp.CondAppType.LOSE) continue;
                        CondApp winCondApp = null;
                        foreach (CondApp.SymPatternList symlist in condapp.symPattern)
                        {
                            //�}����v�t���O
                            bool iscacth = true;
                            for (int i = 0; i< windowsize; i++)
                            {
                                //�}�����m�F
                                SymbolsData sym = reel[i].reelsymbol[reel[i].posculc(reel[i].reelpos - 1 + line.List[i])];
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
                                //JAC�Q�[�����[�h�Ɉڍs
                                jacRest--;
                                ChangeMode(GameMode.JAC_GAME);
                                //JAC�Q�[�����J�E���^������
                                jacGameCount = 12;
                                jacWinCount = 8;
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
                            FlagBonus = activeCondAppGroup;
                            //�y�J�点��
                            lamp.Peka();
                        }
                    }
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
                                ChangeMode(GameMode.NORMAL);
                                //�y�J������
                                lamp.DisPeka();
                            }
                            //�����łȂ���Ώ����Q�[���ɕ��A
                            else
                            {
                                ChangeMode(GameMode.BIG_GAME);
                            }
                        }
                        break;

                }
                //�Q�[���X�V
                gameMaster.UpdateGame(activeCondAppGroup);
                //���_�������ҋ@��
                state = GameState.INSERT;
                break;
            default:
                //�܂�����������Ă��Ȃ�
                break;
        }

        //���X�V
        TextUpdate();
    }
}
