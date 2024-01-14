using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class MovieController : MonoBehaviour
{
    //演出制御スクリプト
    //ナビ用
    public List<NABI> nabiobject;
    public List<VideoClip> nabiList = new List<VideoClip>();
    public Movie effmovie;
    public List<VideoClip> movieList = new List<VideoClip>();

    const int RPidx = 9;
    const int RPBLNidx = 12;
    const int BELidx_NML = 0;
    const int BELidx_R = 3;
    const int BELidx_UR = 6;
    const int RAREidx = 13;
    const int SRidx = 14;
    const int UR1idx = 15;
    const int UR2idx = 18;

    int CulcOrder(int order)
    {
        while (order / 10 != 0) order /= 10;
        return order - 1;
    }
    bool IsOrderTrue(int order, int answer)     //押し順の判定
    {
        if (order == 0) return true;            //レア役なら退場
        int tmp = answer;                       //比較用保存バッファ
        switch (order)
        {
            case 1:
            case 2:
            case 3:
                if (answer < 10) break;
                tmp /= 100;                     //第1停止
                break;
            case 12:
            case 13:
            case 21:
            case 23:
            case 31:
            case 32:
                if (answer < 10) return true;   //第2停止
                tmp /= 10;
                break;
            default:
                if (answer < 10) return true;   
                break;                          //第3停止
        }
        return order == tmp;
    }

    public void NabiSet(CondAppGroup active)        //レバON時ナビセット
    {
        int nabibase = 0;           //ナビ種別インデックス
        if (active.stoporder == 0) {
            if (active.isRAREtype())
            {
                nabibase = 13;
                if (active.isSR()) nabibase = 14;
                else if (active.isUR()) nabibase = 18;
                //レア役の場合
                foreach (NABI nabiobj in nabiobject)
                {
                    nabiobj.SetNabi(nabiList[nabibase]);
                    nabiobj.DispNabi();
                    if (nabibase >= UR1idx) nabibase++;
                }
            }
            return;       //押し順役、レア役でない場合スキップ
        }

        //押し順ベル　リプ
        int i = 2;
        if (active.isREP()) nabibase = RPidx;
        if(active.stoporder < 10)       //停止順指示が第一停止だけの場合
        {
            //不完全ナビ
            for (int tmp = active.stoporder; i >= 0; i--)
            {
                if(tmp - 1 == i)
                {
                    nabiobject[i].SetNabi(nabiList[nabibase]);
                }
                else
                {
                    nabiobject[i].SetNabi(nabiList[RPBLNidx]);
                }
                nabiobject[i].DispNabi();
            }
        }
        else
        {
            //完全押し順
            for (int tmp = active.stoporder; tmp != 0; tmp /= 10)
            {
                nabiobject[(tmp % 10) - 1].SetNabi(nabiList[nabibase + i]);
                nabiobject[i].DispNabi();
                i--;
            }
        }
    }

    public void NabiUpdate(int stoporder, CondAppGroup active)
    {
        if (active.stoporder == 0 && !active.isRAREtype()) return;       //押し順役、レア役でない場合スキップ
        
        if (IsOrderTrue(stoporder, active.stoporder) || active.isRAREtype())
        {
            nabiobject[(stoporder % 10) - 1].OffNabi();     //押し順が正解ならそのナビだけ消す
        }
        else
        {
            foreach (NABI nabi in nabiobject) nabi.OffNabi();       //押し順不正解ならナビ全消去
        }

    }
    //演出のリストから

    public void MoviePlay(int id)
    {
        effmovie.SetMovie(movieList[id]);
        effmovie.DispMovie();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
