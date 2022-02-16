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

    [UnityTest]
    public IEnumerator _0_Drone_Prefab_Position()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, Vector3.zero, Quaternion.identity);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0, prefabInstance.transform.position.x);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0, prefabInstance.transform.position.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0, prefabInstance.transform.position.z);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _1_Drone_Prefab_Position_After_1S ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 10, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        
        Debug.Log("Position 1 later :" + prefabInstance.transform.position);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.x);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(10f, prefabInstance.transform.position.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.z);
    }


    [UnityTest]
    public IEnumerator _2_Input_Cyclic_Roll_Left ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 20, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(-1, 0);
        yield return new WaitForSeconds(1f);
        Debug.Log("Position .5 later :" + prefabInstance.transform.position);
        Assert.IsTrue(prefabInstance.transform.position.x < -0.1);
        // UnityEngine.Assertions.Assert.AreApproximatelyEqual(20f, prefabInstance.transform.position.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.z);
    }
    
    [UnityTest]
    public IEnumerator _3_Input_Cyclic_Roll_Right ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 30, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(1, 0);
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(prefabInstance.transform.position.x > 0.1);
        // UnityEngine.Assertions.Assert.AreApproximatelyEqual(30f, prefabInstance.transform.position.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.z);
    }
    [UnityTest]
    public IEnumerator _4_Input_Cyclic_Pitch_Backward ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 40, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, -1);
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(prefabInstance.transform.position.z < -0.1);
        // UnityEngine.Assertions.Assert.AreApproximatelyEqual(30f, prefabInstance.transform.position.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.x);
    }
    
    [UnityTest]
    public IEnumerator _5_Input_Cyclic_Pitch_Forward ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 50, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, 1);
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(prefabInstance.transform.position.z > 0.1);
        // UnityEngine.Assertions.Assert.AreApproximatelyEqual(40f, prefabInstance.transform.position.y);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.x);
    }
    
    [UnityTest]
    public IEnumerator _6_Input_Pedals_Neg ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 60, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = -1f;
        yield return null;
    }

    [UnityTest]
    public IEnumerator _7_Input_Pedals_Pos ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 70, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = 1f;
        yield return null;
    }

    
    [UnityTest]
    public IEnumerator _8_Input_Throttle_Neg ()
    {        
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(80, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = -1f;
        yield return new WaitForSeconds(1f);
        Debug.Log(prefabInstance.transform.position);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(80f, prefabInstance.transform.position.x);
        Assert.IsTrue(prefabInstance.transform.position.y <  -0.1);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.z);
    }

    
    [UnityTest]
    public IEnumerator _9_Input_Throttle_Pos ()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(90, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = 1f;
        yield return new WaitForSeconds(1f);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(90f, prefabInstance.transform.position.x);
        Assert.IsTrue(prefabInstance.transform.position.y >  0.1);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, prefabInstance.transform.position.z);
    }
}
