using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCameras : MonoBehaviour
{
    public Dictionary<string, GameObject> Cameras = new Dictionary<string, GameObject>();

    private string mainCamera;
    private string previousCamera;

    public GameObject playerCamera;
    public GameObject houseBeachCamera;
    public GameObject droneCamera;


    // Start is called before the first frame update
    void Start()
    {
        Cameras.Add("playerCamera", playerCamera);
        Cameras.Add("houseBeachCamera", houseBeachCamera);
        Cameras.Add("DroneCamera", droneCamera);

        mainCamera = "playerCamera";
        previousCamera = "playerCamera";
    }

    // Update is called once per frame
    void Update()
    {
        showCamera();
    }

    public void showCamera()
    {
        Cameras[previousCamera].SetActive(false);
        Cameras[mainCamera].SetActive(true);
    }

    public void showPlayerCamera()
    {
        if(previousCamera != "playerCamera")
        {
            previousCamera = mainCamera;
            mainCamera = "playerCamera";
        }
    }

    public void showHouseBeachCamera()
    {
        if(previousCamera != "houseBeachCamera")
        {
            previousCamera = mainCamera;
            mainCamera = "houseBeachCamera";
        }
    }

    public void showDroneCamera()
    {
        if(previousCamera != "DroneCamera")
        {
            previousCamera = mainCamera;
            mainCamera = "DroneCamera";
        }
    }
}
