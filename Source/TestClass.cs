using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITestClass
{
    void method_A();
    int method_B(int b);
}

public class TestClass : MonoBehaviour, ITestClass
{
    [SerializeField]int a = 5;
    [SerializeField]int bb = 2;

    public void method_A()
    {
        Debug.Log(a);
    }

    public int method_B(int b)
    {
        Debug.Log(b*bb);
        return b * bb;
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
