using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IndiePixelWay;
using UnityEditor;

public class DroneInputsTests
{
    private GameObject _dronePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/DroneControllable.prefab");
    private GameObject _prefabInstance;
    private Vector3 _pos;

    [SetUp]
    public void BeforeEveryTest()
    {
        _prefabInstance = Object.Instantiate(_dronePrefab, Vector3.zero, Quaternion.identity);
        _pos = _prefabInstance.transform.position;
    }

    [UnityTest]
    public IEnumerator _0_Drone_Prefab_Position()
    {
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0, _pos.x);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0, _pos.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0, _pos.z);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _0_Drone_Prefab_Position_After_1S ()
    {
        _pos = Vector3.up;
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(Vector3.up, _pos);
    }


    [UnityTest]
    public IEnumerator _1_Input_Cyclic_Roll_Left ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, -100, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(-1, 0);
        yield return new WaitForSeconds(5f);
        Assert.IsTrue(prefabInstance.transform.position.x < 0);
        Assert.AreEqual(-100, prefabInstance.transform.position.y);
        Assert.AreEqual(0, prefabInstance.transform.position.z);
    }
    
    [UnityTest]
    public IEnumerator _2_Input_Cyclic_Roll_Right ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, -80, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(1, 0);
        yield return new WaitForSeconds(.1f);
        Assert.IsTrue(prefabInstance.transform.position.x > 0);
        Assert.AreEqual(-80, prefabInstance.transform.position.y);
        Assert.AreEqual(0, prefabInstance.transform.position.z);
    }
    [UnityTest]
    public IEnumerator _3_Input_Cyclic_Pitch_Forward ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, -60, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, 1);
        yield return new WaitForSeconds(.1f);
        Assert.IsTrue(prefabInstance.transform.position.z > 0);
        Assert.AreEqual(0, prefabInstance.transform.position.x);
        Assert.AreEqual(-60, prefabInstance.transform.position.y);
    }
    
    [UnityTest]
    public IEnumerator _4_Input_Cyclic_Pitch_Backward ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, -40, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, -1);
        yield return new WaitForSeconds(.1f);
        Assert.IsTrue(prefabInstance.transform.position.z < 0);
        Assert.AreEqual(0, prefabInstance.transform.position.x);
        Assert.AreEqual(-40, prefabInstance.transform.position.y);
    }
    
    [UnityTest]
    public IEnumerator _5_Input_Pedals_Neg ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = -1f;
        yield return null;
    }

    [UnityTest]
    public IEnumerator _6_Input_Pedals_Pos ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = 1f;
        yield return null;
    }

    
    [UnityTest]
    public IEnumerator _7_Input_Throttle_Neg ()
    {        
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(-10, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = -1f;
        yield return new WaitForSeconds(.1f);
        Assert.IsTrue(prefabInstance.transform.position.y < 0);
        Assert.AreEqual(-10, prefabInstance.transform.position.x);
        Assert.AreEqual(0, prefabInstance.transform.position.z);
    }

    
    [UnityTest]
    public IEnumerator _8_Input_Throttle_Pos ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(10, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = 1f;
        yield return new WaitForSeconds(.1f);
        Assert.IsTrue(prefabInstance.transform.position.y > 0);
        Assert.AreEqual(10, prefabInstance.transform.position.x);
        Assert.AreEqual(0, prefabInstance.transform.position.z);
    }
}
