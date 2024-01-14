using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CondAppGroup")]
public class CondAppGroup : ScriptableObject
{
    //�������u�Q�̃^�C�v
    public enum CondAppGroupTag
    {
        BELL = 1,
        REP = 2,
        CHERRY = 4,
        WTML = 8,
        RARE = 16,
        SR = 32,
        UR = 64,
        BONUS = 128
    };
    //������
    public int stoporder = 0;
    //�������u�Q�f�[�^
    public string CondAppGroupName;
    public int typeofCondApp;                                         //�������u�Q�̖���(0�c�n�Y���A1�c�����A2�c���A�A3�c�m��)
    [SerializeField]
    public List<CondApp> CondApps = new List<CondApp>();
    //��~�����Ƃ̒�~����
    //�e���X�g�͒�~���������
    public List<StopBehavior> stopBh_1st = new List<StopBehavior>();  //����~�����A���A�E��3�ʂ�
    public List<StopBehavior> stopBh_2nd = new List<StopBehavior>();  //�������U�ʂ�
    public List<StopBehavior> stopBh_3rd = new List<StopBehavior>();  //�������U�ʂ�
    //�����~����
    //����̃��[��������̈ʒu�̏ꍇ�ɍ쓮
    public List<SpecialStop> specialStops = new List<SpecialStop>();

    public bool isBELL()
    {
        return (typeofCondApp & (int)CondAppGroupTag.BELL) != 0;
    }

    public bool isREP()
    {
        return (typeofCondApp & (int)CondAppGroupTag.REP) != 0;
    }

    public bool isWTML()
    {
        return (typeofCondApp & (int)CondAppGroupTag.WTML) != 0;
    }

    public bool isCHERRY()
    {
        return (typeofCondApp & (int)CondAppGroupTag.CHERRY) != 0;
    }

    public bool isRARE()
    {
        return (typeofCondApp & (int)CondAppGroupTag.RARE) != 0;
    }

    public bool isSR()
    {
        return (typeofCondApp & (int)CondAppGroupTag.SR) != 0;
    }

    public bool isUR()
    {
        return (typeofCondApp & (int)CondAppGroupTag.UR) != 0;
    }

    public bool isBONUS()
    {
        return (typeofCondApp & (int)CondAppGroupTag.BONUS) != 0;
    }

    public bool isRAREtype()
    {
        return this.isRARE() || this.isSR() || this.isUR() || this.isBONUS();
    }
}
