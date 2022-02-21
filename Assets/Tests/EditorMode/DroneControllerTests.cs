using Drones;
using NUnit.Framework;
using UnityEngine;

public class DroneControllerTests
{
    private readonly GameObject _dronePrefab =
        UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

    private GameObject _drone;
    private DroneController _droneController;

    [SetUp]
    public void SetUp()
    {
        _drone = Object.Instantiate(_dronePrefab);
        _droneController = _drone.GetComponent<DroneController>();
        _droneController.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_drone);
    }

    // [Test]
    // public void TestConstruct()
    // {
    //     _droneController.Construct();
    //     Assert.NotNull(_droneController.Rb);
    //     Assert.NotNull(_droneController.Input);
    // }
    
    [Test]
    public void _0_Move_To_Set_The_Wanted_Position()
    {
        _droneController.MoveTo(Vector3.zero);
        Assert.AreEqual(Vector3.zero, _droneController.wantedPosition);
    }

    [Test]
    public void Move_To_A_Position_Sets_The_Wanted_Position()
    {
        var p = new Vector3(1f, 2f, 3f);
        _droneController.MoveTo(p);
        Assert.AreEqual(p, _droneController.wantedPosition);
    }
    
    [Test]
    public void Move_To_A_Negative_Position_Sets_The_Wanted_Position()
    {
        var p = new Vector3(-100f, -245f, -803f);
        _droneController.MoveTo(p);
        Assert.AreEqual(p, _droneController.wantedPosition);
    }
    
    [Test]
    public void Stop_Move_Sets_All_Inputs_To_Zeros()
    {
        var inputs = _drone.GetComponent<DroneInputs>();
        _droneController.StopMove();
        Assert.AreEqual(Vector2.zero, inputs.Cyclic);
        Assert.AreEqual(0f, inputs.Pedals);
        Assert.AreEqual(0f, inputs.Throttle);
    }

    // [Test]
    // public void _4_Go_Right()
    // {
    //     var drone = new Drone();
    //     var dronePosition = drone.transform.position;
    //     drone.goDown(10f);
    //     Assert.AreEqual(dronePosition - 10f, drone.transform.position);
    // }

    // [Test]
    // public void _5_Go_Left()
    // {
    //     var drone = new Drone();
    //     var dronePosition = drone.transform.position;
    //     drone.goLeft(10f);
    //     Assert.AreEqual(dronePosition - 10f, drone.transform.position);
    // }

    // [Test]
    // public void _6_Turn_On_The_Right()
    // {
    //     var drone = new Drone();
    //     var dronePosition = drone.yaw;
    //     drone.turnRight(90f);
    //     Assert.AreEqual(dronePosition + 90f, drone.yaw);
    // }

    // [Test]
    // public void _7_Turn_On_The_Left()
    // {
    //     var drone = new Drone();
    //     var dronePosition = drone.yaw;
    //     drone.turnLeft(90f);
    //     Assert.AreEqual(dronePosition - 90f, drone.yaw);
    // }
}