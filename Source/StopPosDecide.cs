using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StopPosDecide : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ReelScript> reel = new List<ReelScript>();

    void Start()
    {
        
    }

    public int Culc()
    {
        return 0;
    }

    public int ClucSride(int reelnum, int stoporder, StopBehavior stopbhv)
    {
        // reelnum: ��~�\�胊�[���ԍ��@0:���A1:���A2:�E
        // stoporder: ��~�� 1~3�Ԗ�
        int sride = 0;  //�X�x���R�}�� 0~4
        int max = 0;    //�ő�|�C���g�ۑ�
        List<SymbolsData> window = new List<SymbolsData>();

        for (int i = 0; i < 5; i++)
        {
            //�E�B���h�E�f�[�^�쐬
            for (int j=0; j < 3; j++)
            {
                int temppos = reel[reelnum].reelpos + i + j - 1;
                window.Add(reel[reelnum].reelsymbol[reel[reelnum].posculc(temppos)]);
            }
            //�|�C���g���Z
            int temp = 0;
            foreach (SymbolsData target in stopbhv.PrioritySymbol)
            {
                //window�̒�����������ݐ}���̐��𐔂���    
                temp += window.Select(x => x == target).Count()*16; //16:�D��x2
                //�D��L�����C���ւ̈������݂�����΃|�C���g��+1 �D��x3
                //�`�F���[���������݋֎~�}������������ł����-256 �D��x1
                if(reelnum == 0)
                {
                    //�`�F���[�`�F�b�N
                }
            }
            //�ő�l���X�V
            if (temp > max)
            {
                max = temp;
                sride = i;
            }
            //�E�B���h�E�f�[�^�N���A
            window.Clear();
        }

        return sride;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
