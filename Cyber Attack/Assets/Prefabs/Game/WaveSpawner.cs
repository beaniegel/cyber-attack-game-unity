using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    Queue<WaveBehaviour> waves;
    WaveBehaviour currentWave;
    // StatusBarBehaviour statusBar;

    private bool attackersDestroyed = false;

    void Start ()
    {
        // GameObject container = GameObject.Find ("WaveSpawnStatusBar");
        // Debug.Assert (container != null, "Could not find WaveSpawnStatusBar GameObject");

        // statusBar = container.GetComponentInChildren<StatusBarBehaviour> ();
        // Debug.Assert (statusBar != null, "Could not find StatusBarBehaviour Component");

        WaveBehaviour[] wavesArray = gameObject.GetComponentsInChildren<WaveBehaviour> ();
        waves = new Queue<WaveBehaviour> (wavesArray);

        MessageBoxBehaviour.Push ("Messages/Icons/AnonymousUser", "Messages/Text/FriendlyEmail");
        MessageBoxBehaviour.Push ("Messages/Icons/Virus", "Messages/Text/VirusDownloaded");
        MessageBoxBehaviour.Show ();
    }

    void Update ()
    {
        // Get the next wave
        if (currentWave == null && waves.Count > 0) {
            currentWave = waves.Dequeue ();

        }

        if (currentWave != null) {

            // Start the wave if we haven't already
            if (currentWave.phase == WaveBehaviour.Phase.NotStarted) {
                StartCoroutine (currentWave.Begin ());
            }

            // Update the StatusBar during the Countdown Phase
            // if (currentWave.phase == WaveBehaviour.Phase.Countdown) {
            //     statusBar.message = currentWave.quantity.ToString () + "×" + currentWave.attackerPrefab.name + " in " +
            //     currentWave.timeRemaining.ToString ("F1") + " seconds";
            //     statusBar.percent = currentWave.timeRemaining / currentWave.secondsBeforeStarting;
            // } else {
            //     statusBar.message = "";
            //     statusBar.percent = 0.0f;
            // }

            // Clear the currentWave if it has finished
            if (currentWave.phase == WaveBehaviour.Phase.Finished) {
                currentWave = null;
            }
        }

        if (!attackersDestroyed && waves.Count == 0 && currentWave == null && attackersInPlay == 0) {
            // If we have visited every wave,
            // there is no wave currently spawning,
            // and there are no attackers left on the board,
            // we must have won.
            attackersDestroyed = true;
            EventManager.TriggerEvent ("AttackersDestroyed");
        }
    }

    uint attackersInPlay {
        get {
            return (uint)gameObject.GetComponentsInChildren<AttackerBehaviour> ().Length;
        }
    }
}
