using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControll : MonoBehaviour
{
    //îwåièW
    public List<Sprite> change;
    public void BGChange(int BGid)
    {
        //îwåiïœçX
        SpriteRenderer temp = this.gameObject.GetComponent<SpriteRenderer>();
        temp.sprite = change[BGid];
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
