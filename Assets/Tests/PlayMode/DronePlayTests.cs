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
    public IEnumerator Move_Up_For_1_Meter()
    {
        var pos = new Vector3(0, 1, 0);
        _droneController.MoveTo(pos);
        Debug.Log(Vector3.Normalize(_droneController.wantedPosition - _drone.transform.position));
        yield return new WaitForSeconds(TimeFor1M);

        UnityEngine.Assertions.Assert.AreApproximatelyEqual(1f, _drone.transform.position.y, 0.1f);
    }

    [UnityTest]
    public IEnumerator PlayTest()
    {
        var pos = new Vector3(-1, 2, 3);
        _droneController.MoveTo(pos);
        yield return new WaitForSeconds(2f);
        IsTrue(_droneController.IsAtWantedPosition());
    }

    #endregion

    [UnityTest]
    public IEnumerator Yaw()
    {
        var angle = Quaternion.Euler(0, 60, 0);
        _droneController.TurnTo(angle);
        yield return new WaitForSeconds(3f);
        AreEqual(60f, _drone.transform.rotation.y);
    }
}