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
    private const float _tolerance = 0.00000001f;

    private void AssertApproxVector3(Vector3 originalPosition, Vector3 position, float tolerance)
    {
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalPosition.x, position.x, _tolerance);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalPosition.y, position.y, _tolerance);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalPosition.z, position.z, _tolerance);
    }

    [UnityTest]
    public IEnumerator _0_Drone_Prefab_Position()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, Vector3.zero, Quaternion.identity);
        var originalPosition = prefabInstance.transform.position;
        AssertApproxVector3(originalPosition, prefabInstance.transform.position, _tolerance);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _1_Drone_Prefab_Position_After_1S()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 10, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position;
        yield return new WaitForFixedUpdate();
        AssertApproxVector3(originalPosition, prefabInstance.transform.position, _tolerance);
    }

    [UnityTest]
    public IEnumerator _2_Input_Cyclic_Roll_Left()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 20, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position.x;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(-1, 0);
        yield return new WaitForSeconds(0.2f);

        Assert.Less(prefabInstance.transform.position.x, originalPosition,
            originalPosition + " is greater than " + prefabInstance.transform.position.x);
    }

    [UnityTest]
    public IEnumerator _2_Input_Cyclic_Roll_Right()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 25, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position.x;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(1, 0);
        yield return new WaitForSeconds(0.2f);

        Assert.Greater(prefabInstance.transform.position.x, originalPosition, 
            prefabInstance.transform.position.x + " is greater than " + originalPosition);
    }

    [UnityTest]
    public IEnumerator _3_Input_Cyclic_Roll_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 30, 0), Quaternion.identity);
        var originalY = prefabInstance.transform.position.y;
        var originalZ = prefabInstance.transform.position.z;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(1, 0);
        yield return new WaitForSeconds(1f);

        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalY, prefabInstance.transform.position.y, _tolerance);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalZ, prefabInstance.transform.position.z, _tolerance);
    }


    [UnityTest]
    public IEnumerator _4_Input_Cyclic_Pitch_Backward()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 40, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position.z;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, -1);
        yield return new WaitForSeconds(0.2f);

        Assert.Less(prefabInstance.transform.position.z, originalPosition);
    }

    [UnityTest]
    public IEnumerator _4_Input_Cyclic_Pitch_Forward()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 45, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position.z;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, 1);
        yield return new WaitForSeconds(0.2f);

        Assert.Greater(prefabInstance.transform.position.z, originalPosition);
    }

    [UnityTest]
    public IEnumerator _5_Input_Cyclic_Pitch_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 50, 0), Quaternion.identity);
        var originalX = prefabInstance.transform.position.x;
        var originalY = prefabInstance.transform.position.y;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Cyclic = new Vector2(0, 1);
        yield return new WaitForSeconds(1f);

        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalX, prefabInstance.transform.position.x, _tolerance);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalY, prefabInstance.transform.position.y, _tolerance);
    }


    [UnityTest]
    public IEnumerator _6_Input_Pedals_Negative()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 60, 0), Quaternion.identity);
        var originalRotation = prefabInstance.transform.rotation.y;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = -1f;
        yield return new WaitForFixedUpdate();

        Assert.Less(prefabInstance.transform.rotation.y, originalRotation);
    }

    [UnityTest]
    public IEnumerator _6_Input_Pedals_Positive()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 65, 0), Quaternion.identity);
        var originalRotation = prefabInstance.transform.rotation.y;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = 1f;
        yield return new WaitForFixedUpdate();

        Assert.Greater(prefabInstance.transform.position.y, originalRotation);
    }

    [UnityTest]
    public IEnumerator _7_Input_Pedals_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0, 70, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Pedals = 1f;
        yield return new WaitForFixedUpdate();

        AssertApproxVector3(originalPosition, prefabInstance.transform.position, _tolerance);
    }


    [UnityTest]
    public IEnumerator _8_Input_Throttle_Negative()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(80, 0, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position.y;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = -1f;
        yield return new WaitForFixedUpdate();

        Assert.Less(prefabInstance.transform.position.y, originalPosition);
    }

    [UnityTest]
    public IEnumerator _8_Input_Throttle_Positive()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(85, 0, 0), Quaternion.identity);
        var originalPosition = prefabInstance.transform.position.y;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = 1f;
        yield return new WaitForFixedUpdate();

        Assert.Greater(prefabInstance.transform.position.y, originalPosition);
    }

    [UnityTest]
    public IEnumerator _9_Input_Throttle_Side_Effects()
    {
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(90, 0, 0), Quaternion.identity);
        var originalX = prefabInstance.transform.position.x;
        var originalZ = prefabInstance.transform.position.z;
        prefabInstance.GetComponent<IP_Drone_Inputs>().Throttle = 1f;
        yield return new WaitForFixedUpdate();

        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalX, prefabInstance.transform.position.x, _tolerance);
        UnityEngine.Assertions.Assert.AreApproximatelyEqual(originalZ, prefabInstance.transform.position.z, _tolerance);
    }
}
