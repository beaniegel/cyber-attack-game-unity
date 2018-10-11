using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehaviour : MonoBehaviour
{
    public enum Phase
    {
        NotStarted,
        Countdown,
        Spawning,
        Finished
    }

    [Header ("Wave Configuration")]

    [Range (1.0f, 30.0f)]
    public float secondsBeforeStarting = 2.0f;

    public GameObject spawnPoint;
    [Range (0.1f, 10.0f)]
    public float interval = 1.0f;

    public GameObject attackerPrefab;
    [Range (1, 30)]
    public uint quantity = 1;


    // Private Parts
    private uint _attackersRemaining;

    public uint attackersRemaining {
        get {
            return _attackersRemaining;
        }
    }

    private float _timeRemaining;

    public float timeRemaining {
        get {
            return _timeRemaining;
        }
    }

    Phase _phase;

    public Phase phase {
        get {
            return _phase;
        }
    }


    // Methods

    void Start ()
    {
        _phase = Phase.NotStarted;
    }

    public IEnumerator Begin ()
    {
        yield return StartCoroutine (DoCountdown ());
        yield return StartCoroutine (DoSpawning ());
        _phase = Phase.Finished;
    }

    IEnumerator DoCountdown ()
    {
        _phase = Phase.Countdown;
        _timeRemaining = secondsBeforeStarting;
        while (_timeRemaining >= 0.0f) {
            _timeRemaining -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DoSpawning ()
    {
        _phase = Phase.Spawning;
        _attackersRemaining = quantity;
        while (_attackersRemaining > 0) {
            yield return StartCoroutine (SpawnAttacker ());
        }
    }

    IEnumerator SpawnAttacker ()
    {
        // Spawn attacker
        GameObject newAttacker = Instantiate (attackerPrefab,
                                     spawnPoint.transform.position,
                                     spawnPoint.transform.rotation);
        newAttacker.transform.SetParent (gameObject.transform);
        --_attackersRemaining;

        _timeRemaining = interval;
        while (_timeRemaining >= 0.0f) {
            _timeRemaining -= Time.deltaTime;
            yield return null;
        }
    }
}
