using System.Collections;
using Drones;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static NUnit.Framework.Assert;

public class DronePlayTests
{
    #region Variables

    private readonly GameObject _dronePrefab =
        UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

    private GameObject _drone;
    private DroneController _droneController;
    private const float TimeFor1M = 1f;

    #endregion

    #region Setup Teardown

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
        Object.Destroy(_drone);
    }

    #endregion

    #region Instantiate Test

    [UnityTest]
    public IEnumerator Check_Drone_Is_Instantiate()
    {
        NotNull(_drone);
        yield return null;
    }

    #endregion

    #region Move To Method

    [UnityTest]
    public IEnumerator Dont_Move_With_No_Args()
    {
        _droneController.MoveTo(Vector3.zero);
        yield return new WaitForSeconds(1f);
        AreEqual(Vector3.zero, _drone.transform.position);
    }

    [UnityTest]
    public IEnumerator Move_Up_For_1_Meter_Throttle()
    {
        _droneController.MoveTo(Vector3.up);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(1f, _drone.transform.position.y,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator Move_Down_For_1_Meter_Throttle()
    {
        _droneController.MoveTo(Vector3.down);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(-1f, _drone.transform.position.y,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator Move_Up_For_20_Meters_Throttle()
    {
        _droneController.MoveTo(new Vector3(0, 20f, 0));
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(20f, _drone.transform.position.y,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator Move_Left_For_1_Meter_Cyclic()
    {
        _droneController.MoveTo(Vector3.left);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(-1f, _drone.transform.position.x,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator Move_Right_For_1_Meter_Cyclic()
    {
        _droneController.MoveTo(Vector3.right);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(1f, _drone.transform.position.x,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator Move_Back_For_1_Meter_Cyclic()
    {
        _droneController.MoveTo(Vector3.back);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(-1f, _drone.transform.position.z,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator Move_Forward_For_1_Meter_Cyclic()
    {
        _droneController.MoveTo(Vector3.forward);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(1f, _drone.transform.position.z,
            DroneController.Threshold);
    }

    [UnityTest]
    public IEnumerator PlayTest()
    {
        var pos = new Vector3(-1, 2, 3);
        _droneController.MoveTo(pos);
        yield return new WaitUntil(() => _droneController.IsInRadiusOfWantedPosition());
        // yield return new WaitForSeconds(2f);
        IsTrue(_droneController.IsInRadiusOfWantedPosition());
    }

    #endregion

    #region Turn To Method

    [UnityTest]
    public IEnumerator Turn_To_Angle_0()
    {
        _droneController.TurnTo(0);
        yield return new WaitForSeconds(TimeFor1M);
        AreEqual(0f, _drone.transform.rotation.y);
    }

    // [UnityTest]
    // public IEnumerator Turn_Right_To_Angle_45()
    // {
    //     _droneController.TurnTo(45);
    //     yield return null;
    //     AreEqual(45f, _drone.transform.rotation.y);
    // }

    #endregion
}