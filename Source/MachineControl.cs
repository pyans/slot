using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineControl : MonoBehaviour
{
    // Start is called before the first frame update

    //Inspectorに複数データを表示するためのクラス
    [System.SerializableAttribute]
    public class EffectiveLineList
    {
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
    public CondAppGroup lose;
    public SymbolsData metaSymbol;
    public CondAppDataBase condAppDataBase;
    public List<CondApp> condApps;

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

    public int stoporder = 123;
    
    public CondAppGroup activeCondAppGroup;
    public CondAppGroup FlagBonus = null;

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

    GameState state;

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

    
    void musicControll(bool isstart)
    {
        if (isstart) audioSource.Play();
        else audioSource.Stop();
    }

    //ボーナステキストの有効化及び無効化
    public void SetBonusText(bool isdisp)
    {
        uiSettings.TextGameMode.enabled = isdisp;
        uiSettings.TextLastJAC.enabled = isdisp;
    }

    void ChangeMode(GameMode newgameMode)
    {
        //ゲームモード、抽せんデータ等を一括更新
        gameMode = newgameMode;
        lotteryCode.Data = lotteryDataList[(int)newgameMode];
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
        //抽選データ設定
        lotteryDataList = lotteryDataBase.GetLotteryLists();
        lotteryCode.Data = lotteryDataList[(int)gameMode];
        lotteryCode.lose = lose;
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
                        payout = 0;

                        //SE再生
                        audioSource.PlayOneShot((Sounds.GetSEs())[0]);
                    }
                    
                }

                //再遊技の場合
                if (isreplay)
                {
                    state = GameState.READY;
                    payout = 0;
                }
                break;
            case GameState.READY:
                //内部抽選など
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    //内部抽選
                    activeCondAppGroup = lotteryCode.MainLottery(setting);
                    //ボーナス内部中かつハズレを引いた場合
                    if (FlagBonus != null && activeCondAppGroup == lose)
                    {
                        //ハズレ条件装置を書き換え
                        activeCondAppGroup = FlagBonus;
                    }
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
                audioSource.PlayOneShot((Sounds.GetSEs())[1]);
                //遊技中状態へ
                state = GameState.GAME;
                break;
            case GameState.GAME:
                //遊技中
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
                //調査する条件装置の一時保存バッファ
                List<CondApp> tempCApp = condApps;
                //有効ラインをチェック
                foreach (EffectiveLineList line in effectiveline)
                {
                    foreach(CondApp condapp in tempCApp)
                    {
                        //ハズレ条件装置の場合処理をスキップ
                        if (condapp.condappType == CondApp.CondAppType.LOSE) continue;
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
                            FlagBonus = activeCondAppGroup;
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
                                ChangeMode(GameMode.NORMAL);
                                //ペカを消す
                                lamp.DisPeka();
                            }
                            //そうでなければ小役ゲームに復帰
                            else
                            {
                                ChangeMode(GameMode.BIG_GAME);
                            }
                        }
                        break;

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
