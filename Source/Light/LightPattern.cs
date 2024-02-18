using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Effect/Create LightPattern")]

public class LightPattern : ScriptableObject
{
    //�������u�f�[�^
    [System.SerializableAttribute]
    public class OneLightPattern
    {
        public List<int> pattern = new List<int>();
        public float sec = 1.0f / 60.0f;
    }

    [SerializeField]
    List<OneLightPattern> onepattern = new List<OneLightPattern>();
    public string comment;
    public bool isrepeat = false;       //�p�^�[�����J��Ԃ���
    public bool isclear = false;        //�X�V���������������邩
    public float repeatsec = 0;

    public List<OneLightPattern> GetPatterns()
    {
        return onepattern;
    }
}
