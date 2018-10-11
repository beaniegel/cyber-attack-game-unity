using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExampleResponder : MonoBehaviour
{
    private UnityAction _someListener;

    void Awake ()
    {
        _someListener = new UnityAction (SomeFunction);
    }

    void OnEnable ()
    {
        EventManager.StartListening ("example", _someListener);
    }

    void OnDisable ()
    {
        EventManager.StopListening ("example", _someListener);
    }

    void SomeFunction ()
    {
        Debug.Log ("SomeFunction was called!");
    }
}
