using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightProgram : MonoBehaviour
{
    public Light myLight;
     
    void Start()
    {
        myLight.enabled = false;
    }
     
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            myLight.enabled = !myLight.enabled;
        }
    }
}
