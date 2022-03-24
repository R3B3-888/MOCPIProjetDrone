using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Swarm;
using Cinemachine;

public class SwitchCameras : MonoBehaviour
{
    #region Variables

    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private CinemachineVirtualCamera _houseBeachCamera;
    [SerializeField] private CinemachineVirtualCamera _boatCamera;
    [SerializeField] private SwarmManager _deployedSwarm;

    private readonly Dictionary<string, CinemachineVirtualCamera> _cameras =
        new Dictionary<string, CinemachineVirtualCamera>();

    private int _droneCount;
    private bool _isCamerasFilled;

    #endregion

    #region Main Methods

    private void Start()
    {
        _cameras.Add("playerCamera", _playerCamera);
        _cameras.Add("houseBeachCamera", _houseBeachCamera);
        _cameras.Add("boatCamera", _boatCamera);
    }

    private void Update()
    {
        LogCameras();
        
        // Will pass only once right after all drone have spawn
        if (_isCamerasFilled is false && _deployedSwarm.state == GameState.Standby)
        {
            FillCamerasWithDronesCamera();
            _isCamerasFilled = true;
        }

        if (_deployedSwarm.drones.Count >= _droneCount) return;

        // If a drone crashed
        _cameras.Remove(_deployedSwarm.dronesLost.Last().droneInstance.name);
        _droneCount = _deployedSwarm.drones.Count;
    }

    #endregion

    #region Custom Methods

    private void FillCamerasWithDronesCamera()
    {
        _droneCount = _deployedSwarm.drones.Count;
        foreach (var drone in _deployedSwarm.drones)
        {
            _cameras.Add(drone.droneInstance.name,
                drone.droneInstance.GetComponentInChildren(typeof(CinemachineVirtualCamera)) as
                    CinemachineVirtualCamera);
        }
    }

    public void ShowCamera(string keyName)
    {
        // if last drone crashed and we try access his camera
        if (!_cameras.ContainsKey(keyName)) return;

        foreach (var cam in _cameras)
            cam.Value.Priority = 9;

        _cameras[keyName].Priority = 10;
    }

    private void LogCameras(char sep = '|')
    {
        var camerasToString = new StringBuilder("_cameras : ");
        foreach (var cam in _cameras)
        {
            camerasToString.Append(cam.Key);
            camerasToString.Append(sep);
        }

        Debug.Log(camerasToString.ToString());
    }

    #endregion
}