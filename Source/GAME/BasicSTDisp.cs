using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicSTDisp : MonoBehaviour
{
    //�\���X�V�֐�
    public GameObject STpanel;
    public TextMeshProUGUI lvtext;
    public TextMeshProUGUI hptext;
    int maxhpbuff;
    public void TurnONOFF_STDisp(bool isactive)
    {
        STpanel.SetActive(isactive);                        //�\������
    }
    public void RenewHP(int newhp)
    {
        hptext.text = "HP:" + newhp + "/" + maxhpbuff;      //�ő�HP�X�V�Ȃ�
    }
    public void RenewHP(int newhp, int newmaxhp)
    {
        hptext.text = "HP:" + newhp + "/" + newmaxhp;       //�ő�HP�X�V����
        maxhpbuff = newmaxhp;
    }
    public void RenewLV(int newlv)
    {
        lvtext.text = "Lv:" + newlv;        //���x���X�V
    }
    public void RenewST(int newlv, int newhp, int newmaxhp)
    {
        RenewLV(newlv);                                     //�ꊇ�X�V
        RenewHP(newhp, newmaxhp);
    }
}
