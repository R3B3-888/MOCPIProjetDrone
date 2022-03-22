using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Swarm;
using Cinemachine;

public class SwitchCameras : MonoBehaviour
{
    public Dictionary<string, CinemachineVirtualCamera> Cameras = new Dictionary<string, CinemachineVirtualCamera>();

    private string mainCamera;
    private string previousCamera;
    private int mainDrone;
    private int previousDrone;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera houseBeachCamera;
    public CinemachineVirtualCamera boatCamera;
    public SwarmManager deployedSwarm;
    private int droneCount;

    // Start is called before the first frame update
    void Start()
    {
        // add the cameras
        Cameras.Add("playerCamera", playerCamera);
        Cameras.Add("houseBeachCamera", houseBeachCamera);
        Cameras.Add("boatCamera", boatCamera);

        droneCount = deployedSwarm.drones.Count;
    }

    void Update(){
        if(deployedSwarm.drones.Count != droneCount)
        {
            for (int i = 0; i < deployedSwarm.drones.Count; i++)
            {
                Debug.Log(deployedSwarm.drones[i].droneInstance.name);
                Cameras.Add(deployedSwarm.drones[i].droneInstance.name, (CinemachineVirtualCamera) deployedSwarm.drones[i].droneInstance.GetComponentInChildren(typeof(CinemachineVirtualCamera)));
            }
            droneCount = deployedSwarm.drones.Count;
        }
    }

    public void showCamera(string name)
    {
        foreach(KeyValuePair<string, CinemachineVirtualCamera> cam in Cameras)
        {
            if(cam.Key != name)
            cam.Value.Priority = 9;
        }
        Cameras[name].Priority = 10;
    }
}
