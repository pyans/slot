using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Inspector�ɕ\�������
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
                //���_������
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
                //�񓷉�]�J�n
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
                //�L�����C�����`�F�b�N
                
                //�����o����
                state = GameState.PAY;
                break;
            case GameState.PAY:
                //�����o��
                medal += payout;
                //���_�������ҋ@��
                state = GameState.INSERT;
                break;
        }
    }
}
