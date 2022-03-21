using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Swarm;
using Cinemachine;

public class SwitchCameras : MonoBehaviour
{
    public Dictionary<string, GameObject> Cameras = new Dictionary<string, GameObject>();

    private string mainCamera;
    private string previousCamera;
    private int mainDrone;
    private int previousDrone;

    public GameObject playerCamera;
    public GameObject houseBeachCamera;
    public GameObject boatCamera;
    public GameObject deployedSwarm;


    // Start is called before the first frame update
    void Start()
    {
        Cameras.Add("playerCamera", playerCamera);
        Cameras.Add("houseBeachCamera", houseBeachCamera);
        Cameras.Add("boatCamera", boatCamera);

        mainCamera = "playerCamera";
        previousCamera = "playerCamera";
        mainDrone = 0;
        previousDrone = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showCamera()
    {
        Cameras[previousCamera].SetActive(false);
        Cameras[mainCamera].SetActive(true);
    }

    public void showDroneCamera()
    {
        deployedSwarm.GetComponent<SwarmManager>().drones[previousDrone].droneInstance.GetComponentInChildren(typeof(CinemachineVirtualCamera)).gameObject.SetActive(false);
        deployedSwarm.GetComponent<SwarmManager>().drones[mainDrone].droneInstance.GetComponentInChildren(typeof(CinemachineVirtualCamera)).gameObject.SetActive(true);
    }

    public void showPlayerCamera()
    {
        if(mainCamera != "playerCamera")
        {
            previousCamera = mainCamera;
            mainCamera = "playerCamera";
        }
        showCamera();
    }

    public void showHouseBeachCamera()
    {
        if(mainCamera != "houseBeachCamera")
        {
            previousCamera = mainCamera;
            mainCamera = "houseBeachCamera";
        }
        showCamera();
    }

    public void showBoatCamera()
    {
        if(mainCamera != "boatCamera")
        {
            previousCamera = mainCamera;
            mainCamera = "boatCamera";
        }
        showCamera();
    }

    public void showDroneCameraNb(int index)
    {
        if(mainCamera != "deployedSwarm")
        {
            previousCamera = mainCamera;
            mainCamera = "deployedSwarm";
            previousDrone = mainDrone;
            mainDrone = index;
        }
        showDroneCamera();
    }
}
