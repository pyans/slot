using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class Movie : MonoBehaviour
{
    public VideoPlayer video;

    public void DispMovie()
    {
        //ナビを表示
        Debug.Assert(video != null, "Can't find nabi movie.");  //動画データがないならエラー
        this.gameObject.SetActive(true);                        //ナビ表示
    }

    public void OffMovie()
    {
        //ナビを消す
        this.gameObject.SetActive(false);                        //ナビ非表示
    }

    public void SetMovie(VideoClip data)
    {
        //ナビ種類をセット
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

