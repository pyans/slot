using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //キャラクターのリスト
    List<Character> CharacterList = new List<Character>();
    //敵及び味方のリスト
    List<Character> TeamList = new List<Character>();
    List<Character> EnemyList = new List<Character>();
    //行動のリスト
    List<Character> ActorList = new List<Character>();

    //プレイヤー情報（ゲームオーバー判定に用いる）
    public Player player;

    //キャラクターのステータスリスト
    public CharacterDataBase characterDataBase;
    List<Status> statusList;
    //行動リスト
    public SpecialActions spact;

    //当選条件装置
    CondAppGroup condappGroup;

    //AT状態
    ATMODE atstate = ATMODE.NML;     //AT状態
    ATMODE newatstate = ATMODE.NML;     //次AT状態
    //int zen_game = 0;       //前兆ゲーム数

    //背景スクリプト
    public BGControll bg;
    //ステ表示
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

    //キャラ登場位置データ
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

    //行動者更新フラグ
    bool renewactor;
    //味方側登場位置オフセット
    private int posdata_offset = 9;

    //AT状態関連
    public void SetATState(ATMODE newatmode)
    {
        newatstate = newatmode;
    }

/*    public void SetZenGame(int set_zen_game)
    {
        zen_game = set_zen_game;        //前兆ゲーム数設定
    }*/

    bool isATmodeChange()
    {
        return (atstate != newatstate);
    }

    int ApperePosDecide(int posindx, List<Character> clist)
    {
        int result = posindx;
        List<int> ng_pos = new List<int>();
        //出現場所設定
        foreach (Character temp in clist)
        {
            //すでにキャラのいる位置はスキップ
            ng_pos.Add(temp.posindex);
        }

        for (int i = 0; i < 9; i++)
        {
            if (ng_pos.Contains(result))
            {
                //すでにそのポジションにキャラがいる
                //次の出現ポジションを検索
                result++;
                if (result > 8) result -= 9;
            }
            else
            {
                //出現場所が見つかった
                return result;
            }
        }
        //出現数限界
        return -1;
    }

    void AddCharacter(Status status, int posindx)
    {
        //出現位置
        int pos_plan;
        //出現位置決定
        if (posindx < 9)
        {
            //敵の場合
            if ((pos_plan = ApperePosDecide(posindx, EnemyList))== -1)
            {
                //出現数限界
                Debug.Log("Can't spone monster.");
                return;
            }
        }
        else
        {
            //味方の場合
            if ((pos_plan = ApperePosDecide(posindx - 9, EnemyList) + 9) == -1)
            {
                //出現数限界
                Debug.Log("Can't spone friend.");
                return;
            }
        }
        //キャラオブジェクト作成
        GameObject prefab = Resources.Load<GameObject>("GAME/newChara");
        GameObject newChara = Instantiate(prefab, this.gameObject.transform);
        Character newchara = newChara.GetComponent<Character>();
        //ステータス初期化
        newchara.SetStatus(status);
        //初期待機時間設定
        newchara.SetCount(newchara.GetStatus().firstwait);
        //位置設定
        newchara.posindex = pos_plan;

        //キャラリストに彫り込む
        CharacterList.Add(newchara);
        
        if (posindx < 9)
        {
            //敵の場合
            newchara.isenemy = true;
            EnemyList.Add(newchara);
            
        }
        else
        {
            //味方の場合
            newchara.isenemy = false;
            TeamList.Add(newchara);
        }

        newChara.transform.Translate(posdata[pos_plan]);
    }

    void AddPlayer()
    {
        //主人公オブジェクト作成
        GameObject prefab = Resources.Load<GameObject>("GAME/newPlayer");
        GameObject newChara = Instantiate(prefab, this.gameObject.transform);
        Player newchara = newChara.GetComponent<Player>();
        //ステータス初期化
        newchara.SetStatus(statusList[0]);
        //初期待機時間設定
        newchara.SetCount(newchara.GetStatus().firstwait);
        //位置設定
        newchara.posindex = 10;
        //キャラリストに彫り込む
        CharacterList.Add(newchara);
        //味方の場合
        TeamList.Add(newchara);
        newChara.transform.Translate(posdata[10]);
        //プレイヤー情報保存
        player = newchara;
    }

    // Start is called before the first frame update
    void Start()
    {
        statusList = characterDataBase.GetStatusLists();
        //キャラクターは主人公→モンスターの順に先に動く
        //AddCharacter(statusList[0], 1 + posdata_offset);
        AddPlayer();
        AddCharacter(statusList[1], 1);
        AddCharacter(statusList[2], 2);
        AddCharacter(statusList[3], 0);
        AddCharacter(statusList[4], 4);
        AddCharacter(statusList[1], 7);

        //特殊行動データベースを作成
        spact = new SpecialActions(CharacterList, TeamList, EnemyList);
        //初期状態設定
        videoGameState = VideoGameState.STANDBY;

        stdisp.RenewST(player.GetLevel(), player.hp, player.GetStatus().hp);
    }

    public void LotBFGame(CondAppGroup hitminor, Lottery lottery)
    {
        //遊技開始時抽せん
        //CZ抽せん
        //czlot = lottery.CZlottery(hitminor);

    }

    public void UpdateGame(CondAppGroup hitminor)
    {
        //ターン終了時処理
        //条件装置情報更新
        condappGroup = hitminor;

        //行動待機カウンタ更新
        foreach (Character temp in CharacterList)
        {
            //死んでいたらスキップ
            if (temp.isdeath()) continue;
            //行動待ち時間-1
            temp.DecCount();
            if(temp.waitcount <= 0)
            {
                //行動リストに追加
                ActorList.Add(temp);
                //カウントリセット
                temp.ResetCount();
            }
        }

        //行動するキャラがいたら更新
        if(ActorList.Count != 0)
        {
            renewactor = true;
        }

        //ゲーム状態更新
        videoGameState = VideoGameState.ACTION;
    }

    void DeathCheck()
    {
        //経験値取得
        int exp = 0;
        foreach(Character tmp in EnemyList.FindAll(s => s.isdeath()))
        {
            exp += tmp.GetStatus().exp;
        }
        //すべてのリストから死者を削除
        TeamList.RemoveAll(s => s.isdeath());
        EnemyList.RemoveAll(s => s.isdeath());
        ActorList.RemoveAll(s => s.isdeath());

        //Characterlistから死んだキャラを削除
        foreach(Character temp in CharacterList)
        {
            if (temp.isdeath())
            {
                //ゲームオブジェクトをディレイして削除
                Destroy(temp.gameObject, 0.5f);
            }
        }
        //ディレイ中にキャラリストから削除
        CharacterList.RemoveAll(s => s.isdeath());
        //プレイヤー死亡
        if (player == null || player.isdeath())
        {
            //gameover処理
            Debug.Log("You are dead.");
        }
        else
        {
            //レベルアップ
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
                    // 行動
                    if (ActorList[0].Action())
                    {
                        //特技使用
                        spact.SPActSelect(ActorList[0], condappGroup, ActorList[0].GetStatus().skill.id);
                    }
                    else
                    {
                        //通常攻撃
                        spact.SPAct_0(ActorList[0], condappGroup);
                    }
                    // 行動したものは削除
                    ActorList.RemoveAt(0);
                    //死亡者を削除
                    DeathCheck();

                    // 行動者更新
                    renewactor = false;
                }
                else
                {
                    //行動者がいなくなったら
                    if (ActorList.Count == 0)
                    {
                        //次のターンへ
                        ActorList.Clear();
                        
                        if (isATmodeChange())
                        {
                            switch (newatstate)
                            {
                                case ATMODE.CZ:
                                    //ボス出現
                                    AddCharacter(statusList[5], 4);
                                    //背景を変化
                                    bg.BGChange(2);
                                    break;
                                case ATMODE.CZ_PND:
                                    //背景を変化
                                    bg.BGChange(1);
                                    break;
                                default:
                                    //背景を変化
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
                //ステータス情報の更新
                stdisp.RenewST(player.GetLevel(), player.hp, player.GetStatus().hp);      
                break;
            default:
                break;
        }


    }
}
