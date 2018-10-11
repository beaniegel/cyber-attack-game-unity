using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.google.com/document/d/1p2PJO59Z7DPsPsT7s1xzBedSAJEXoDTSEWR5YHl1qFA/edit#heading=h.dun79s8qqhs7

public class BaseBehaviour : MonoBehaviour
{
    [Range (1, 1000)]
    public int maxLife = 100;


    private bool baseDestroyed = false;

    private int _life;

    public int life {
        get {
            return _life;
        }
        set {
            _life = Mathf.Clamp (value, 0, maxLife);
            statusBar.percent = (float)life / (float)maxLife;
        }
    }

    public void changeLife (int amount)
    {
        life = life + amount;
        if (!baseDestroyed && life == 0) {
            baseDestroyed = true;
            EventManager.TriggerEvent ("BaseDestroyed");
        }
    }

    StatusBarBehaviour statusBar;

    void Start ()
    {
        GameObject container = GameObject.Find ("BaseHealthStatusBar");
        Debug.Assert (container != null, "Could not find BaseHealthStatusBar GameObject");

        statusBar = container.GetComponentInChildren<StatusBarBehaviour> ();
        Debug.Assert (statusBar != null, "Could not find StatusBarBehaviour Component");

        statusBar.message = "Memory";
        life = maxLife;
    }
}
