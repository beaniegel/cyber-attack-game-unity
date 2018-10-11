using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{
    private UnityAction _baseDestroyedAction;
    private UnityAction _attackersDestroyedAction;

    void Awake ()
    {
        _attackersDestroyedAction = new UnityAction (Won);
        _baseDestroyedAction = new UnityAction (Lost);
    }

    void OnEnable ()
    {
        EventManager.StartListening ("AttackersDestroyed", _attackersDestroyedAction);
        EventManager.StartListening ("BaseDestroyed", _baseDestroyedAction);

    }

    void OnDisable ()
    {
        EventManager.StopListening ("AttackersDestroyed", _attackersDestroyedAction);
        EventManager.StopListening ("BaseDestroyed", _baseDestroyedAction);
    }

    void Won ()
    {
        MessageBoxBehaviour.Show ("Messages/Icons/EmojiWin", "Messages/Text/YouWon");
        EndGame ();
    }

    void Lost ()
    {
        MessageBoxBehaviour.Show ("Messages/Icons/EmojiLose", "Messages/Text/YouLost");
        EndGame ();
    }

    void EndGame ()
    {
        #if UNITY_WEBGL
        GatherFeedback ();
        #endif
    }

    #if UNITY_WEBGL
    [DllImport ("__Internal")]
    private static extern void GatherFeedback ();
    #endif

    public static void PauseGame ()
    {
        // Pause game and prevent unpause by disabling the button
        GameObject pauseButton = GameObject.Find ("Pause(Clone)");
        pauseButton.GetComponent<PauseBehaviour> ().Pause ();
        pauseButton.GetComponent<Button> ().interactable = false;

        // Prevent building any more towers
        GameObject buildPanel = GameObject.Find ("BuildPanel(Clone)");
        foreach (Button b in buildPanel.GetComponentsInChildren<Button> ()) {
            b.interactable = false;
        }
    }

    public static void ContinueGame ()
    {
        // Continue game and enable the button
        GameObject pauseButton = GameObject.Find ("Pause(Clone)");
        pauseButton.GetComponent<Button> ().interactable = true;

        // Make build panel working again
        GameObject buildPanel = GameObject.Find ("BuildPanel(Clone)");
        foreach (Button b in buildPanel.GetComponentsInChildren<Button> ()) {
            b.interactable = true;
        }
    }
}
