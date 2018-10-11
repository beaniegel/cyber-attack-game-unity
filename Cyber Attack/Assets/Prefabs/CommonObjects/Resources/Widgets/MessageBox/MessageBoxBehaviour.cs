using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MessageBoxBehaviour : MonoBehaviour
{
    public enum ButtonValue
    {
        OK,
        Yes,
        No,
        Cancel
    }

    public static void Show ()
    {
        // If there are no messages to show, don't do anything
        if (messageBoxInstance.queue.Count == 0) {
            return;
        }

        messageBoxInstance.ShowWrapper ();
    }


    // Show Convenience Functions
    // --------------------------
    public static void Show (String imagePath, String messagePath)
    {
        Push (imagePath, messagePath);
        Show ();
    }

    public static void Show (Texture image, String message)
    {
        Push (image, message);
        Show ();
    }

    public static void Show (Texture image, String message, UnityAction okAction)
    {
        Push (image, message, okAction);
        Show ();
    }

    public static void Show (Texture image, String message, Dictionary<ButtonValue, UnityAction> buttonActions)
    {
        Push (image, message, buttonActions);
        Show ();
    }

    private static void Show (MessageDescription desc)
    {
        Push (desc);
        Show ();
    }


    // Push Convenience Functions
    // --------------------------
    public static void Push (String imagePath, String messagePath)
    {
        Texture image = Resources.Load (imagePath) as Texture;
        TextAsset message = Resources.Load (messagePath) as TextAsset;
        Push (image, message.text);
    }

    public static void Push (Texture image, String message)
    {
        Push (image, message, new Dictionary<ButtonValue, UnityAction> ());
    }

    public static void Push (Texture image, String message, UnityAction okAction)
    {
        Dictionary<ButtonValue, UnityAction> buttonActions = new Dictionary<ButtonValue, UnityAction> ();
        buttonActions.Add (ButtonValue.OK, okAction);
        Push (image, message, buttonActions);
    }

    public static void Push (Texture image, String message, Dictionary<ButtonValue, UnityAction> buttonActions)
    {
        // Ensure we have an action dictionary
        if (buttonActions == null) {
            buttonActions = new Dictionary<ButtonValue, UnityAction> ();
        }

        // Ensure there is at least one button enabled
        if (buttonActions.Count == 0) {
            buttonActions.Add (ButtonValue.OK, null);
        }

        MessageDescription desc = new MessageDescription ();
        desc.image = image;
        desc.message = message;
        desc.buttonActions = buttonActions;
        Push (desc);
    }

    private static void Push (MessageDescription desc)
    {
        // Check we have a valid messsage
        Debug.Assert (messageBoxInstance != null, "MessageBox instance does not exist");
        Debug.Assert (desc.image != null);
        Debug.Assert (desc.message != null);
        Debug.Assert (desc.buttonActions != null);
        Debug.Assert (desc.buttonActions.Count > 0);

        // Add it to the queue
        messageBoxInstance.queue.Enqueue (desc);
    }

    private struct MessageDescription
    {
        public Texture image;
        public String message;
        public Dictionary<ButtonValue, UnityAction> buttonActions;
    }

    private static MessageBoxBehaviour messageBoxInstance;

    static Dictionary<ButtonValue, Color32[]> buttonColors = new Dictionary<ButtonValue, Color32[]> {
        // [0] -> Normal Color
        // [1] -> Highlighted Color
        { ButtonValue.OK, new Color32 [] { Colours.LightGreen, Colours.DarkGreen } },
        { ButtonValue.Yes, new Color32 [] { Colours.LightGreen, Colours.DarkGreen } },
        { ButtonValue.No, new Color32 [] { Colours.LightRed, Colours.DarkRed } },
        { ButtonValue.Cancel, new Color32 [] { Colours.LightOrange, Colours.DarkOrange } }
    };


    Queue<MessageDescription> queue = new Queue<MessageDescription> ();
    Dictionary<ButtonValue, Button> buttons;
    RawImage image;
    Text text;
    ButtonValue? clicked = null;


    // Construction
    // ============

    void Awake ()
    {
        if (messageBoxInstance != null) {
            return;
        }

        messageBoxInstance = this;

        BuildMessage ();
        BuildButtons ();

        gameObject.SetActive (false);
    }

    void BuildMessage ()
    {
        RectTransform rt;

        // Message panel
        GameObject messagePanel = new GameObject ();
        messagePanel.transform.SetParent (this.transform, false);
        messagePanel.name = "Message";
        messagePanel.layer = Helpers.LayerUI;

        rt = messagePanel.AddComponent<RectTransform> ();
        Helpers.ResetOffsets (rt);
        Helpers.SetAnchors (rt, 0.0f, 1.0f, 0.2f, 1.0f);

        // Image panel
        GameObject imagePanel = new GameObject ();
        imagePanel.transform.SetParent (messagePanel.transform, false);
        imagePanel.name = "ImagePanel";
        imagePanel.layer = Helpers.LayerUI;
        rt = imagePanel.AddComponent<RectTransform> ();
        Helpers.SetOffsets (rt, 10, 10, 5, 5);
        Helpers.SetAnchors (rt, 0.0f, 0.3f, 0.0f, 1.0f);

        // RawImage
        GameObject rawImagePrefab = Resources.Load ("Widgets/MessageBox/RawImage", typeof(GameObject)) as GameObject;
        GameObject rawImageInstance = Instantiate (rawImagePrefab, this.gameObject.transform);
        rawImageInstance.transform.SetParent (imagePanel.transform, false);
        image = rawImageInstance.GetComponent<RawImage> ();

        // Text panel
        GameObject textPanel = new GameObject ();
        textPanel.transform.SetParent (messagePanel.transform, false);
        textPanel.name = "TextPanel";
        textPanel.layer = Helpers.LayerUI;
        rt = textPanel.AddComponent<RectTransform> ();
        Helpers.SetOffsets (rt, 10, 10, 10, 5);
        Helpers.SetAnchors (rt, 0.3f, 1.0f, 0.0f, 1.0f);

        GameObject scrollViewInstance = BuildScrollView ();
        scrollViewInstance.transform.SetParent (textPanel.transform, false);
    }

    GameObject BuildScrollView ()
    {
        // Reusable prefab
        GameObject prefab;

        // Scroll View
        prefab = Resources.Load ("Widgets/MessageBox/Scroll View", typeof(GameObject)) as GameObject;
        GameObject scrollView = Instantiate (prefab);
        ScrollRect scrollRect = scrollView.GetComponent<ScrollRect> ();



        // Scrollbar Vertical
        prefab = Resources.Load ("Widgets/MessageBox/Scrollbar Vertical", typeof(GameObject)) as GameObject;
        GameObject scrollbarVertical = Instantiate (prefab, scrollView.transform);
        Scrollbar scrollbar = scrollbarVertical.GetComponent<Scrollbar> ();

        // Sliding Area
        prefab = Resources.Load ("Widgets/MessageBox/Sliding Area", typeof(GameObject)) as GameObject;
        GameObject slidingArea = Instantiate (prefab, scrollbarVertical.transform);

        // Handle
        prefab = Resources.Load ("Widgets/MessageBox/Handle", typeof(GameObject)) as GameObject;
        GameObject handle = Instantiate (prefab, slidingArea.transform);



        // Viewport
        prefab = Resources.Load ("Widgets/MessageBox/Viewport", typeof(GameObject)) as GameObject;
        GameObject viewport = Instantiate (prefab, scrollView.transform);

        // Panel
        prefab = Resources.Load ("Widgets/MessageBox/Panel", typeof(GameObject)) as GameObject;
        GameObject panel = Instantiate (prefab, viewport.transform);
        text = panel.GetComponentInChildren<Text> ();


        // Wire everything together
        scrollRect.content = text.GetComponent<RectTransform> ();
        scrollRect.viewport = viewport.GetComponent<RectTransform> ();
        scrollRect.verticalScrollbar = scrollbar;
        scrollbar.handleRect = handle.GetComponent<RectTransform> ();


        return scrollView;
    }

    void BuildButtons ()
    {
        // Create button panel

        GameObject buttonPanel = new GameObject ();
        buttonPanel.transform.SetParent (this.transform, false);
        buttonPanel.name = "Buttons";
        buttonPanel.layer = Helpers.LayerUI;

        RectTransform rt = buttonPanel.AddComponent<RectTransform> ();
        Helpers.ResetOffsets (rt);
        Helpers.SetAnchors (rt, 0.0f, 1.0f, 0.0f, 0.2f);

        HorizontalLayoutGroup hlg = buttonPanel.AddComponent <HorizontalLayoutGroup> ();
        hlg.padding.top = 5;
        hlg.padding.bottom = 5;
        hlg.padding.left = 5;
        hlg.padding.right = 5;
        hlg.spacing = 5.0f;
        hlg.childAlignment = TextAnchor.MiddleRight;
        hlg.childControlHeight = true;
        hlg.childControlWidth = true;
        hlg.childForceExpandHeight = true;
        hlg.childForceExpandWidth = true;


        // Create buttons

        GameObject buttonPrefab = Resources.Load ("Widgets/MessageBox/Button", typeof(GameObject)) as GameObject;

        buttons = new Dictionary<ButtonValue, Button> ();
        foreach (ButtonValue bv in Enum.GetValues(typeof(ButtonValue))) {
            // Create the button
            GameObject go = Instantiate (buttonPrefab, buttonPanel.transform);
            go.name += bv.ToString ();
            Button btn = go.GetComponent<Button> ();
            buttons.Add (bv, btn);

            // Set the colours
            ColorBlock cb = ColorBlock.defaultColorBlock;
            cb.normalColor = Colours.DarkGrey;
            cb.highlightedColor = buttonColors [bv] [0];
            cb.pressedColor = buttonColors [bv] [1];
            btn.colors = cb;

            // Set the label
            Text t = go.GetComponentInChildren<Text> ();
            t.text = bv.ToString ();
        }
    }


    // Actions
    // =======

    void ShowWrapper ()
    {
        GameBehaviour.PauseGame ();
        gameObject.SetActive (true);
        StartCoroutine (ShowAll ());
    }

    IEnumerator ShowAll ()
    {
        while (queue.Count > 0) {
            ShowNext ();
            while (clicked == null) {
                yield return null;
            }
        }
        gameObject.SetActive (false);
        GameBehaviour.ContinueGame ();
    }

    void ShowNext ()
    {
        // Get next message
        MessageDescription desc = queue.Dequeue ();

        // Configure buttons
        ResetButtons ();
        foreach (ButtonValue bv in desc.buttonActions.Keys) {
            EnableButton (bv, desc.buttonActions [bv]);
        }

        // Set content
        SetImage (desc.image);
        SetMessage (desc.message);
    }


    void ResetButtons ()
    {
        clicked = null;
        foreach (Button b in buttons.Values) {
            b.gameObject.SetActive (false);
            b.onClick.RemoveAllListeners ();
        }
    }

    void EnableButton (ButtonValue bv, UnityAction ua)
    {
        Button b = buttons [bv];
        b.onClick.AddListener (delegate {
            this.clicked = bv;
        });
        b.gameObject.SetActive (true);
    }


    // Configuration

    void SetImage (Texture image)
    {
        this.image.texture = image;
        AspectRatioFitter arf = this.image.gameObject.GetComponent<AspectRatioFitter> ();
        arf.aspectRatio = (float)image.width / (float)image.height;
    }

    void SetMessage (String message)
    {
        this.text.text = message;
    }
}
