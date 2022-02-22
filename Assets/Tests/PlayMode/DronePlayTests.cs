using System.Collections;
using Drones;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static NUnit.Framework.Assert;

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
        public IEnumerator Check_Drone_Is_Instantiate()
        {
            NotNull(_drone);
            yield return null;
        }

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
            _droneController.MoveTo(Vector3.up);
        
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

        [UnityTest]
        public IEnumerator Yaw()
        {
            var angle = Quaternion.Euler(0, 60, 0);
            _droneController.TurnTo(angle);
            yield return new WaitForSeconds(3f);
            AreEqual(60f, _drone.transform.rotation.y);
        }
    }
}