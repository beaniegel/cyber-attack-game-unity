using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent (typeof(Camera))]
public class GameCameraController : MonoBehaviour
{
    [HideInInspector]
    public Camera gameCamera;

    [Range (1.0f, 300.0f)]
    public float scrollSpeed = 100.0f;

    void Update ()
    {
        // Calculate transform to move the camera
        float horizontal = Input.GetAxis ("Horizontal") * scrollSpeed;
        float vertical = Input.GetAxis ("Vertical") * scrollSpeed;
        Vector3 total = new Vector3 (horizontal, 0.0f, vertical);
        transform.Translate (total * Time.deltaTime, Space.World);
    }
}
