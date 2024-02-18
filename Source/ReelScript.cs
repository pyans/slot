using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelScript : MonoBehaviour
{
    public int reelnum;             //���Ԗڂ̃��[����
    public int reelpos = 0;         //���[���̒ʉ߈ʒu
    public int symbolsnum = 20;     //���[���̐}����
    float onestep;                  //1�}�����̊p�x
    //��~�\��
    public float stoprad;
    public int stoppos;

    [SerializeField]
    float nowrotate = 0.0f;
    [SerializeField]
    public List<SymbolsData> reelsymbol = new List<SymbolsData>();
    public SymbolsDataBase symbolDataBase;
    public MachineControl master;

    public float rollspeed = 480;
    private bool rollstate = false;     //�����I�ɉ�]���Ă��邩�̃t���O
    private bool stopmode = false;      //��~���[�h��(��~�{�^����������Ă���񓷂��~�܂�܂�)���ǂ����̃t���O
    private bool standbystop = false;   //��~�{�^�����L�����ǂ����̃t���O
    private bool pussingstop = false;   //��~�{�^����������Ă��邩�ǂ����̃t���O

    //�����e�X�g
    public bool isturnoff = false;
    public ReelLight ReelLight;

    //enum RealID
    //{
    //    First = 1,
    //    Second = 2,
    //    Third = 3
    //}
    //// Start is called before the first frame update
    void Start()
    {
        //�����ݒ�
        //�}�����Ƃ̊p�x�ݒ�@18�x
        //���[���ʒu������ 7
        onestep = 360 / symbolsnum;
        reelpos = (int)(nowrotate / onestep);
        //transform.Rotate(new Vector3(0, -(onestep / 2), 0));
    }

    //�ʒu�v�Z�֐�
    public int posculc(int num)
    {
        int temp = num;
        while (temp >= symbolsnum)
        {
            temp -= symbolsnum;
        }

        while (temp < 0)
        {
            temp += symbolsnum;
        }

        return temp;
    }

    //�񓷉�]���t���O
    public void SetRollState(bool newstate)
    {
        rollstate = newstate;
    }

    //�񓷒�~�{�^�����������ǂ���
    public void SetPushbutton(bool newstate)
    {
        pussingstop = newstate;
    }

    public bool GetPushbutton()
    {
        return pussingstop;
    }

    //�񓷉�]����Ԃ̎擾
    public bool GetRollState()
    {
        return rollstate;
    }

    //��~����Ԃ̎擾
    public bool IsStopOK()
    {
        return standbystop;
    }

    public void PrepareForStop()
    {
        //�@�񓷒�~����
        standbystop = false;
        stopmode = true;
        // ��~�ʒu����
        int slide = master.spDecide.decideStop(master.stoporder, reelpos, master.activeCondAppGroup);
        Debug.Log("reelpos:" + reelpos + "  Slide:" + slide);
        //��~�\��ʒu��ݒ�
        stoppos = posculc(reelpos - slide);
        stoprad = stoppos * onestep;
    }

    //��~����
    public void PermitStop()
    {
        standbystop = true;
    }

    // Update is called once per frame
    void Update()
    {

        //��]���Ȃ烊�[������]������
        if (rollstate)
        {
            float rollrad = rollspeed * Time.deltaTime;
            transform.Rotate(new Vector3(0, -rollrad, 0));

            //���[���ʒu�̍X�V
            nowrotate -= rollrad;
            if (nowrotate < 0) nowrotate += 360.0f;
            reelpos = (int)(nowrotate / onestep);
        }

        //��~
        if (stopmode)
        {
            if(nowrotate - stoprad < 0 && nowrotate - stoprad >= (-1)*onestep)
            {
                //��~�ʒu���C��
                float sub = stoprad - nowrotate;
                transform.Rotate(new Vector3(0, sub, 0));
                //���[���ʒu�̍X�V
                nowrotate += sub;
                if (nowrotate >= 360) nowrotate -= 360;
                reelpos = (int)(nowrotate / onestep);
                //��~�t���O��ݒ�
                stopmode = false;
                rollstate = false;
            }
            else if (stoprad == 0 && nowrotate > 342)
            {
                //��~�ʒu���O�̏ꍇ
                //��~�ʒu���C��
                float sub = 360.0f - nowrotate;
                transform.Rotate(new Vector3(0, sub, 0));
                //���[���ʒu�̍X�V
                nowrotate = 0;
                reelpos = 0;
                //��~�t���O��ݒ�
                stopmode = false;
                rollstate = false;

            }

            if (!stopmode)
            {
                //���ʏ���
                //����
                if (isturnoff)
                {
                    ReelLight.turnOFF();
                    isturnoff = false;
                }
                //SE�Đ�
                if (master.isTenpai() != 0)
                {
                    master.SEControll(4);
                }
                else
                {
                    master.SEControll(2);
                }
            }
        }
    }
}
