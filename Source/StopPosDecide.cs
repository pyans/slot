using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StopPosDecide
{
    public List<ReelScript> reels;      //リール情報

    int CulcReelNum(int order)
    {
        while(order/10 != 0)
        {
            order /= 10;
        }
        return order - 1;
    }

    public Dictionary<int, int> tableIdDict = new Dictionary<int, int>()
    {
        {12, 0},
        {13, 1},
        {21, 2},
        {23, 3},
        {31, 4},
        {32, 5},
        {123, 0},
        {132, 1},
        {213, 2},
        {231, 3},
        {312, 4},
        {321, 5},
    };

    public int decideStop(int stoporder, int reelpos, CondAppGroup condAppG)
    {
        //停止制御
        int slide = 0;      //スベリコマ数
        int table_id;   //停止制御テーブル

        //MEMO:
        //今回はリール制御にテーブル制御を用いる
        //以下の条件で停止制御を設定してそれを参照する
        //第一停止のみ押し順で判定
        //  ・第２停止か第３停止か
        //　・作動条件装置
        //  ・停止リール
        
        switch (stoporder)
        {
            case 1:
            case 2:
            case 3:
                //第一停止
                //リールの現在位置からスベリテーブルを参照
                slide = condAppG.stopBh_1st[stoporder-1].slide[reelpos];
                break;
            case 12:
            case 13:
            case 21:
            case 23:
            case 31:
            case 32:
                //第二停止
                //第一停止によって参照が変わる
                table_id = tableIdDict[stoporder];
                slide = condAppG.stopBh_2nd[table_id].slide[reelpos];
                //特殊制御判定
                if (condAppG.specialStops.Count == 0) break;
                if (reels is null || reels.Count < 3)
                {
                    Debug.Log("Error:No reel info in StopPosDecide.cs");
                    break;
                }
                //特種制御条件を満たすか検索
                foreach(SpecialStop spstop in condAppG.specialStops)
                {
                    bool breakflag = false;                                     //ループ脱出フラグ
                    if (!spstop.stoporder.Contains(stoporder)) continue;        //停止順及び停止回胴が違えばスキップ
                    foreach (int pos in spstop.pos_1streel)
                    {
                        if (reels[CulcReelNum(stoporder)].stoppos == pos)    //スライド打ち等を考慮して停止予定位置で判定
                        {
                            //特種制御をおこなう　スベリコマ数を上書き
                            slide = spstop.stopBehavior.slide[reelpos];
                            breakflag = true;
                            break;
                        }
                    }
                    //条件合致なら退場
                    if (breakflag) break;
                }
                break;
            case 123:
            case 132:
            case 213:
            case 231:
            case 312:
            case 321:
                //第三停止
                //第二停止とさほど変わらない
                table_id = tableIdDict[stoporder];
                slide = condAppG.stopBh_3rd[table_id].slide[reelpos];
                //特殊制御判定
                if (condAppG.specialStops.Count == 0) break;
                if (reels is null || reels.Count < 3)
                {
                    Debug.Log("Error:No reel info in StopPosDecide.cs");
                    break;
                }
                //特種制御条件を満たすか検索
                foreach (SpecialStop spstop in condAppG.specialStops)
                {
                    bool breakflag = false;                                     //ループ脱出フラグ
                    if (!spstop.stoporder.Contains(stoporder)) continue;        //停止順及び停止回胴が違えばスキップ
                    foreach (int pos1 in spstop.pos_1streel)
                    {
                        if (spstop.pos_2ndreel.Count == 0)
                        {
                            //第一停止のみが条件の場合
                            if (reels[CulcReelNum(stoporder)].stoppos == pos1)    //スライド打ち等を考慮して停止予定位置で判定
                            {
                                //特種制御をおこなう　スベリコマ数を上書き
                                slide = spstop.stopBehavior.slide[reelpos];
                                breakflag = true;
                                break;
                            }
                        }
                        else
                        {
                            //両方が条件の場合
                            foreach (int pos2 in spstop.pos_2ndreel)
                            {
                                if (reels[CulcReelNum(stoporder)].stoppos == pos1 &&
                                    reels[CulcReelNum(stoporder % 100)].stoppos == pos2)    //スライド打ち等を考慮して停止予定位置で判定
                                {
                                    //特種制御をおこなう　スベリコマ数を上書き
                                    slide = spstop.stopBehavior.slide[reelpos];
                                    breakflag = true;
                                    break;
                                }
                            }
                        }
                    }

                    //条件合致なら退場
                    if (breakflag) break;
                }
                break;
            default:
                break;

        }
        return slide;
    }
}
