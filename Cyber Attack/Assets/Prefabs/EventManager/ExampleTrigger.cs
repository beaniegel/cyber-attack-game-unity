using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleTrigger : MonoBehaviour
{
    void Update ()
    {
        if (Input.GetKeyDown ("t"))
            EventManager.TriggerEvent ("example");
    }
}
