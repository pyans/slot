using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create CondAppGroup")]
public class CondAppGroup : ScriptableObject
{
    //�������u�Q�f�[�^

    public string CondAppGroupName;
    [SerializeField]
    public List<CondApp> CondApps = new List<CondApp>();
    //��~�����Ƃ̒�~����
    //�e���X�g�͒�~���������
    public List<StopBehavior> stopBh_1st = new List<StopBehavior>();  //����~�����A���A�E��3�ʂ�
    public List<StopBehavior> stopBh_2nd = new List<StopBehavior>();  //�������U�ʂ�
    public List<StopBehavior> stopBh_3rd = new List<StopBehavior>();  //�������U�ʂ�

}
