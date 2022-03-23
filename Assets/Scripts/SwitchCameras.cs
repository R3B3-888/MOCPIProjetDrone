using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Swarm;
using Cinemachine;
using Drones;

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

    void Update()
    {
        if (deployedSwarm.drones.Count != droneCount)
        {
            foreach (var key in Cameras.Keys.Where(key => key.StartsWith("Drone")))
                Cameras.Remove(key);

            foreach (var drone in deployedSwarm.drones)
            {
                Cameras.Add(drone.droneInstance.name,
                    drone.droneInstance.GetComponentInChildren(typeof(CinemachineVirtualCamera)) as
                        CinemachineVirtualCamera);
            }

            droneCount = deployedSwarm.drones.Count;
        }
    }

    public void ShowCamera(string keyName)
    {
        // if last drone crashed and we try access his camera
        if (!Cameras.ContainsKey(keyName)) return; 
        
        foreach (var cam in Cameras)
        {
            if (cam.Key != keyName)
                cam.Value.Priority = 9;
        }

        Cameras[keyName].Priority = 10;
    }
}