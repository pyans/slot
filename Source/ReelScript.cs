using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelScript : MonoBehaviour
{
    public int reelnum;
    public int reelpos = 0;
    public int symbolsnum = 20;
    float onestep;
    public float stoprad;

    [SerializeField]
    float nowrotate = 0.0f;
    [SerializeField]
    public List<SymbolsData> reelsymbol = new List<SymbolsData>();
    public SymbolsDataBase symbolDataBase;

    public float rollspeed = 480;
    private bool rollstate = false;
    private bool stopmode = false;
    private bool standbystop = false;

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
        //リール位置初期化 8
        onestep = 360 / symbolsnum;
        reelpos = (int)(nowrotate / onestep);
        //transform.Rotate(new Vector3(0, -(onestep / 2), 0));
    }

    //位置計算関数
    public int posculc(int num)
    {
        while (num >= symbolsnum)
        {
            num -= symbolsnum;
        }

        while (num < 0)
        {
            num += symbolsnum;
        }

        return 0;
    }

    //回胴回転中フラグ
    public void SetRollState(bool newstate)
    {
        rollstate = newstate;
    }

    public bool GetRollState()
    {
        return rollstate;
    }

    public void PrepareForStop()
    {
        standbystop = false;
        stopmode = true;
        stoprad = reelpos * onestep;
    }

    //停止許可
    public void PermitStop()
    {
        standbystop = true;
    }

    public void PlayerAction()
    {
        if (standbystop)
        {
            //stop reel
            switch (reelnum)
            {
                case 1:
                    if (Input.GetKeyDown("z") || Input.GetButtonDown("Fire3"))
                    {
                        Debug.Log("1st Stop");
                        PrepareForStop();
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown("x") || Input.GetButtonDown("Fire2"))
                    {
                        Debug.Log("2nd Stop");
                        PrepareForStop();
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown("c") || Input.GetButtonDown("Fire1"))
                    {
                        Debug.Log("3rd Stop");
                        PrepareForStop();
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ボタン操作
        PlayerAction();

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
            if(nowrotate < stoprad)
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
            }else if (stoprad == 0 && nowrotate > 342)
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
        }
    }
}
