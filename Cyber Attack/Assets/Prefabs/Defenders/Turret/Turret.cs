using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private float fireCountdown = 0f;

    //design the inspector
    [Header ("Turret Setting")]
    public float range = 20f;
    public GameObject rangeIndicator;
    public float fireRate = 1f;
    //variable, responsible for duration of defenders' life. Defender is disabled after 10 shoots.
    public float bulletStock = 10f;


    [Header ("Unity Setup Field")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    // Use this for initialization
    void Start ()
    {
        //do a method-updatetarget in a specific time-0s and repeat it in repeatrate-0.5s
        InvokeRepeating ("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget ()
    {
        //find a series of gameobjects with the same tog-enemytag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag (enemyTag);
        //the shortest distance is infinite faraway
        float shortestDistance = Mathf.Infinity;
        //initilizaion of neatsetEnemy reference
        GameObject nearestEnemy = null;
        //
        foreach (GameObject enemy in enemies) {
            //return the distance from point A to ponit B
            float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
            //if an enemy appears on the map
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        //if the enemy is on the defending range of defender
        if (shortestDistance <= range && nearestEnemy != null) {
            target = nearestEnemy.transform;
        }
    }


    // Update is called once per frame
    void Update ()
    {
        //if no enemy comes into the range of defender
        if (target == null || bulletStock <= 0f) {
            return;
        } 
        //if an enemey comes into the range of defender
        Vector3 dir = target.position - transform.position;
        //return a vector and a angle, which construct a rotation of the vector
        Quaternion lookRotation = Quaternion.LookRotation (dir);
        //make the rotation of defender smoothly
        Vector3 rotation = Quaternion.Lerp (partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        //the rotation of defender is round the y axis
        partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);

        //shoot twice in one second
        if (fireCountdown <= 0f & bulletStock >= 0f) {
            Shoot ();
            fireCountdown = 1f / fireRate;
            bulletStock -= 1f;
        }

        fireCountdown -= Time.deltaTime;
    }

    // shoot
    void Shoot ()
    {
        // instantiate bullets
        GameObject bulletObject = Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
        bulletObject.transform.SetParent (gameObject.transform);
        Bullet bulletComponent = bulletObject.GetComponent<Bullet> ();

        // bullet seek the target
        if (bulletComponent != null) {
            bulletComponent.Seek (target);
        }
    }

    //assistant for turret range
    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, range);
    }

    public bool RangeIndicatorVisible {
        get {
            return (rangeIndicator != null && rangeIndicator.activeSelf);
        }
        set {
            if (rangeIndicator != null) {
                rangeIndicator.SetActive (value);
            }
        }
    }
}
