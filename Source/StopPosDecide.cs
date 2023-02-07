using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StopPosDecide : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ReelScript> reel = new List<ReelScript>();

    void Start()
    {
        
    }

    public int Culc()
    {
        return 0;
    }

    public int ClucSride(int reelnum, int stoporder, StopBehavior stopbhv)
    {
        // reelnum: 停止予定リール番号　0:左、1:中、2:右
        // stoporder: 停止順 1~3番目
        int sride = 0;  //スベリコマ数 0~4
        int max = 0;    //最大ポイント保存
        List<SymbolsData> window = new List<SymbolsData>();

        for (int i = 0; i < 5; i++)
        {
            //ウィンドウデータ作成
            for (int j=0; j < 3; j++)
            {
                int temppos = reel[reelnum].reelpos + i + j - 1;
                window.Add(reel[reelnum].reelsymbol[reel[reelnum].posculc(temppos)]);
            }
            //ポイント加算
            int temp = 0;
            foreach (SymbolsData target in stopbhv.PrioritySymbol)
            {
                //windowの中から引き込み図柄の数を数える    
                temp += window.Select(x => x == target).Count()*16; //16:優先度2
                //優先有効ラインへの引き込みがあればポイントを+1 優先度3
                //チェリー等引き込み禁止図柄を引き込んでいると-256 優先度1
                if(reelnum == 0)
                {
                    //チェリーチェック
                }
            }
            //最大値を更新
            if (temp > max)
            {
                max = temp;
                sride = i;
            }
            //ウィンドウデータクリア
            window.Clear();
        }

        return sride;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
