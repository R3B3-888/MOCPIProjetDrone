using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCameras : MonoBehaviour
{
    public Dictionary<string, GameObject> Cameras = new Dictionary<string, GameObject>();

    public GameObject mainCamera;
    public GameObject previousCamera;
    public GameObject droneCamera;


    // Start is called before the first frame update
    void Start()
    {
        Cameras.Add("playerCamera", mainCamera);
        Cameras.Add("houseBeachCamera", previousCamera);
        Cameras.Add("DroneCamera", droneCamera);
    }

    // Update is called once per frame
    void Update()
    {
        showCamera();
    }

    public void showCamera()
    {
        previousCamera.SetActive(false);
        mainCamera.SetActive(true);
    }

    public void showPlayerCamera()
    {
        previousCamera = mainCamera;
        mainCamera = Cameras["playerCamera"];
    }

    public void showHouseBeachCamera()
    {
        previousCamera = mainCamera;
        mainCamera = Cameras["houseBeachCamera"];
    }

    public void showDroneCamera()
    {
        previousCamera = mainCamera;
        mainCamera = Cameras["DroneCamera"];
    }
}
