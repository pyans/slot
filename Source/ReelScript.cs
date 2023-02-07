using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelScript : MonoBehaviour
{
    public int reelnum;
    public int reelpos = 0;
    public int symbolsnum = 20;
    float onestep;
    public float stoprad;

    [SerializeField]
    float nowrotate = 0.0f;
    [SerializeField]
    public List<SymbolsData> reelsymbol = new List<SymbolsData>();
    public SymbolsDataBase symbolDataBase;

    public float rollspeed = 480;
    private bool rollstate = false;
    private bool stopmode = false;
    private bool standbystop = false;

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
        //���[���ʒu������ 8
        onestep = 360 / symbolsnum;
        reelpos = (int)(nowrotate / onestep);
        //transform.Rotate(new Vector3(0, -(onestep / 2), 0));
    }

    //�ʒu�v�Z�֐�
    public int posculc(int num)
    {
        while (num >= symbolsnum)
        {
            num -= symbolsnum;
        }

        while (num < 0)
        {
            num += symbolsnum;
        }

        return 0;
    }

    //�񓷉�]���t���O
    public void SetRollState(bool newstate)
    {
        rollstate = newstate;
    }

    public bool GetRollState()
    {
        return rollstate;
    }

    public void PrepareForStop()
    {
        standbystop = false;
        stopmode = true;
        stoprad = reelpos * onestep;
    }

    //��~����
    public void PermitStop()
    {
        standbystop = true;
    }

    public void PlayerAction()
    {
        if (standbystop)
        {
            //stop reel
            switch (reelnum)
            {
                case 1:
                    if (Input.GetKeyDown("z") || Input.GetButtonDown("Fire3"))
                    {
                        Debug.Log("1st Stop");
                        PrepareForStop();
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown("x") || Input.GetButtonDown("Fire2"))
                    {
                        Debug.Log("2nd Stop");
                        PrepareForStop();
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown("c") || Input.GetButtonDown("Fire1"))
                    {
                        Debug.Log("3rd Stop");
                        PrepareForStop();
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�{�^������
        PlayerAction();

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
            if(nowrotate < stoprad)
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
            }else if (stoprad == 0 && nowrotate > 342)
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
        }
    }
}
