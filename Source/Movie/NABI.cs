using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class NABI : MonoBehaviour
{
    public VideoPlayer video;
    

    public void DispNabi()
    {
        //�i�r��\��
        Debug.Assert(video != null, "Can't find nabi movie.");  //����f�[�^���Ȃ��Ȃ�G���[
        this.gameObject.SetActive(true);                        //�i�r�\��
    }

    public void OffNabi()
    {
        //�i�r������
        this.gameObject.SetActive(false);                        //�i�r��\��
    }

    public void SetNabi(VideoClip data)
    {
        //�i�r��ނ��Z�b�g
        video.clip = data;
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
