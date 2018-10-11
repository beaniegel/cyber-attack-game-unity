
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public int damage = 20;

    public GameObject impactEffect;

    public void Seek (Transform _target)
    {
        target = _target;
    }

    void Update ()
    {

        if (target == null) {
            Destroy (gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // hit the attacker
        if (dir.magnitude <= distanceThisFrame) {
            HitTarget ();
            return;
        }

        transform.Translate (dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget ()
    {
        ReduceHealth (target);
        Destroy (gameObject);
    }

    void ReduceHealth (Transform target)
    {
        AttackerBehaviour attacker = target.GetComponent<AttackerBehaviour> ();
        if (attacker != null) {
            attacker.TakeDamage (damage);
        }
        if (attacker.health <= 0) {
            KillEnemy (target);
        }
    }

    void KillEnemy (Transform target)
    {
        GameObject effect = Instantiate (impactEffect, transform.position, transform.rotation);
        Destroy (effect, 2f);
    }
}
