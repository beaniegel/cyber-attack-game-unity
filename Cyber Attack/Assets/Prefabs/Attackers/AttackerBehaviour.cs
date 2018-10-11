
using UnityEngine;
using System.Collections.Generic;

public class AttackerBehaviour : MonoBehaviour
{
    private const float coincideDistance = 0.4f;

    public float speed = 10f;
    public int health = 100;

    private Currency currency;
    private Queue<Transform> waypoints;
    private GameObject target;

    void Start ()
    {
        currency = GetComponent<Currency> ();
        GameObject waypointContainer = GameObject.Find ("LevelObjects/WaypointContainer");
        Transform[] waypointArray = waypointContainer.GetComponentsInChildren<Transform> ();
        waypoints = new Queue<Transform> (waypointArray);
        waypoints.Dequeue (); // the first Transform is the WaypointContainer's so throw it away
        TargetNextWaypoint ();
        Debug.Assert (target != null);
    }

    void Update ()
    {
        Move ();
        if (CoincidesWith (target.transform)) {
            if (TargetIsAttackable ()) {
                AttackTarget ();
                return;
            }
            TargetNextWaypoint ();
        }
    }

    void Move ()
    {
        Vector3 dir = target.transform.position - transform.position;
        transform.Translate (dir.normalized * speed * Time.deltaTime, Space.World);
    }

    void TargetNextWaypoint ()
    {
        Transform nextTransform = waypoints.Dequeue ();
        if (nextTransform == null) {
            Die (); // no more waypoints: my life is complete
        } else {
            target = nextTransform.gameObject;
        }
    }

    bool CoincidesWith (Transform other)
    {
        // only interested in the x and z dimensions
        Vector2 myPosition = new Vector2 (transform.position.x, transform.position.z);
        Vector2 otherPosition = new Vector2 (other.position.x, other.position.z);
        float distance = Vector2.Distance (myPosition, otherPosition);

        // allow a small tolerance
        return distance <= coincideDistance;
    }

    bool TargetIsAttackable ()
    {
        return (target.GetComponent<BaseBehaviour> () != null);
    }

    void AttackTarget ()
    {
        BaseBehaviour attackableTarget = target.GetComponent<BaseBehaviour> ();
        attackableTarget.changeLife (-health);
        Die ();
    }

    public void TakeDamage (int damage)
    {
        health -= damage;
        if (health <= 0) {
            if (currency != null) {
                currency.DoTransaction ();
            }
            Die ();
        }
    }

    void Die ()
    {
        Destroy (gameObject);
    }
}
