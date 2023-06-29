using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEff : MonoBehaviour
{
    // Start is called before the first frame update
    public float Timer = 1.0f;
    float nowtimer = 200.0f;

    List<int> dispnum(int num)
    {
        var value = new List<int>();

        do
        {
            value.Add(num % 10);
            num /= 10;
        } while (num != 0);

        return value;
    }

    public void MyStart(int damage)
    {
        //タイマー設定
        nowtimer = Timer;
        //数字オブジェクト製作
        var numlist = new List<int>();
        numlist = dispnum(damage);
        int i = 0;

        foreach (int onenum in numlist)
        {
            GameObject obj = Resources.Load<GameObject>("GAME/UI_GPX/num_" + onenum);
            Instantiate(obj, this.transform.position + new Vector3(-0.54f * i, 0, 0), Quaternion.identity, gameObject.transform);
            i++;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //1秒で死ぬ
        nowtimer -= Time.deltaTime;
        if (nowtimer >= 0.6f) {
            this.transform.Translate(0, (-0.8f + nowtimer) / 20, 0);
        }
        else if (nowtimer <= 0)
        {
            Debug.Log("Damage End.");
            Destroy(this.gameObject);
        }
    }
}
