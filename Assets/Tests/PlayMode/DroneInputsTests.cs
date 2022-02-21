using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Drones;
using UnityEditor;

public class DroneInputsTests
{
    private GameObject _dronePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

    [UnityTest]
    public IEnumerator _Drone_Prefab_Position()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, Vector3.zero, Quaternion.identity);
        Assert.AreEqual(Vector3.zero, prefabInstance.transform.position);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _Drone_Prefab_Position_After_1S()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 10, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(new Vector3(0, 10, 0), prefabInstance.transform.position);
    }

    [UnityTest]
    public IEnumerator _Input_Cyclic_Roll_Left()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 20, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(-1, 0);
        yield return new WaitForSeconds(.2f);

        Assert.Less(prefabInstance.transform.position.x, 0f);
    }

    [UnityTest]
    public IEnumerator _Input_Cyclic_Roll_Right()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 25, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(1, 0);
        yield return new WaitForSeconds(.2f);

        Assert.Greater(prefabInstance.transform.position.x, 0f);
    }

    [UnityTest]
    public IEnumerator _Input_Cyclic_Roll_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 30, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(1, 0);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(30f, prefabInstance.transform.position.y);
        Assert.AreEqual(0, prefabInstance.transform.position.z);
    }


    [UnityTest]
    public IEnumerator _Input_Cyclic_Pitch_Backward()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 40, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(0, -1);
        yield return new WaitForSeconds(.2f);

        Assert.Less(prefabInstance.transform.position.z, 0);
    }

    [UnityTest]
    public IEnumerator _Input_Cyclic_Pitch_Forward()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 45, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(0, 1);
        yield return new WaitForSeconds(.2f);

        Assert.Greater(prefabInstance.transform.position.z, 0);
    }

    [UnityTest]
    public IEnumerator _Input_Cyclic_Pitch_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 50, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(0, 1);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(0, prefabInstance.transform.position.x);
        Assert.AreEqual(50f, prefabInstance.transform.position.y);
    }


    [UnityTest]
    public IEnumerator _Input_Pedals_Negative()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 60, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Pedals = -1f;
        yield return new WaitForFixedUpdate();

        Assert.Less(prefabInstance.transform.rotation.y, 0);
    }

    [UnityTest]
    public IEnumerator _Input_Pedals_Positive()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 65, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Pedals = 1f;
        yield return new WaitForFixedUpdate();

        Assert.Greater(prefabInstance.transform.rotation.y, 0);
    }

    [UnityTest]
    public IEnumerator _Input_Pedals_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 70, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Pedals = 1f;
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(new Vector3(0,70,0), prefabInstance.transform.position);
        Assert.AreEqual(0f, prefabInstance.transform.rotation.x);
        Assert.AreEqual(0f, prefabInstance.transform.rotation.z);
    }


    [UnityTest]
    public IEnumerator _Input_Throttle_Negative()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(80, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Throttle = -1f;
        yield return new WaitForFixedUpdate();

        Assert.Less(prefabInstance.transform.position.y, 0);
    }

    [UnityTest]
    public IEnumerator _Input_Throttle_Positive()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(85, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Throttle = 1f;
        yield return new WaitForFixedUpdate();

        Assert.Greater(prefabInstance.transform.position.y, 0);
    }

    [UnityTest]
    public IEnumerator _Input_Throttle_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(90, 0, 0), Quaternion.identity);
        prefabInstance.GetComponent<DroneInputs>().Throttle = 1f;
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(90f, prefabInstance.transform.position.x);
        Assert.AreEqual(0, prefabInstance.transform.position.z);
    }
}
