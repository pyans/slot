using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create SPStopBH")]
public class SpecialStop : ScriptableObject
{
    //“Áê’â~§Œä
    public string Comment;             //‚±‚Ì’â~§Œä‚ÌˆÓ}
    public StopBehavior stopBehavior;  //’â~§Œä
    //ğŒ
    CondAppGroup ActiveCondApp; //ì“®ğŒ‘•’u(•s—vH)
    //CondApp FlagBonus;          //ì“®–ğ˜A
    public List<int> stoporder;       //’â~ƒŠ[ƒ‹(’â~‡‚Æ¡~‚ß‚½‚¢ƒŠ[ƒ‹‚Ìî•ñ)
    public List<int> pos_1streel;     //‚P”Ô–ÚƒŠ[ƒ‹’â~ˆÊ’u
    public List<int> pos_2ndreel;     //‚Q”Ô–ÚƒŠ[ƒ‹’â~ˆÊ’u
}
