using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create SPStopBH")]
public class SpecialStop : ScriptableObject
{
    //�����~����
    public string Comment;             //���̒�~����̈Ӑ}
    public StopBehavior stopBehavior;  //��~����
    //����
    CondAppGroup ActiveCondApp; //�쓮�������u(�s�v�H)
    //CondApp FlagBonus;          //�쓮��A
    public List<int> stoporder;       //��~���[��(��~���ƍ��~�߂������[���̏��)
    public List<int> pos_1streel;     //�P�Ԗڃ��[����~�ʒu
    public List<int> pos_2ndreel;     //�Q�Ԗڃ��[����~�ʒu
}
