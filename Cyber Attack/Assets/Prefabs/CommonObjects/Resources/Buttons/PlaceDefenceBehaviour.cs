using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaceDefenceBehaviour : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private static PlaceDefenceBehaviour activeBehaviour = null;

    public static void RegisterMouseOver (GameObject obj)
    {
        if (activeBehaviour != null) {
            activeBehaviour.defenceInstance.transform.position = obj.transform.position;
        }
    }

    public static GameObject CreateDefender ()
    {
        GameObject newTenant = null;
        if (activeBehaviour != null) {
            newTenant = Instantiate (activeBehaviour.defencePrefab);
            newTenant.transform.SetParent (activeBehaviour.defenderContainer.transform);

            Turret turret = newTenant.GetComponentInChildren<Turret> ();
            if (turret != null) {
                turret.RangeIndicatorVisible = false;
            }
        }
        return newTenant;
    }



    public GameObject defencePrefab;
    private GameObject defenceInstance;
    private GameObject defenderContainer;
    private bool previouslyClicked = false;

    void Start ()
    {
        Debug.Assert (defencePrefab != null,
            this.ToString () + ": defencePrefab must be set to the prefab this button should create");
        defenderContainer = GameObject.Find ("LevelObjects/DefenderContainer");
    }

    public void OnSelect (BaseEventData data)
    {
        Activate ();
    }

    public void OnDeselect (BaseEventData data)
    {
        Deactivate ();
    }

    bool Active ()
    {
        return (activeBehaviour == this);
    }

    void Activate ()
    {
        if (!Active ()) {
            if (activeBehaviour != null) {
                activeBehaviour.Deactivate ();
            }
            defenceInstance = Instantiate (defencePrefab);
            defenceInstance.layer = Helpers.LayerIgnoreRaycast;
            activeBehaviour = this;
        }

        if (!previouslyClicked) {
            previouslyClicked = true;
            MessageBoxBehaviour.Push ("Messages/Icons/Scanner", "Messages/Text/InstallVirusScanner");
            MessageBoxBehaviour.Push ("Messages/Icons/PlacedVirusScanner", "Messages/Text/Money");
            MessageBoxBehaviour.Show ();
        }
    }

    void Deactivate ()
    {
        if (Active ()) {
            if (defenceInstance != null) {
                Destroy (defenceInstance);
            }
            defenceInstance = null;
            activeBehaviour = null;
        }
    }
}
