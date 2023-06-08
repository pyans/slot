using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StopPosDecide
{

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
        //作動した条件装置の組み合わせごとに停止制御を設定してそれを参照する
        
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
                break;
            default:
                break;

        }
        return slide;
    }
}
