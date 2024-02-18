using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelScript : MonoBehaviour
{
    public int reelnum;             //何番目のリールか
    public int reelpos = 0;         //リールの通過位置
    public int symbolsnum = 20;     //リールの図柄数
    float onestep;                  //1図柄分の角度
    //停止予定
    public float stoprad;
    public int stoppos;

    [SerializeField]
    float nowrotate = 0.0f;
    [SerializeField]
    public List<SymbolsData> reelsymbol = new List<SymbolsData>();
    public SymbolsDataBase symbolDataBase;
    public MachineControl master;

    public float rollspeed = 480;
    private bool rollstate = false;     //物理的に回転しているかのフラグ
    private bool stopmode = false;      //停止モード中(停止ボタンが押されてから回胴が止まるまで)かどうかのフラグ
    private bool standbystop = false;   //停止ボタンが有効かどうかのフラグ
    private bool pussingstop = false;   //停止ボタンが押されているかどうかのフラグ

    //消灯テスト
    public bool isturnoff = false;
    public ReelLight ReelLight;

    //enum RealID
    //{
    //    First = 1,
    //    Second = 2,
    //    Third = 3
    //}
    //// Start is called before the first frame update
    void Start()
    {
        //初期設定
        //図柄ごとの角度設定　18度
        //リール位置初期化 7
        onestep = 360 / symbolsnum;
        reelpos = (int)(nowrotate / onestep);
        //transform.Rotate(new Vector3(0, -(onestep / 2), 0));
    }

    //位置計算関数
    public int posculc(int num)
    {
        int temp = num;
        while (temp >= symbolsnum)
        {
            temp -= symbolsnum;
        }

        while (temp < 0)
        {
            temp += symbolsnum;
        }

        return temp;
    }

    //回胴回転中フラグ
    public void SetRollState(bool newstate)
    {
        rollstate = newstate;
    }

    //回胴停止ボタン押下中かどうか
    public void SetPushbutton(bool newstate)
    {
        pussingstop = newstate;
    }

    public bool GetPushbutton()
    {
        return pussingstop;
    }

    //回胴回転中状態の取得
    public bool GetRollState()
    {
        return rollstate;
    }

    //停止許可状態の取得
    public bool IsStopOK()
    {
        return standbystop;
    }

    public void PrepareForStop()
    {
        //　回胴停止準備
        standbystop = false;
        stopmode = true;
        // 停止位置決定
        int slide = master.spDecide.decideStop(master.stoporder, reelpos, master.activeCondAppGroup);
        Debug.Log("reelpos:" + reelpos + "  Slide:" + slide);
        //停止予定位置を設定
        stoppos = posculc(reelpos - slide);
        stoprad = stoppos * onestep;
    }

    //停止許可
    public void PermitStop()
    {
        standbystop = true;
    }

    // Update is called once per frame
    void Update()
    {

        //回転中ならリールを回転させる
        if (rollstate)
        {
            float rollrad = rollspeed * Time.deltaTime;
            transform.Rotate(new Vector3(0, -rollrad, 0));

            //リール位置の更新
            nowrotate -= rollrad;
            if (nowrotate < 0) nowrotate += 360.0f;
            reelpos = (int)(nowrotate / onestep);
        }

        //停止
        if (stopmode)
        {
            if(nowrotate - stoprad < 0 && nowrotate - stoprad >= (-1)*onestep)
            {
                //停止位置を修正
                float sub = stoprad - nowrotate;
                transform.Rotate(new Vector3(0, sub, 0));
                //リール位置の更新
                nowrotate += sub;
                if (nowrotate >= 360) nowrotate -= 360;
                reelpos = (int)(nowrotate / onestep);
                //停止フラグを設定
                stopmode = false;
                rollstate = false;
            }
            else if (stoprad == 0 && nowrotate > 342)
            {
                //停止位置が０の場合
                //停止位置を修正
                float sub = 360.0f - nowrotate;
                transform.Rotate(new Vector3(0, sub, 0));
                //リール位置の更新
                nowrotate = 0;
                reelpos = 0;
                //停止フラグを設定
                stopmode = false;
                rollstate = false;

            }

            if (!stopmode)
            {
                //共通処理
                //消灯
                if (isturnoff)
                {
                    ReelLight.turnOFF();
                    isturnoff = false;
                }
                //SE再生
                if (master.isTenpai() != 0)
                {
                    master.SEControll(4);
                }
                else
                {
                    master.SEControll(2);
                }
            }
        }
    }
}
