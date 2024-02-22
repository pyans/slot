using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelLight : MonoBehaviour
{
    //ƒŠ[ƒ‹Á“”‰‰o
    public void turnON()
    {
        this.gameObject.SetActive(false);
    }
    public void turnOFF()
    {
        this.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
