using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitBehaviour : MonoBehaviour, IPointerClickHandler
{
    public bool QuitToMainMenu = true;

    public void OnPointerClick (PointerEventData pointerEventData)
    {
        if (QuitToMainMenu) {
            SceneManager.LoadScene ("Scenes/Menus/Main", LoadSceneMode.Single);
            return;
        }

        #if UNITY_EDITOR
        if (EditorApplication.isPlaying) {
            EditorApplication.isPlaying = false;
        }
        #elif UNITY_WEBGL
        // Only windows which were opened via JavaScript may be closed via Javascript
        // therefore in WebGL this function does nothing.
        #else
        // Will probably not handle iOS requirements due to
        // https://developer.apple.com/library/content/qa/qa1561/_index.html
        Application.Quit ();
        #endif
    }
}
