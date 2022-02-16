using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Drones;
using UnityEditor;

public class DronePlayTests
{
    const int TIME_FOR_1M = 2;

    private GameObject _dronePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/DroneControllable.prefab");
    private GameObject _prefabInstance;

    [SetUp]
    public void BeforeEveryTest()
    {
        _prefabInstance = Object.Instantiate(_dronePrefab, Vector3.zero, Quaternion.identity);
    }

    [UnityTest]
    public IEnumerator _0_Drone_Prefab_OK()
    {
        Assert.IsNotNull(_prefabInstance);
        yield return null;
    }

    // [UnityTest]
    // public IEnumerator _0_Spawn_At_Position()
    // {
    //     var gameObject = new GameObject();
    //     var drone = gameObject.AddComponent<DroneObject>();
    //     var expectedPos = new Vector3(25, 50, 50);


    //     drone.Spawn(expectedPos);

    //     yield return new WaitForSeconds(TIME_FOR_1M);

    //     Assert.AreEqual(expectedPos, drone.TransformPosition);
    // }

    [UnityTest]
    public IEnumerator _1_Stay_In_The_Air()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        yield return new WaitForSeconds(5);

        Assert.AreEqual(new Vector3(0, 0, 0), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator _2_Move_Up()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        drone.Move(Vector3.up);

        yield return new WaitForSeconds(TIME_FOR_1M);

        Assert.AreEqual(new Vector3(0, 1, 0), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator _3_Move_Down()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        drone.Move(Vector3.down);

        yield return new WaitForSeconds(TIME_FOR_1M);

        Assert.AreEqual(new Vector3(0, -1, 0), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator _4_Move_Right()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        drone.Move(Vector3.right);

        yield return new WaitForSeconds(TIME_FOR_1M);

        Assert.AreEqual(new Vector3(1, 0, 0), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator _5_Move_Left()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        drone.Move(Vector3.left);

        yield return new WaitForSeconds(TIME_FOR_1M);

        Assert.AreEqual(new Vector3(-1, 0, 0), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator _6_Move_Forward()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        drone.Move(Vector3.forward);

        yield return new WaitForSeconds(TIME_FOR_1M);

        Assert.AreEqual(new Vector3(0, 0, 1), gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator _7_Move_Backward()
    {
        var gameObject = new GameObject();
        var drone = gameObject.AddComponent<DroneObject>();

        drone.Move(Vector3.back);

        yield return new WaitForSeconds(TIME_FOR_1M);

        Assert.AreEqual(new Vector3(0, 0, -1), gameObject.transform.position);
    }
}
