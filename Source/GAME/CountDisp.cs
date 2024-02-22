using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDisp : MonoBehaviour
{
    // Start is called before the first frame update
    
    GameObject newobj;
    const int MAXNUM = 9;
    const int MINNUM = 0;
    public void UpdateCount(int onenum)
    {
        if (onenum < MINNUM) onenum = MINNUM;
        else if (onenum > MAXNUM) onenum = MAXNUM;
        GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/num_" + onenum);
        if (newobj != null) Destroy(newobj);
        newobj = Instantiate(obj, this.transform.position + new Vector3(-0.24f, -1.24f, 0), Quaternion.identity, gameObject.transform);
        newobj.GetComponent<SpriteRenderer>().color = new Color32(128, 128, 255, 255);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
