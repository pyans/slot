using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicSTDisp : MonoBehaviour
{
    //表示更新関数
    public GameObject STpanel;
    public TextMeshProUGUI lvtext;
    public TextMeshProUGUI hptext;
    int maxhpbuff;
    public void TurnONOFF_STDisp(bool isactive)
    {
        STpanel.SetActive(isactive);                        //表示消去
    }
    public void RenewHP(int newhp)
    {
        hptext.text = "HP:" + newhp + "/" + maxhpbuff;      //最大HP更新なし
    }
    public void RenewHP(int newhp, int newmaxhp)
    {
        hptext.text = "HP:" + newhp + "/" + newmaxhp;       //最大HP更新あり
        maxhpbuff = newmaxhp;
    }
    public void RenewLV(int newlv)
    {
        lvtext.text = "Lv:" + newlv;        //レベル更新
    }
    public void RenewST(int newlv, int newhp, int newmaxhp)
    {
        RenewLV(newlv);                                     //一括更新
        RenewHP(newhp, newmaxhp);
    }
}
