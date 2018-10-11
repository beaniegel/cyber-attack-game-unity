using UnityEngine;


public class Helpers
{
    public static int LayerUI = LayerMask.NameToLayer ("UI");
    public static int LayerIgnoreRaycast = LayerMask.NameToLayer ("Ignore Raycast");

    public static void SetOffsets (RectTransform rt, float left, float top, float right, float bottom)
    {
        rt.offsetMin = new Vector2 (left, bottom);
        rt.offsetMax = new Vector2 (-right, -top);
    }

    public static void SetOffsets (RectTransform rt, int left, int top, int right, int bottom)
    {
        SetOffsets (rt, (float)left, (float)top, (float)right, (float)bottom);
    }

    public static void ResetOffsets (RectTransform rt)
    {
        SetOffsets (rt, 0, 0, 0, 0);
    }

    public static void SetAnchors (RectTransform rt, float xMin, float xMax, float yMin, float yMax)
    {
        rt.anchorMin = new Vector2 (xMin, yMin);
        rt.anchorMax = new Vector2 (xMax, yMax);
    }

    public static void ResetAnchors (RectTransform rt)
    {
        SetAnchors (rt, 0.0f, 1.0f, 0.0f, 1.0f);
    }
}
