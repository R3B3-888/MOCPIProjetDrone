using Drones;
using NUnit.Framework;
using UnityEngine;
using static NUnit.Framework.Assert;

public class DroneControllerTests
{
    #region Variables

    private readonly GameObject _dronePrefab =
        UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

    private GameObject _drone;
    private static DroneController _droneController;
    private static DroneInputs _inputs;

    #endregion

    #region Setup Teardown

    [SetUp]
    public void SetUp()
    {
        _drone = Object.Instantiate(_dronePrefab);
        _droneController = _drone.GetComponent<DroneController>();
        _droneController.Awake();
        _inputs = _drone.GetComponent<DroneInputs>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_drone);
    }

    #endregion

    #region Move To Method

    [Test]
    public void _0_Move_To_Set_The_Wanted_Position()
    {
        _droneController.MoveTo(Vector3.zero);
        AreEqual(Vector3.zero, _droneController.wantedPosition);
    }

    [Test]
    public void Move_To_A_Position_Sets_The_Wanted_Position()
    {
        var p = new Vector3(1f, 2f, 3f);
        _droneController.MoveTo(p);
        AreEqual(p, _droneController.wantedPosition);
    }

    [Test]
    public void Move_To_A_Negative_Position_Sets_The_Wanted_Position()
    {
        var p = new Vector3(-100f, -245f, -803f);
        _droneController.MoveTo(p);
        AreEqual(p, _droneController.wantedPosition);
    }

    #endregion

    #region Turn To Method

    [Test]
    public void _0_Turn_To_Set_The_Wanted_Rotation()
    {
        _droneController.TurnTo(0);
        AreEqual(0, _droneController.wantedRotation);
    }

    [Test]
    public void Turn_To_An_Angle_Sets_The_Wanted_Rotation()
    {
        _droneController.TurnTo(45f);
        AreEqual(45f, _droneController.wantedRotation);
    }

    [Test]
    public void Turn_To_An_Angle_Above_360_Sets_The_Wanted_Rotation()
    {
        _droneController.TurnTo(380f);
        AreEqual(20f, _droneController.wantedRotation);
    }

    [Test]
    public void Turn_To_A_Negative_Rotation_Sets_The_Wanted_Rotation()
    {
        _droneController.TurnTo(-150f);
        AreEqual(-150f, _droneController.wantedRotation);
    }

    [Test]
    public void Turn_To_An_Angle_Under_Neg_360_Sets_The_Wanted_Rotation()
    {
        _droneController.TurnTo(-395f);
        AreEqual(-35f, _droneController.wantedRotation);
    }

    #endregion


    #region IsInRadius Method

    [Test]
    public void Is_In_Radius_of_Wanted_Position_0()
    {
        IsTrue(_droneController.IsInRadiusOfWantedPosition());
    }

    [Test]
    public void Is_In_Radius_of_Wanted_Position_In_Radius()
    {
        _droneController.MoveTo(new Vector3(0.2f, 0, 0.1f));
        IsTrue(_droneController.IsInRadiusOfWantedPosition(.5f));
    }

    [Test]
    public void Is_In_Radius_of_Wanted_Position_Not_In_Radius()
    {
        _droneController.MoveTo(new Vector3(0.2f, 0, 0.1f));
        IsFalse(_droneController.IsInRadiusOfWantedPosition(.1f));
    }

    #endregion

    #region IsAtWantedRotation Method

    [Test]
    public void Is_At_The_Wanted_Rotation_0()
    {
        _droneController.TurnTo(0);
        IsTrue(_droneController.IsAtWantedRotation());
    }
    
    [Test]
    public void Is_Not_At_Unwanted_Rotation()
    {
        _droneController.TurnTo(40);
        IsFalse(_droneController.IsAtWantedRotation());
    }

    #endregion

    #region GetDirection Method

    [Test]
    public void Get_Direction_From_A_Position_In_Vector3()
    {
        var pos = new Vector3(-6, 3, 0);
        _droneController.MoveTo(pos);
        var dir = _droneController.GetDirection();
        IsTrue(dir.x < 0);
        IsTrue(dir.y > 0);
        IsTrue(dir.z == 0);
    }

    [Test]
    public void Get_Direction_From_Position_0()
    {
        _droneController.MoveTo(Vector3.zero);
        var dir = _droneController.GetDirection();
        AreEqual(Vector3.zero, dir);
    }

    [Test]
    public void Get_Direction_From_Neg_Pos()
    {
        _droneController.MoveTo(new Vector3(-6, -9, -25));
        var dir = _droneController.GetDirection();
        IsTrue(dir.x < 0);
        IsTrue(dir.y < 0);
        IsTrue(dir.z < 0);
    }

    [Test]
    public void Get_Direction_From_Positive_Pos()
    {
        _droneController.MoveTo(new Vector3(85, 9, 27));
        var dir = _droneController.GetDirection();
        IsTrue(dir.x > 0);
        IsTrue(dir.y > 0);
        IsTrue(dir.z > 0);
    }

    #endregion
}