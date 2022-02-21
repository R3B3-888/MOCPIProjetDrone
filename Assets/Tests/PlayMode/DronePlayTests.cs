using System.Collections;
using Drones;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class DronePlayTests
    {
        private readonly GameObject _dronePrefab =
            UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

        private GameObject _drone;
        private DroneController _droneController;
        private const float TimeFor1M = 1f;

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

        [UnityTest]
        public IEnumerator _Check_Drone_Is_Instantiate()
        {
            Assert.NotNull(_drone);
            yield return null;
        }

        [UnityTest]
        public IEnumerator _Dont_Move_With_No_Args()
        {
            _droneController.MoveTo(Vector3.zero);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(Vector3.zero, _drone.transform.position);
        }

        [UnityTest]
        public IEnumerator _Move_Up_For_1_Meter()
        {
            _droneController.MoveTo(Vector3.up);
        
            yield return new WaitForSeconds(TimeFor1M);
        
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(1f, _drone.transform.position.y, 0.1f);
        }

        //
        // [UnityTest]
        // public IEnumerator _3_Move_Down()
        // {
        //     var gameObject = new GameObject();
        //     var drone = gameObject.AddComponent<DroneObject>();
        //
        //     drone.Move(Vector3.down);
        //
        //     yield return new WaitForSeconds(TIME_FOR_1M);
        //
        //     Assert.AreEqual(new Vector3(0, -1, 0), gameObject.transform.position);
        // }
        //
        // [UnityTest]
        // public IEnumerator _4_Move_Right()
        // {
        //     var gameObject = new GameObject();
        //     var drone = gameObject.AddComponent<DroneObject>();
        //
        //     drone.Move(Vector3.right);
        //
        //     yield return new WaitForSeconds(TIME_FOR_1M);
        //
        //     Assert.AreEqual(new Vector3(1, 0, 0), gameObject.transform.position);
        // }
        //
        // [UnityTest]
        // public IEnumerator _5_Move_Left()
        // {
        //     var gameObject = new GameObject();
        //     var drone = gameObject.AddComponent<DroneObject>();
        //
        //     drone.Move(Vector3.left);
        //
        //     yield return new WaitForSeconds(TIME_FOR_1M);
        //
        //     Assert.AreEqual(new Vector3(-1, 0, 0), gameObject.transform.position);
        // }
        //
        // [UnityTest]
        // public IEnumerator _6_Move_Forward()
        // {
        //     var gameObject = new GameObject();
        //     var drone = gameObject.AddComponent<DroneObject>();
        //
        //     drone.Move(Vector3.forward);
        //
        //     yield return new WaitForSeconds(TIME_FOR_1M);
        //
        //     Assert.AreEqual(new Vector3(0, 0, 1), gameObject.transform.position);
        // }
        //
        // [UnityTest]
        // public IEnumerator _7_Move_Backward()
        // {
        //     var gameObject = new GameObject();
        //     var drone = gameObject.AddComponent<DroneObject>();
        //
        //     drone.Move(Vector3.back);
        //
        //     yield return new WaitForSeconds(TIME_FOR_1M);
        //
        //     Assert.AreEqual(new Vector3(0, 0, -1), gameObject.transform.position);
        // }
    }
}