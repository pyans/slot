using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Game/Create CharacterStatus")]
public class Status : ScriptableObject
{
    //��b�X�e
    public string c_name;
    public int hp;
    public int attack;
    public int diffence;
    public int firstwait;
    //��ʐݒ�
    public enum WHO
    {
        PLAYERCHARA,
        NPC,
        MONSTER,
    }

    public enum BADSTATUS
    {
        DEATH = 1,
        STONE = 2
    }

    public WHO whoAreYou;
    //�G�����t���O
    public bool isenemy;

    public Action skill;//����s��
    public int skillLot;    //���Z�g�p��
    //�����A��Ԉُ�
    public int statusFlug;
    //�O��
    public Sprite sprite;
    //�o���l
    public int exp;

    public Status StatusCopy()
    {
        //WARNIG
        //MemberWiseClone�̓V�����[�R�s�[�̖͗l
        //���̂܂܂��ƃ��X�g���̎Q�ƌ^�͏㏑������
        return (Status)MemberwiseClone();
    }
}
