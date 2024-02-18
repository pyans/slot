using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<ReelLight> reelLight = new List<ReelLight>();
    [SerializeField]
    List<LightPattern> lightPatterns;
    LightPattern currentlightPattern;
    int patternPointer = -1;

    public enum REELID
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public enum LINEID
    {
        CENTER,
        TOP,
        BOTTOM,
        CROSS_DOWN,
        CROSS_UP
    }

    private List<List<int>> lineposbuf = new List<List<int>>();
    private const int ReelNum = 3;
    private const int MaxWindow = 9;
    private float Timer = -1.0f;



    public void TurnOffbyReel(REELID reel)
    {
        //����
        int b = (int)reel;
        for (int i=b*ReelNum; i < (b + 1) * ReelNum; i++)
        {
            reelLight[i].turnOFF();
        }
    }

    public void TurnOff(List<int> posid)
    {
        //�󃊃X�g�̏ꍇ��return
        if (posid == null) return;
        //����
        for (int i = 0; i < MaxWindow; i++)
        {
            if (posid.Contains(i)){
                reelLight[i].turnOFF();
            }
        }
    }

    public void TurnOff(LINEID lineid)
    {
        //���C������
        TurnOff(lineposbuf[(int)lineid]);
    }

    public void LightReset()
    {
        //�S��
        foreach(ReelLight a in reelLight)
        {
            a.turnON();
        }

    }

    public void EffReset()
    {
        //������������
        LightReset();
        Timer = -1.0f;
        patternPointer = -1;
    }

    public void AllReverse()
    {
        //���]
        foreach (ReelLight a in reelLight)
        {
            //�A�N�e�B�u�ȃ��C�g������
            if (a.isActiveAndEnabled)
            {
                a.turnOFF();
            }
            else
            {
                a.turnON();
            }
        }
    }

    public void EffSet(LightPattern pattern)
    {
        //�����p�^�[���f�[�^���Z�b�g
        currentlightPattern = pattern;
        patternPointer = -1;
        EffUpdate();
    }

    public void EffSet(int id)
    {
        //�����p�^�[���f�[�^���Z�b�g(id�Q��)
        currentlightPattern = lightPatterns[id];
        patternPointer = -1;
        EffUpdate();
    }

    public void EffUpdate()
    {
        List<LightPattern.OneLightPattern> temp = currentlightPattern.GetPatterns();
        patternPointer += 1;
        if (temp.Count <= patternPointer)
        {
            //���s�[�g�̏ꍇ
            if (currentlightPattern.isrepeat)
            {
                patternPointer = 0;
            }
        }
        //�Ώۃf�[�^�����݂���Ȃ�
        if (temp.Count > patternPointer)
        {
            if (currentlightPattern.isclear)
            {
                //�S�_��(�_���󋵃��Z�b�g)
                LightReset();
            }
            //�f�[�^���烊�[�����������s
            TurnOff(temp[patternPointer].pattern);
            Timer = temp[patternPointer].sec;
        }
        
    }

    void Start()
    {
        lineposbuf.Add(new List<int>(){ 1,4,7});
        lineposbuf.Add(new List<int>(){ 0,3,6});
        lineposbuf.Add(new List<int>(){ 2,5,8});
        lineposbuf.Add(new List<int>(){ 0,4,8});
        lineposbuf.Add(new List<int>(){ 2,4,6});
    }

    // Update is called once per frame
    void Update()
    {
        //���o�p�^�C�}
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                //���̃p�^�[���ɍX�V
                EffUpdate();
            }
        }
    }
}
