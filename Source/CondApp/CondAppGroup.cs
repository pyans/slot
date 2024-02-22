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
        BONUS = 128,
        ORDER = 256
    };
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
}
