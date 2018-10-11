using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarBehaviour : MonoBehaviour
{
    // Bar

    private Slider _slider;

    public float percent {
        get {
            return _slider.value;
        }
        set {
            _slider.value = Mathf.Clamp (value, 0.0f, 1.0f);
        }
    }


    // Message

    private Text _text;

    public string message {
        get {
            return _text.text;
        }
        set {
            _text.text = value;
        }
    }

    void Start ()
    {
        _slider = gameObject.GetComponentInChildren<Slider> ();
        percent = 1.0f;
        _text = gameObject.GetComponentInChildren<Text> ();
        message = "∞";
    }
}
