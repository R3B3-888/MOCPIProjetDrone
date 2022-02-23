using Drones;
using NUnit.Framework;
using UnityEngine;

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

    #endregion

    #region IsAtWantedPosition Method

    [Test]
    public void Is_At_The_Wanted_Position_Without_Any_Displacement()
    {
        _droneController.MoveTo(Vector3.zero);
        Assert.IsTrue(_droneController.IsAtWantedPosition());
    }

    #endregion

    #region GetDirection Method

    [Test]
    public void Get_Direction_From_A_Position_In_Vector3()
    {
        var pos = new Vector3(-6, 3, 0);
        _droneController.MoveTo(pos);
        var direction = _droneController.GetDirection();
        Assert.AreEqual(new Vector3(-1, 1, 0), direction);
    }

    [Test]
    public void Get_Direction_From_Position_0()
    {
        _droneController.MoveTo(Vector3.zero);
        Assert.AreEqual(Vector3.zero, _droneController.GetDirection());
    }

    [Test]
    public void Get_Direction_From_Neg_Pos()
    {
        _droneController.MoveTo(new Vector3(-6, -9, -25978));
        Assert.AreEqual(new Vector3(-1, -1, -1), _droneController.GetDirection());
    }

    [Test]
    public void Get_Direction_From_Positive_Pos()
    {
        _droneController.MoveTo(new Vector3(85, 9, 27));
        Assert.AreEqual(Vector3.one, _droneController.GetDirection());
    }

    #endregion
}