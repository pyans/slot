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
    public Character player;

    //キャラクターのステータスリスト
    public CharacterDataBase characterDataBase;
    List<Status> statusList;
    //行動リスト
    public SpecialActions spact;

    //当選条件装置
    CondAppGroup condappGroup;

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

    private int posdata_offset = 9;

    void AddCharacter(Status status, int posindx)
    {
        //キャラオブジェクト作成
        GameObject prefab = Resources.Load<GameObject>("GAME/newChara");
        GameObject newChara = Instantiate(prefab, this.gameObject.transform);
        Character newchara = newChara.GetComponent<Character>();
        //ステータス初期化
        newchara.SetStatus(status);

        //キャラリストに彫り込む
        CharacterList.Add(newchara);
        
        if (status.isenemy)
        {
            //敵の場合
            EnemyList.Add(newchara);
        }
        else
        {
            //味方の場合
            TeamList.Add(newchara);
        }

        newChara.transform.Translate(posdata[posindx]);
    }

    // Start is called before the first frame update
    void Start()
    {
        statusList = characterDataBase.GetStatusLists();
        //キャラクターは主人公→モンスターの順に先に動く
        AddCharacter(statusList[0], 1 + posdata_offset);
        AddCharacter(statusList[1], 1);
        AddCharacter(statusList[2], 2);
        AddCharacter(statusList[3], 0);
        AddCharacter(statusList[4], 4);
        AddCharacter(statusList[1], 7);

        //主人公の情報を保存
        player = CharacterList[0];

        //特殊行動データベースを作成
        spact = new SpecialActions(CharacterList, TeamList, EnemyList);
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
    }

    void DeathCheck()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (renewactor)
        {
            // 行動
            if (ActorList[0].Action())
            {
                //特技使用
                spact.SPActSelect(ActorList[0].GetStatus(), condappGroup, ActorList[0].GetStatus().skill.id);
            }
            else
            {
                //通常攻撃
                spact.SPAct_0(ActorList[0].GetStatus(), condappGroup);
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
                return;
            }
            renewactor = true;
        }


    }
}
