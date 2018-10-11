using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    private static StatusBarBehaviour statusBar = null;

    public static int maxBalance = 50;
    public static int balance = maxBalance;

    private static void UpdateStatus ()
    {
        if (statusBar != null) {
            statusBar.percent = (float)balance / (float)maxBalance;
            statusBar.message = "#" + balance + " update points";
        }
    }

    public static void ResetBalance ()
    {
        balance = maxBalance;
        Debug.Log ("Balance reset");
        UpdateStatus ();
    }


    public int value = 0;

    void Start ()
    {
        if (statusBar == null) {
            GameObject container = GameObject.Find ("CurrencyStatusBar");
            Debug.Assert (container != null, "Could not find CurrencyStatusBar GameObject");

            statusBar = container.GetComponentInChildren<StatusBarBehaviour> ();
            Debug.Assert (statusBar != null, "Could not find StatusBarBehaviour Component");
        }
        UpdateStatus ();
    }

    public bool DoTransaction ()
    {
        if (balance + value >= 0) {
            balance += value;
            NotifyTransactionSuccess (value);
            return true;
        } else {
            NotifyTransactionFailure ();
            return false;
        }
    }

    public bool UndoTransaction ()
    {
        if (balance - value >= 0) {
            balance -= value;
            NotifyTransactionSuccess (-value);
            return true;
        } else {
            NotifyTransactionFailure ();
            return false;
        }
    }

    private void NotifyTransactionSuccess (int change)
    {
        Debug.Log ("Currency " + value.ToString ("+#;-#;0"));
        UpdateStatus ();
    }

    private void NotifyTransactionFailure ()
    {
        Debug.Log ("Transaction failed");
        UpdateStatus ();
    }
}
