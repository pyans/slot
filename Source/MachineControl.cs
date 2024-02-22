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
    EN
}

public class MachineControl : MonoBehaviour
{
    // Start is called before the first frame update

    //Inspectorに複数データを表示するためのクラス
    [System.SerializableAttribute]
    public class EffectiveLineList
    {
        //有効ライン
        public List<int> List = new List<int>();

        public EffectiveLineList(List<int> list)
        {
            List = list;
        }
    }

    //UIのオブジェクト情報
    [System.SerializableAttribute]
    public class UISettings
    {
        //UIテキスト
        public Text TextMedal;
        public Text TextGameNum;
        public Text TextPayout;
        public Text TextGameMode;
        public Text TextLastJAC;
    }

    //停止制御スクリプト
    public StopPosDecide spDecide = new StopPosDecide();
    //抽選スクリプト
    public Lottery lotteryCode = new Lottery();
    public LotteryDataBase lotteryDataBase;
    public List<LotteryData> lotteryDataList;
    public SubLotDataBase subLotDataBase;
    public CondAppGroup lose;
    public SymbolsData metaSymbol;
    public CondAppDataBase condAppDataBase;
    public List<CondApp> condApps;      //作動可能性のある条件装置(デバッグのために残してあるが最終的に消す)

    //リール消灯情報
    public int lightoff = 0;

    //GOGOランプ
    public Lamp lamp;
    //UI情報
    public UISettings uiSettings;
    //ゲーム数
    public int gameNumber = 0;

    //Inspectorに表示される
    [SerializeField]
    private List<EffectiveLineList> effectiveline = new List<EffectiveLineList>();
    public int windowsize;
    public int setting;
    public int medal;
    public int bet;
    public int payout;
    public int payoutmax = 15;
    //JAC中ゲーム数管理
    int jacRest = 0;
    int jacWinCount = 0;
    int jacGameCount = 0;
    //リール情報
    public List<ReelScript> reel = new List<ReelScript>();
    //SEリスト
    public Sounds Sounds;
    //簡単な音楽再生
    public AudioSource audioSource;

    //押し順バッファ
    public int stoporder = 123;
    
    //作動条件装置情報
    public CondAppGroup activeCondAppGroup; //作動条件装置
    public CondApp FlagBonus = null;        //内部中役物
    public CondApp ActiveCAD = null;      //作動中役連
    public CondApp ActiveBonus = null;      //作動中役物

    //リプレイフラグ
    bool isreplay = false;

    //ゲームフロー
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

    GameState state;    //遊技機の状態

    //AT状態
    ATMODE atstate;     //AT状態
    ATMODE nextatstate;     //次AT状態
    int zen_game1;       //前兆ゲーム数1
    int zen_game2;       //前兆ゲーム数2

    //遊技状態
    enum GameMode
    {
        NORMAL = 0,
        BIG_GAME = 1,
        JAC_GAME = 2
    }

    GameMode gameMode;

    //ゲームスクリプト
    public GameMaster gameMaster;

    public void InputOrder(int reelnum)
    {
        //停止順を更新(すごく適当なのでバグに注意)
        stoporder *= 10;
        stoporder += reelnum;
    }

    //UIの更新
    public void TextUpdate()
    {
        //基本情報
        uiSettings.TextMedal.text = string.Format("持ちメダル:{0:D}枚", medal);
        uiSettings.TextPayout.text = string.Format("払い出し:{0:D}枚", payout);
        string temp;
        int temp2 = gameNumber;
        //ボーナス中の情報
        if (gameMode == GameMode.BIG_GAME || gameMode == GameMode.JAC_GAME)
        {
            if (gameMode == GameMode.BIG_GAME)
            {
                uiSettings.TextGameMode.text = "BIG CHANCE";
                temp = "残り小役ゲーム:";
            }
            else
            {
                uiSettings.TextGameMode.text = "JAC GAME";
                temp = "JACゲーム:";
                temp2 = jacWinCount;
            }
            uiSettings.TextLastJAC.text = string.Format("JAC:{0:D}/3回", jacRest);
            
        }
        else
        {
            temp = "前回ボーナスから:";
        }
        uiSettings.TextGameNum.text = string.Format(temp+"{0:D}回転", temp2);
    }

    //音楽再生
    void musicControll(bool isstart)
    {
        if (isstart) audioSource.Play();
        else audioSource.Stop();
    }

    //効果音再生
    public void SEControll(int seid)
    {
        audioSource.PlayOneShot((Sounds.GetSEs())[seid]);
    }

    //ボーナステキストの有効化及び無効化
    public void SetBonusText(bool isdisp)
    {
        uiSettings.TextGameMode.enabled = isdisp;
        uiSettings.TextLastJAC.enabled = isdisp;
    }

    //抽せんデータの変更
    void ChangeLottery(GameMode gameMode, CondApp FlagBonus, CondApp ActiveCAD, CondApp ActiveBonus)
    {
        foreach (LotteryData lotdata in lotteryDataList)
        {
            if (FlagBonus == lotdata.FlagBonus 
                && ActiveCAD == lotdata.ActiveCAD 
                && ActiveBonus == lotdata.ActiveBonus)
            {
                lotteryCode.Data = lotdata;
                return;
            }
        }
    }

    void ChangeMode(GameMode newgameMode)
    {
        //ゲームモード、抽せんデータ等を一括更新
        gameMode = newgameMode;
        ChangeLottery(newgameMode, FlagBonus, ActiveCAD, ActiveBonus);
        condAppDataBase = lotteryCode.Data.effectiveCondApp;
        condApps = condAppDataBase.GetCondAppLists();
        //テキストの表示変更
        if(newgameMode == GameMode.BIG_GAME || newgameMode == GameMode.JAC_GAME){
            SetBonusText(true);
        }else if(newgameMode == GameMode.NORMAL)
        {
            SetBonusText(false);
            //BGM停止
            musicControll(false);
        }
    }

    void Start()
    {
        //初期設定
        state = GameState.INSERT;
        gameMode = GameMode.NORMAL;
        //停止制御用初期設定
        spDecide.reels = reel;      //リール情報を横流し
        //抽選データ設定
        lotteryDataList = lotteryDataBase.GetLotteryLists();
        lotteryCode.Data = lotteryDataList[(int)gameMode];
        lotteryCode.lose = lose;
        lotteryCode.sublotlist = subLotDataBase.GetSubLotList();
        //遊技状態設定
        ChangeMode(GameMode.NORMAL);
        //テキストの更新
        TextUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.INSERT:
                //メダル投入
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    
                    if (medal >= lotteryCode.Data.betlevel)
                    {
                        //規定数分のメダルを投入
                        Debug.Log("Bet " + lotteryCode.Data.betlevel);
                        medal -= lotteryCode.Data.betlevel;
                        bet = lotteryCode.Data.betlevel;
                        state = GameState.READY;
                        
                    }
                    
                }

                //再遊技の場合
                if (isreplay) state = GameState.READY;

                //遊技スタート状態なら
                if (state == GameState.READY)
                {
                    payout = 0;

                    //リール点灯
                    foreach (ReelScript tmp in reel)
                    {
                        tmp.ReelLight.turnON();
                        lightoff = 0;
                    }

                    //SE再生
                    SEControll(0);
                }
               
                break;
            case GameState.READY:
                //内部抽選など
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    //内部抽選
                    activeCondAppGroup = lotteryCode.MainLottery(setting);
                    //ゲーム部分の抽選
                    //gameMaster.LotBFGame(activeCondAppGroup, lotteryCode);
                    //ART抽せん
                    int czhit = lotteryCode.CZlottery(activeCondAppGroup);
                    if (nextatstate == ATMODE.NML)
                    {
                        if (czhit != 0 || (activeCondAppGroup.typeofCondApp & (int)(CondAppGroup.CondAppGroupTag.RARE | CondAppGroup.CondAppGroupTag.SR)) != 0)
                        {
                            //前兆設定
                            (zen_game1, zen_game2) = lotteryCode.ZenChoulottery(czhit, activeCondAppGroup);
                            if (czhit != 0)
                            {
                                //CZ前半へ
                                nextatstate = ATMODE.CZ_PND;
                            }
                            if (zen_game1 != 0) Debug.Log("前兆突入！：ゲーム数：" + zen_game1);
                        }
                    }
                    //消灯抽せん
                    lightoff = lotteryCode.Efflottery_Lightoff(activeCondAppGroup, zen_game1);
                    //ゲーム数管理
                    switch (gameMode)
                    {
                        case GameMode.NORMAL:
                            //通常時はゲーム数増加
                            gameNumber++;
                            break;
                        case GameMode.BIG_GAME:
                            //小役ゲーム中はゲーム数減算
                            gameNumber--;
                            break;
                        case GameMode.JAC_GAME:
                            //JAC中遊技数減算
                            jacGameCount--;
                            break;

                    }
                    stoporder = 0;
                    state = GameState.START;
                }
                break;
            case GameState.START:
                //リール回転及び停止許可
                foreach (ReelScript a_reel in reel)
                {
                    a_reel.SetRollState(true);
                    a_reel.PermitStop();
                }
                //SE再生
                SEControll(1);
                //遊技中状態へ
                state = GameState.GAME;
                break;
            case GameState.GAME:

                //回胴停止
                if (Input.GetKeyDown("z") || Input.GetButtonDown("Fire3"))
                {
                    if (reel[0].IsStopOK())
                    {
                        Debug.Log("1st Stop");
                        //消灯処理
                        if (lightoff > 0)
                        {
                            lightoff--;
                            reel[0].isturnoff = true;
                        }
                        
                        InputOrder(1);
                        reel[0].PrepareForStop();
                        
                    }
                }

                if (Input.GetKeyDown("x") || Input.GetButtonDown("Fire2"))
                {
                    if (reel[1].IsStopOK())
                    {
                        Debug.Log("2nd Stop");
                        //消灯処理
                        if (lightoff > 0)
                        {
                            lightoff--;
                            reel[1].isturnoff = true;
                        }
                        InputOrder(2);
                        reel[1].PrepareForStop();
                    }
                }

                if (Input.GetKeyDown("c") || Input.GetButtonDown("Fire1"))
                {
                    if (reel[2].IsStopOK())
                    {
                        //消灯処理
                        if (lightoff > 0)
                        {
                            lightoff--;
                            reel[2].isturnoff = true;
                        }
                        Debug.Log("3rd Stop");
                        InputOrder(3);
                        reel[2].PrepareForStop();
                    }
                }

                //遊技終了判定
                bool isgameend = true;
                foreach (ReelScript a_reel in reel)
                {
                    //回転中の回胴がなければ
                    if (a_reel.GetRollState()) isgameend = false;
                }
                //遊技終了処理
                if (isgameend)
                {
                    Debug.Log("Game End.");
                    state = GameState.RESULT;
                }
                break;
            case GameState.RESULT:
                //再遊技の解除
                isreplay = false;
                //有効ラインをチェック
                foreach (EffectiveLineList line in effectiveline)
                {
                    //ハズレ条件装置の場合スキップ
                    if (activeCondAppGroup.CondApps.Count == 0) continue;
                    foreach(CondApp condapp in activeCondAppGroup.CondApps)
                    {
                        CondApp winCondApp = null;
                        foreach (CondApp.SymPatternList symlist in condapp.symPattern)
                        {
                            //図柄一致フラグ
                            bool iscacth = true;
                            for (int i = 0; i< windowsize; i++)
                            {
                                //図柄を確認
                                SymbolsData sym = reel[i].reelsymbol[reel[i].posculc(reel[i].reelpos - 1 + line.List[i])];
                                //メタシンボルは判定をスキップ
                                if (symlist.List[i] == metaSymbol)
                                {
                                    continue;
                                }
                                //図柄が判定している図柄組み合わせと違えば次の図柄組み合わせへ
                                if (sym != symlist.List[i])
                                {
                                    iscacth = false;
                                    break;
                                }
                            }

                            if (iscacth)
                            {
                                //図柄がそろっていればループ脱出
                                winCondApp = condapp;
                                break;
                            }
                        }

                        if(winCondApp is not null)
                        {
                            //勝ち取り役が入賞の場合
                            if (winCondApp.condappType == CondApp.CondAppType.WIN)
                            {
                                //払い出しを加算
                                payout += winCondApp.payoutList[(int)gameMode];
                            }
                            //勝ち取り役が再遊技の場合
                            else if (winCondApp.condappType == CondApp.CondAppType.REPLAY)
                            {
                                //再遊技フラグをON
                                isreplay = true;
                            }
                            //勝ち取り役が役連の場合
                            else if (winCondApp.condappType == CondApp.CondAppType.BONUS)
                            {
                                //作動役連フラグ設定
                                ActiveCAD = winCondApp;
                                //役物フラグを消去
                                FlagBonus = null;
                                //小役ゲームモードに移行
                                ChangeMode(GameMode.BIG_GAME);
                                //JACゲーム数、小役ゲーム数を設定
                                jacRest = winCondApp.maxJac;
                                gameNumber = winCondApp.maxMinorGame;
                                //払い出しを加算(通常時しかないぞ)
                                payout += winCondApp.payoutList[(int)GameMode.NORMAL];
                                //BGM再生
                                musicControll(true);
                            }
                            //勝ち取り役が役物の場合
                            else if (winCondApp.condappType == CondApp.CondAppType.JAC)
                            {
                                //作動役物フラグ設定
                                ActiveBonus = winCondApp;
                                //JACゲームモードに移行
                                jacRest--;
                                ChangeMode(GameMode.JAC_GAME);
                                //JACゲーム中カウンタ初期化
                                jacGameCount = 12;
                                jacWinCount = 8;
                            }
                        }
                    }
                }

                //役物チェック
                //通常時のみ役連フラグ成立でフラグを保存
                if (gameMode == GameMode.NORMAL && activeCondAppGroup != lose)
                {
                    foreach (CondApp condapp in activeCondAppGroup.CondApps)
                    {
                        //役物タイプが作動した場合
                        if (condapp.condappType == CondApp.CondAppType.BONUS)
                        {
                            //当選した役物を保存
                            FlagBonus = condapp;
                            //内部中へ移行
                            ChangeLottery(gameMode, FlagBonus, ActiveCAD, ActiveBonus);
                            //ペカらせる
                            lamp.Peka();
                        }
                    }
                }

                //払い出し枚数の上限をチェック
                if (payout > payoutmax)
                {
                    payout = payoutmax;
                }
                //払い出しへ
                state = GameState.PAY;
                break;
            case GameState.PAY:
                //払い出し
                medal += payout;
                //遊技終了時ゲーム数管理
                switch (gameMode)
                {
                    case GameMode.BIG_GAME:
                        if(gameNumber <= 0)
                        {
                            //小役ゲーム終了で通常時へ
                            gameNumber = 0;
                            ActiveCAD = null;
                            ChangeMode(GameMode.NORMAL);
                            //ペカを消す
                            lamp.DisPeka();
                        }
                        break;
                    case GameMode.JAC_GAME:
                        //JAC中入賞回数減算
                        if (payout > 0) jacWinCount--;
                        //回数0でゲームモード遷移
                        if(jacGameCount <= 0 || jacWinCount <= 0)
                        {
                            //残りJAC回数0で通常時へ
                            if(jacRest <= 0)
                            {
                                gameNumber = 0;
                                ActiveCAD = null;
                                ActiveBonus = null;
                                ChangeMode(GameMode.NORMAL);
                                //ペカを消す
                                lamp.DisPeka();
                            }
                            //そうでなければ小役ゲームに復帰
                            else
                            {
                                ActiveBonus = null;
                                ChangeMode(GameMode.BIG_GAME);
                            }
                        }
                        break;

                }
                //前兆減算
                zen_game1--;
                if (zen_game1 < 0)
                {
                    //ゲーム側に通達
                    atstate = nextatstate;
                    //次状態設定
                    if (nextatstate == ATMODE.CZ_PND)
                    {
                        nextatstate = ATMODE.CZ;
                        zen_game1 = zen_game2;
                    }
                    else
                    {
                        //前兆を初期化
                        zen_game1 = 0;
                    }
                    //前兆更新
                    gameMaster.SetATState(atstate);
                }
                //ゲーム更新
                gameMaster.UpdateGame(activeCondAppGroup);
                
                //メダル投入待機へ
                state = GameState.INSERT;
                break;
            default:
                //まだ処理を作っていない
                break;
        }

        //情報更新
        TextUpdate();
    }
}
