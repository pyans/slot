using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass2 : MonoBehaviour
{
    ITestClass testClass;
    public TestClass tmp;
    // Start is called before the first frame update
    void Start()
    {
        testClass = (ITestClass)tmp;
        testClass.method_B(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
