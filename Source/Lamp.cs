using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer { get; set; }

    public void Peka()
    {
        SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }

    public void DisPeka()
    {
        SpriteRenderer.color = new Color(0.2f, 0.2f, 0.2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
