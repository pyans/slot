using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/Create CharacterStatus")]
public class Status : ScriptableObject
{
    //基礎ステ
    public string c_name;
    public int hp;
    public int attack;
    public int defence;
    public int firstwait;
    //種別設定
    public enum WHO
    {
        PLAYERCHARA,
        NPC,
        MONSTER,
    }

    public enum BADSTATUS
    {
        DEATH = 1,
        STONE = 2,
        POISON = 4,
        PARALYSIS = 8,
        SLEEP = 16,
        STUN = 32,
        DARK = 64,
        CHARM = 128,
        CONFUSE = 256
    }

    public WHO whoAreYou;

    public MyAction skill;//特殊行動
    public int skillLot;    //特技使用率
    
    //グラ
    public Sprite sprite;
    //経験値
    public int exp;

    public Status StatusCopy()
    {
        //WARNIG
        //MemberWiseCloneはシャローコピーの模様
        //このままだとリスト等の参照型は上書きする
        return (Status)MemberwiseClone();
    }
}
