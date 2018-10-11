using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonObjectBuilder : MonoBehaviour
{

    // Static
    // ======

    private static CommonObjectBuilder commonObjectRoot;

    // Instance
    // ========

    // The starting position of the camera can be customised per-level
    // by changing cameraLocation and cameraSize in the Unity Editor
    public Vector2 cameraLocation;
    public int cameraSize;

    private Canvas canvas;

    void Awake ()
    {
        if (commonObjectRoot == null) {
            commonObjectRoot = gameObject.GetComponent<CommonObjectBuilder> ();
        } else {
            Debug.LogError ("More than one active CommonObjectRoot");
        }

        Canvas canvasPrefab = Resources.Load ("Containers/GuiCanvas", typeof(Canvas)) as Canvas;
        canvas = Instantiate (canvasPrefab, this.gameObject.transform);

        BuildCamera ();
        BuildLight ();
        BuildGui ();
        BuildGame ();
    }

    void BuildCamera ()
    {
        // Create a new camera as a child of this script's GameObject (and rename it)
        Camera cameraPrefab = Resources.Load ("GameCamera/GameCamera", typeof(Camera)) as Camera;
        Camera camera = Instantiate (cameraPrefab, this.gameObject.transform) as Camera;
        GameCameraController gcc = camera.GetComponent<GameCameraController> ();
        gcc.gameCamera = camera;

        // Translate it to the level-specific starting position
        camera.transform.Translate (cameraLocation.x, 0.0f, cameraLocation.y, Space.World);
        camera.orthographicSize = cameraSize;

        GameObject prefab = Resources.Load<GameObject> ("GameCamera/CameraPanel");
        GameObject cameraPanel = Instantiate (prefab, canvas.gameObject.transform);
        CameraPanelBehaviour cpb = cameraPanel.GetComponent<CameraPanelBehaviour> ();
        cpb.gameCamera = camera;
    }


    void BuildLight ()
    {
        // Create a new light as a child of this script's GameObject (and rename it)
        Light lightPrefab = Resources.Load ("GameLight/Directional light", typeof(Light)) as Light;
        Instantiate (lightPrefab, this.gameObject.transform);
    }


    // Defenders which may be placed in this level
    public List<GameObject> defenderPrefabs = new List<GameObject> ();

    void BuildGui ()
    {
        BuildSidePanel ();
        BuildInfoPanel ();
        BuildMessageBox ();
    }

    void BuildSidePanel ()
    {
        // SidePanel
        GameObject sidePanelPrefab = Resources.Load ("Containers/SidePanel", typeof(GameObject)) as GameObject;
        GameObject sidePanelInstance = Instantiate (sidePanelPrefab, canvas.transform);

        BuildBuildPanel (sidePanelInstance);
        BuildGameFlowPanel (sidePanelInstance);
    }

    void BuildBuildPanel (GameObject sidePanel)
    {
        // BuildPanel
        GameObject buildPanelPrefab = Resources.Load ("Containers/BuildPanel", typeof(GameObject)) as GameObject;
        GameObject buildPanelInstance = Instantiate (buildPanelPrefab, sidePanel.transform);

        // PlaceDefence buttons
        GameObject placeDefencePrefab = Resources.Load ("Buttons/PlaceDefence", typeof(GameObject)) as GameObject;
        foreach (GameObject defender in defenderPrefabs) {
            GameObject placeDefenceInstance = Instantiate (placeDefencePrefab, buildPanelInstance.transform);
            placeDefenceInstance.GetComponent<PlaceDefenceBehaviour> ().defencePrefab = defender;
        }
    }

    void BuildGameFlowPanel (GameObject sidePanel)
    {
        // GameFlowPanel
        GameObject gameFlowPanelPrefab = Resources.Load ("Containers/GameFlowPanel", typeof(GameObject)) as GameObject;
        GameObject gameFlowPanelInstance = Instantiate (gameFlowPanelPrefab, sidePanel.transform);

        // Pause, Restart and Quit buttons
        Instantiate (Resources.Load ("Buttons/Pause"), gameFlowPanelInstance.transform);
        Instantiate (Resources.Load ("Buttons/Restart"), gameFlowPanelInstance.transform);
        Instantiate (Resources.Load ("Buttons/Quit"), gameFlowPanelInstance.transform);
    }


    void BuildInfoPanel ()
    {
        // InfoPanel
        GameObject infoPanelPrefab = Resources.Load ("Containers/InfoPanel", typeof(GameObject)) as GameObject;
        GameObject infoPanelInstance = Instantiate (infoPanelPrefab, canvas.transform);

        BuildStatusBar (infoPanelInstance, "CurrencyStatusBar");
        BuildStatusBar (infoPanelInstance, "BaseHealthStatusBar");
    }

    void BuildStatusBar (GameObject infoPanel, string name)
    {
        GameObject statusBarPrefab = Resources.Load ("Widgets/StatusBar/StatusBar", typeof(GameObject)) as GameObject;
        GameObject statusBarInstance = Instantiate (statusBarPrefab, infoPanel.transform);
        statusBarInstance.name = name;

        GameObject customSliderPrefab = Resources.Load ("Widgets/StatusBar/CustomSlider", typeof(GameObject)) as GameObject;
        GameObject customSliderInstance = Instantiate (customSliderPrefab, statusBarInstance.transform);

        GameObject backgroundPrefab = Resources.Load ("Widgets/StatusBar/Background", typeof(GameObject)) as GameObject;
        Instantiate (backgroundPrefab, customSliderInstance.transform);

        GameObject fillAreaPrefab = Resources.Load ("Widgets/StatusBar/Fill Area", typeof(GameObject)) as GameObject;
        GameObject fillAreaInstance = Instantiate (fillAreaPrefab, customSliderInstance.transform);

        GameObject fillInstance = fillAreaInstance.transform.GetChild (0).transform.gameObject;
        Slider sliderInstance = customSliderInstance.GetComponent<Slider> ();
        sliderInstance.interactable = false;
        sliderInstance.fillRect = fillInstance.GetComponent<RectTransform> ();

        GameObject TextPrefab = Resources.Load ("Widgets/StatusBar/Text", typeof(GameObject)) as GameObject;
        Instantiate (TextPrefab, customSliderInstance.transform);
    }

    void BuildMessageBox ()
    {
        // MessageBox
        GameObject messageBoxPrefab = Resources.Load ("Widgets/MessageBox/MessageBox", typeof(GameObject)) as GameObject;
        Instantiate (messageBoxPrefab, canvas.transform);
    }

    void BuildGame ()
    {
        // GameMaster
        GameObject gameMasterPrefab = Resources.Load ("GameMaster/GameMaster", typeof(GameObject)) as GameObject;
        Instantiate (gameMasterPrefab, this.gameObject.transform);
    }
}
