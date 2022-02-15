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
    public IEnumerator _0_Drone_Prefab_OK()
    {
        var droneController = _dronePrefab.GetComponent<IP_Drone_Controller>();
        var prefabInstance = Object.Instantiate(_dronePrefab, new Vector3(0,50,0), Quaternion.identity);
        Assert.IsNotNull(prefabInstance);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _1_Input_Cyclic_X_Neg ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }
    
    [UnityTest]
    public IEnumerator _2_Input_Cyclic_X_Pos ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }
    [UnityTest]
    public IEnumerator _3_Input_Cyclic_Y_Neg ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }
    
    [UnityTest]
    public IEnumerator _4_Input_Cyclic_Y_Pos ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }
    
    [UnityTest]
    public IEnumerator _5_Input_Pedals_Neg ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }

    [UnityTest]
    public IEnumerator _6_Input_Pedals_Pos ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }

    
    [UnityTest]
    public IEnumerator _7_Input_Throttle_Neg ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }

    
    [UnityTest]
    public IEnumerator _8_Input_Throttle_Pos ()
    {
        //:TODO:15/02/22:ALEXIS:Tests to implement
        yield return new System.NotImplementedException();
    }
}
