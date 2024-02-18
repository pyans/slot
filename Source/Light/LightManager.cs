using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<ReelLight> reelLight = new List<ReelLight>();
    [SerializeField]
    List<LightPattern> lightPatterns;
    LightPattern currentlightPattern;
    int patternPointer = -1;

    public enum REELID
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public enum LINEID
    {
        CENTER,
        TOP,
        BOTTOM,
        CROSS_DOWN,
        CROSS_UP
    }

    private List<List<int>> lineposbuf = new List<List<int>>();
    private const int ReelNum = 3;
    private const int MaxWindow = 9;
    private float Timer = -1.0f;



    public void TurnOffbyReel(REELID reel)
    {
        //消灯
        int b = (int)reel;
        for (int i=b*ReelNum; i < (b + 1) * ReelNum; i++)
        {
            reelLight[i].turnOFF();
        }
    }

    public void TurnOff(List<int> posid)
    {
        //空リストの場合即return
        if (posid == null) return;
        //消灯
        for (int i = 0; i < MaxWindow; i++)
        {
            if (posid.Contains(i)){
                reelLight[i].turnOFF();
            }
        }
    }

    public void TurnOff(LINEID lineid)
    {
        //ライン消灯
        TurnOff(lineposbuf[(int)lineid]);
    }

    public void LightReset()
    {
        //全灯
        foreach(ReelLight a in reelLight)
        {
            a.turnON();
        }

    }

    public void EffReset()
    {
        //消灯を初期化
        LightReset();
        Timer = -1.0f;
        patternPointer = -1;
    }

    public void AllReverse()
    {
        //反転
        foreach (ReelLight a in reelLight)
        {
            //アクティブなライトを消す
            if (a.isActiveAndEnabled)
            {
                a.turnOFF();
            }
            else
            {
                a.turnON();
            }
        }
    }

    public void EffSet(LightPattern pattern)
    {
        //消灯パターンデータをセット
        currentlightPattern = pattern;
        patternPointer = -1;
        EffUpdate();
    }

    public void EffSet(int id)
    {
        //消灯パターンデータをセット(id参照)
        currentlightPattern = lightPatterns[id];
        patternPointer = -1;
        EffUpdate();
    }

    public void EffUpdate()
    {
        List<LightPattern.OneLightPattern> temp = currentlightPattern.GetPatterns();
        patternPointer += 1;
        if (temp.Count <= patternPointer)
        {
            //リピートの場合
            if (currentlightPattern.isrepeat)
            {
                patternPointer = 0;
            }
        }
        //対象データが存在するなら
        if (temp.Count > patternPointer)
        {
            if (currentlightPattern.isclear)
            {
                //全点灯(点灯状況リセット)
                LightReset();
            }
            //データからリール消灯を実行
            TurnOff(temp[patternPointer].pattern);
            Timer = temp[patternPointer].sec;
        }
        
    }

    void Start()
    {
        lineposbuf.Add(new List<int>(){ 1,4,7});
        lineposbuf.Add(new List<int>(){ 0,3,6});
        lineposbuf.Add(new List<int>(){ 2,5,8});
        lineposbuf.Add(new List<int>(){ 0,4,8});
        lineposbuf.Add(new List<int>(){ 2,4,6});
    }

    // Update is called once per frame
    void Update()
    {
        //演出用タイマ
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                //次のパターンに更新
                EffUpdate();
            }
        }
    }
}
