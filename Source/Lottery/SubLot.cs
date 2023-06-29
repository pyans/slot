using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create sublottery")]
public class SubLot : ScriptableObject
{
    //�������u�f�[�^
    [System.SerializableAttribute]
    public class OneLot
    {
        public string comment;  //�R�����g
        public List<int> sublotdata = new List<int>();  //������f�[�^
    }

    public string lotname;      //������
    public string comment;      //�R�����g
    [SerializeField]
    public List<OneLot> sublotdatas = new List<OneLot>();   //������f�[�^�̃��X�g
}
