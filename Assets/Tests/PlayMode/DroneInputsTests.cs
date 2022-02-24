using System.Collections;
using Drones;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static NUnit.Framework.Assert;

namespace Tests.PlayMode
{
    public class DroneInputsTests
    {
        private readonly GameObject _dronePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

        private GameObject _prefabInstance;

        [SetUp]
        public void SetUp()
        {
            _prefabInstance = Object.Instantiate(_dronePrefab, Vector3.zero, Quaternion.identity);
            _prefabInstance.GetComponent<DroneController>().enabled = false;
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_prefabInstance);
        }

        [UnityTest]
        public IEnumerator Drone_Prefab_Position()
        {
            AreEqual(Vector3.zero, _prefabInstance.transform.position);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Drone_Prefab_Position_After_1S()
        {
            yield return new WaitForSeconds(1f);
            AreEqual(Vector3.zero, _prefabInstance.transform.position);
        }

        [UnityTest]
        public IEnumerator Input_Cyclic_Roll_Left()
        {
            _prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(-1, 0);
            yield return new WaitForSeconds(.2f);

            Less(_prefabInstance.transform.position.x, 0f);
        }

        [UnityTest]
        public IEnumerator Input_Cyclic_Roll_Right()
        {
            _prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(1, 0);
            yield return new WaitForSeconds(.2f);

            Greater(_prefabInstance.transform.position.x, 0f);
        }

        [UnityTest]
        public IEnumerator Input_Cyclic_Roll_Side_Effects()
        {
            _prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(1, 0);
            yield return new WaitForFixedUpdate();

            var position = _prefabInstance.transform.position;
            AreEqual(0f, position.y);
            AreEqual(0, position.z);
        }


        [UnityTest]
        public IEnumerator Input_Cyclic_Pitch_Backward()
        {
            _prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(0, -1);
            yield return new WaitForSeconds(.2f);

            Less(_prefabInstance.transform.position.z, 0);
        }

        [UnityTest]
        public IEnumerator Input_Cyclic_Pitch_Forward()
        {
            _prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(0, 1);
            yield return new WaitForSeconds(.2f);

            Greater(_prefabInstance.transform.position.z, 0);
        }

        [UnityTest]
        public IEnumerator Input_Cyclic_Pitch_Side_Effects()
        {
            _prefabInstance.GetComponent<DroneInputs>().Cyclic = new Vector2(0, 1);
            yield return new WaitForFixedUpdate();

            var position = _prefabInstance.transform.position;
            AreEqual(0, position.x);
            AreEqual(0f, position.y);
        }


        [UnityTest]
        public IEnumerator Input_Pedals_Negative()
        {
            _prefabInstance.GetComponent<DroneInputs>().Pedals = -1f;
            yield return new WaitForFixedUpdate();

            Less(_prefabInstance.transform.rotation.y, 0);
        }

        [UnityTest]
        public IEnumerator Input_Pedals_Positive()
        {
            _prefabInstance.GetComponent<DroneInputs>().Pedals = 1f;
            yield return new WaitForFixedUpdate();

            Greater(_prefabInstance.transform.rotation.y, 0);
        }

        [UnityTest]
        public IEnumerator Input_Pedals_Side_Effects()
        {
            _prefabInstance.GetComponent<DroneInputs>().Pedals = 1f;
            yield return new WaitForFixedUpdate();

            AreEqual(Vector3.zero, _prefabInstance.transform.position);
            var rotation = _prefabInstance.transform.rotation;
            AreEqual(0f, rotation.x);
            AreEqual(0f, rotation.z);
        }


        [UnityTest]
        public IEnumerator Input_Throttle_Negative()
        {
            _prefabInstance.GetComponent<DroneInputs>().Throttle = -1f;
            yield return new WaitForFixedUpdate();

            Less(_prefabInstance.transform.position.y, 0);
        }

        [UnityTest]
        public IEnumerator Input_Throttle_Positive()
        {
            _prefabInstance.GetComponent<DroneInputs>().Throttle = 1f;
            yield return new WaitForFixedUpdate();

            Greater(_prefabInstance.transform.position.y, 0);
        }

        [UnityTest]
        public IEnumerator Input_Throttle_Side_Effects()
        {
            _prefabInstance.GetComponent<DroneInputs>().Throttle = 1f;
            yield return new WaitForFixedUpdate();

            var position = _prefabInstance.transform.position;
            AreEqual(0f, position.x);
            AreEqual(0, position.z);
        }
    }
}