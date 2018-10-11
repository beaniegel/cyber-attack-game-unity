using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlaceableBehaviour : MonoBehaviour
{
    private GameObject newTenant;
    private GameObject _tenant;

    public GameObject tenant {
        get {
            return _tenant;
        }
        set {
            if (_tenant == value) {
                return;
            }
            if (_tenant != null) {
                Destroy (_tenant);
            }
            _tenant = value;
            _tenant.transform.position = transform.position;
        }
    }

    void OnMouseEnter ()
    {
        PlaceDefenceBehaviour.RegisterMouseOver (gameObject);
    }


    // If the left mouse button was pressed down on this GameObject then we create
    // a new tenant, but deactivate it until we know the press was deliberate.
    void OnMouseDown ()
    {
        newTenant = PlaceDefenceBehaviour.CreateDefender ();
        if (newTenant != null) {
            newTenant.SetActive (false);
        }
    }

    // If the left mouse button was also released over this GameObject then we
    // assume the click was intentional and activate our tenant.
    void OnMouseUpAsButton ()
    {
        if (newTenant != null) {
            // We assume the build is free and therefore we can afford it
            bool affordable = true;

            Currency c = newTenant.GetComponent<Currency> ();
            if (c != null) {
                // If the item has a cost, we try to buy one
                affordable = c.DoTransaction ();
            }

            // Place it if we can afford it
            if (affordable) {
                newTenant.SetActive (true);
                tenant = newTenant;
            } else {
                Destroy (newTenant);
            }
            newTenant = null;
        }
    }

    // However, if the left mouse button was released somewhere else,
    // OnMouseUpAsButton won't have been called, therefore the tenant will still
    // be inactive, so we assume the original click was a mistake and clean it up.
    void OnMouseUp ()
    {
        if (newTenant != null) {
            Destroy (newTenant);
            newTenant = null;
        }
    }
}
