using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Inspectorに表示される
    [SerializeField]
    private List<EffectiveLineList> effectiveline = new List<EffectiveLineList>();
    public int windowsize;
    public int medal;
    public int bet;
    public int payout;
    public List<ReelScript> reel = new List<ReelScript>();

    enum GameState
    {
        SETUP,
        INSERT,
        START,
        GAME,
        RESULT,
        PAY,
    }

    void Lottery()
    {

    }

    GameState state;
    void Start()
    {
        state = GameState.INSERT;
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
                    Debug.Log("Bet 3");
                    if (medal >= 3)
                    {
                        medal -= 3;
                        bet = 3;
                        state = GameState.START;
                    }
                    
                }
                break;
            case GameState.START:
                //回胴回転開始
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Vertical"))
                {
                    foreach (ReelScript a_reel in reel){
                        a_reel.SetRollState(true);
                        a_reel.PermitStop();
                    }
                    state = GameState.GAME;
                }
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
                //有効ラインをチェック
                
                //払い出しへ
                state = GameState.PAY;
                break;
            case GameState.PAY:
                //払い出し
                medal += payout;
                //メダル投入待機へ
                state = GameState.INSERT;
                break;
        }
    }
}
