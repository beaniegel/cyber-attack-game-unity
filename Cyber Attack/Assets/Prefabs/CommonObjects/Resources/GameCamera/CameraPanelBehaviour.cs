using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraPanelBehaviour : MonoBehaviour, IScrollHandler
{
    [Range (1.0f, 300.0f)]
    public float zoomSpeed = 30.0f;

    [HideInInspector]
    public Camera gameCamera;

    private float _maxZoom = 10.0f;

    public void OnScroll (PointerEventData eventData)
    {
        // Calculate zoom to alter the field of view
        float zoom = (eventData.scrollDelta.y / 10.0f) * zoomSpeed * -1.0f;
        float clampedZoom = Mathf.Max (_maxZoom, gameCamera.orthographicSize + zoom);
        gameCamera.orthographicSize = clampedZoom;
    }
}
