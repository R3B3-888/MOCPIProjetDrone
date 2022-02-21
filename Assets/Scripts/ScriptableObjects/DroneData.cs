using UnityEngine;

[CreateAssetMenu(fileName = "DroneData", menuName = "Simulation Objects/Drone Data", order = 0)]
public class DroneData : ScriptableObject
{
    public byte id = 0;
    public byte batteryLevelAtTheStart = 100;
    public GameObject dronePrefab;
    public bool backUpDrone = false;
}