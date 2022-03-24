using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Swarm;
using Cinemachine;

public class SwitchCameras : MonoBehaviour
{
    private readonly Dictionary<string, CinemachineVirtualCamera> _cameras = new Dictionary<string, CinemachineVirtualCamera>();

    private string mainCamera;
    private string previousCamera;
    private int mainDrone;
    private int previousDrone;

    public CinemachineVirtualCamera _playerCamera;
    public CinemachineVirtualCamera _houseBeachCamera;
    public CinemachineVirtualCamera _boatCamera;
    public SwarmManager _deployedSwarm;
    private int _droneCount;
    private bool _isCamerasFilled = false;

    private void Start()
    {
        _cameras.Add("playerCamera", _playerCamera);
        _cameras.Add("houseBeachCamera", _houseBeachCamera);
        _cameras.Add("boatCamera", _boatCamera);
    }

    private void Update()
    {
        if (_isCamerasFilled is false && _deployedSwarm.state == GameState.Standby)
            FillCamerasWithDronesCamera();
        if (_deployedSwarm.drones.Count >= _droneCount) return;
        _cameras.Remove(_deployedSwarm.dronesLost.Last().droneInstance.name);
        _droneCount = _deployedSwarm.drones.Count;
    }

    private void FillCamerasWithDronesCamera()
    {
        _droneCount = _deployedSwarm.drones.Count;
        foreach (var drone in _deployedSwarm.drones)
        {
            _cameras.Add(drone.droneInstance.name,
                drone.droneInstance.GetComponentInChildren(typeof(CinemachineVirtualCamera)) as
                    CinemachineVirtualCamera);
        }

        _isCamerasFilled = true;
    }

    public void ShowCamera(string keyName)
    {
        // if last drone crashed and we try access his camera
        if (!_cameras.ContainsKey(keyName)) return;

        foreach (var cam in _cameras)
            if (cam.Key != keyName)
                cam.Value.Priority = 9;

        _cameras[keyName].Priority = 10;
    }
}