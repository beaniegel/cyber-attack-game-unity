using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system

    private Dictionary<string, UnityEvent> _eventDictionary;

    private static EventManager _eventManager = null;

    public static EventManager instance {
        get {
            if (_eventManager == null) {
                _eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;
                Assert.IsNotNull (_eventManager); // Remember to attach this script to a GameObject
                _eventManager.Init ();
            }
            return _eventManager;
        }
    }

    void Init ()
    {
        if (_eventDictionary == null)
            _eventDictionary = new Dictionary<string, UnityEvent> ();
    }

    public static void StartListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        EventManager i = instance;
        Dictionary<string, UnityEvent> d = instance._eventDictionary;
        if (d.TryGetValue (eventName, out thisEvent)) {
            thisEvent.AddListener (listener);
        } else {
            thisEvent = new UnityEvent ();
            thisEvent.AddListener (listener);
            d.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction listener)
    {
        if (_eventManager == null)
            return;
        UnityEvent thisEvent = null;
        if (instance._eventDictionary.TryGetValue (eventName, out thisEvent))
            thisEvent.RemoveListener (listener);
    }

    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance._eventDictionary.TryGetValue (eventName, out thisEvent))
            thisEvent.Invoke ();
    }
}
