using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBehaviour : MonoBehaviour
{
    private static Color32 lightAmber = new Color32 (0xFF, 0xAA, 0x33, 0xFF);
    private static Color32 darkAmber = new Color32 (0xCC, 0x77, 0x00, 0xFF);
    private static Color32 lightGreen = new Color32 (0x88, 0xDD, 0x44, 0xFF);
    private static Color32 darkGreen = new Color32 (0x44, 0x88, 0x11, 0xFF);

    private Button button;
    private Image image;
    private Sprite pauseIcon;
    private Sprite playIcon;

    private float pausedSpeed = 0.0f;
    private float playingSpeed = 1.0f;

    void Start ()
    {
        // Wire up the PauseBehaviour
        button = gameObject.GetComponent<Button> ();
        image = gameObject.GetComponent<Image> ();
        pauseIcon = Resources.Load ("Sprites/Icons_Pause", typeof(Sprite)) as Sprite;
        playIcon = Resources.Load ("Sprites/Icons_Play", typeof(Sprite)) as Sprite;
        button.onClick.AddListener (TogglePause);

        // And Pause to start with
        Pause ();
    }

    public bool Paused ()
    {
        return (Time.timeScale == pausedSpeed);
    }

    public void TogglePause ()
    {
        if (Paused ()) {
            Play ();
        } else {
            Pause ();
        }
    }

    public void Pause ()
    {
        Time.timeScale = pausedSpeed;

        // The icon indicates the next state, not the current state
        image.sprite = playIcon;
        ColorBlock newColors = button.colors;
        newColors.highlightedColor = lightGreen;
        newColors.pressedColor = darkGreen;
        button.colors = newColors;
    }

    public void Play ()
    {
        Time.timeScale = playingSpeed;

        // The icon indicates the next state, not the current state
        image.sprite = pauseIcon;
        ColorBlock newColors = button.colors;
        newColors.highlightedColor = lightAmber;
        newColors.pressedColor = darkAmber;
        button.colors = newColors;
    }
}
