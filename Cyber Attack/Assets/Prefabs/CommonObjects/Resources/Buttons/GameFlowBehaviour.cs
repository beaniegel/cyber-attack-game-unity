using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlowBehaviour : MonoBehaviour
{
    public void StartScene (string name)
    {
        SceneManager.LoadScene (name, LoadSceneMode.Single);
    }

    public void RestartScene ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
        Currency.ResetBalance ();
    }
}
