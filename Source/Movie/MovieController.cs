using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class MovieController : MonoBehaviour
{
    //���o����X�N���v�g
    //�i�r�p
    public List<NABI> nabiobject;
    public List<VideoClip> nabiList = new List<VideoClip>();
    public Movie effmovie;
    public List<VideoClip> movieList = new List<VideoClip>();

    const int RPidx = 9;
    const int RPBLNidx = 12;
    const int BELidx_NML = 0;
    const int BELidx_R = 3;
    const int BELidx_UR = 6;
    const int RAREidx = 13;
    const int SRidx = 14;
    const int UR1idx = 15;
    const int UR2idx = 18;

    int CulcOrder(int order)
    {
        while (order / 10 != 0) order /= 10;
        return order - 1;
    }
    bool IsOrderTrue(int order, int answer)     //�������̔���
    {
        if (order == 0) return true;            //���A���Ȃ�ޏ�
        int tmp = answer;                       //��r�p�ۑ��o�b�t�@
        switch (order)
        {
            case 1:
            case 2:
            case 3:
                if (answer < 10) break;
                tmp /= 100;                     //��1��~
                break;
            case 12:
            case 13:
            case 21:
            case 23:
            case 31:
            case 32:
                if (answer < 10) return true;   //��2��~
                tmp /= 10;
                break;
            default:
                if (answer < 10) return true;   
                break;                          //��3��~
        }
        return order == tmp;
    }

    public void NabiSet(CondAppGroup active)        //���oON���i�r�Z�b�g
    {
        int nabibase = 0;           //�i�r��ʃC���f�b�N�X
        if (active.stoporder == 0) {
            if (active.isRAREtype())
            {
                nabibase = 13;
                if (active.isSR()) nabibase = 14;
                else if (active.isUR()) nabibase = 18;
                //���A���̏ꍇ
                foreach (NABI nabiobj in nabiobject)
                {
                    nabiobj.SetNabi(nabiList[nabibase]);
                    nabiobj.DispNabi();
                    if (nabibase >= UR1idx) nabibase++;
                }
            }
            return;       //���������A���A���łȂ��ꍇ�X�L�b�v
        }

        //�������x���@���v
        int i = 2;
        if (active.isREP()) nabibase = RPidx;
        if(active.stoporder < 10)       //��~���w��������~�����̏ꍇ
        {
            //�s���S�i�r
            for (int tmp = active.stoporder; i >= 0; i--)
            {
                if(tmp - 1 == i)
                {
                    nabiobject[i].SetNabi(nabiList[nabibase]);
                }
                else
                {
                    nabiobject[i].SetNabi(nabiList[RPBLNidx]);
                }
                nabiobject[i].DispNabi();
            }
        }
        else
        {
            //���S������
            for (int tmp = active.stoporder; tmp != 0; tmp /= 10)
            {
                nabiobject[(tmp % 10) - 1].SetNabi(nabiList[nabibase + i]);
                nabiobject[i].DispNabi();
                i--;
            }
        }
    }

    public void NabiUpdate(int stoporder, CondAppGroup active)
    {
        if (active.stoporder == 0 && !active.isRAREtype()) return;       //���������A���A���łȂ��ꍇ�X�L�b�v
        
        if (IsOrderTrue(stoporder, active.stoporder) || active.isRAREtype())
        {
            nabiobject[(stoporder % 10) - 1].OffNabi();     //�������������Ȃ炻�̃i�r��������
        }
        else
        {
            foreach (NABI nabi in nabiobject) nabi.OffNabi();       //�������s�����Ȃ�i�r�S����
        }

    }
    //���o�̃��X�g����

    public void MoviePlay(int id)
    {
        effmovie.SetMovie(movieList[id]);
        effmovie.DispMovie();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
