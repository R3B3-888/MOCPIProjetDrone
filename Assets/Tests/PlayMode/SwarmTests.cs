using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Drones;
using NUnit.Framework;
using Swarm;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static NUnit.Framework.Assert;

namespace Tests.PlayMode
{
    public class SwarmTests
    {
        private static readonly GameObject DronePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

        private static SwarmManager _swarm;
        private const int NumberOfDrone = 5;
        private static GameObject _gameObject;
        private static readonly Vector3 TargetPosition = new Vector3(7, 2, 5);

        public class SwarmStandbyOn
        {
            private const bool OnStandByAfterSpawn = true;

            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, 1f,
                        OnStandByAfterSpawn);
            }

            [TearDown]
            public void TearDown()
            {
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            [UnityTest]
            public IEnumerator Swarm_Doing_State_1_Standby()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Standby);
                var positions1secAgo = _swarm.drones.Select(drone => drone.Position());
                yield return new WaitForSeconds(1);
                var positions = _swarm.drones.Select(drone => drone.Position());
                AreEqual(positions, positions1secAgo);
            }

            [UnityTest]
            public IEnumerator Swarm_Deploy_Change_State()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Standby);
                AreEqual(GameState.Standby, _swarm.state);
                _swarm.OnDeploy();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                AreEqual(GameState.TakeOff, _swarm.state);
            }
        }

        public class SwarmRepositionning
        {
            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, 1f);
            }

            [TearDown]
            public void TearDown()
            {
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_A_Drone()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForSeconds(1);
                var droneYBeforeCrash = _swarm.drones[0].droneInstance.transform.position.y;
                _swarm.OnCrashing(0);
                yield return new WaitForSeconds(1);
                var droneYAfterCrash = _swarm.drones[0].droneInstance.transform.position.y;
                Less(droneYAfterCrash, droneYBeforeCrash);
                yield return new WaitForSeconds(5.1f);
                AreEqual(4, _swarm.transform.childCount);
            }
        }

        public class SwarmStandbyOff
        {
            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, 1f);
            }

            [TearDown]
            public void TearDown()
            {
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            [UnityTest]
            public IEnumerator Swarm_Doing_State_0_SpawningDrones()
            {
                yield return new WaitForFixedUpdate();
                AreEqual(5, _swarm.transform.childCount);
            }

            [UnityTest]
            public IEnumerator Swarm_Doing_State_2_TakeOff()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.TakeOff);

                // We pickup the y of all drones of the swarm
                var dronesElevation = _swarm.drones.Select(drone => drone.Position().y).ToList();

                yield return new WaitWhile(() => _swarm.state == GameState.TakeOff);

                // We pickup the y of all drones of the swarm after 1 second
                var dronesElevationAtSpawn = _swarm.drones.Select(drone => drone.Position().y).ToList();

                // We compare the two 2 list simultaneously
                foreach (var (y, ySpawn) in dronesElevationAtSpawn.Zip(dronesElevation, Tuple.Create))
                    Greater(y, ySpawn);
            }

            [UnityTest]
            public IEnumerator Swarm_Doing_State_3_On_The_Way_In()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.OnTheWayIn);

                // We calculate the distance DronePosition <===> TargetPosition just at the begin of the new State
                var distanceDroneToTargetBefore = new List<float>(NumberOfDrone);
                distanceDroneToTargetBefore.AddRange(_swarm.drones.Select(drone =>
                    Vector3.Distance(drone.Position(), drone.CalculateTargetPosition(
                        NumberOfDrone, TargetPosition))));

                yield return new WaitForSeconds(2);

                // We calculate the distance DronePosition <===> TargetPosition after 2 sec
                var distanceDroneToTargetOnTheWayIn = new List<float>(NumberOfDrone);
                distanceDroneToTargetOnTheWayIn.AddRange(_swarm.drones.Select(drone =>
                    Vector3.Distance(drone.Position(), drone.CalculateTargetPosition(NumberOfDrone, TargetPosition))));

                // We comparer the two list simultaneously 
                foreach (var (d, dBefore) in distanceDroneToTargetBefore.Zip(
                             distanceDroneToTargetOnTheWayIn, Tuple.Create))
                    Less(dBefore, d);
            }

            [UnityTest]
            public IEnumerator Swarm_Doing_State_4_Monitoring_Stabilisation()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForSeconds(.4f);
                foreach (var rb in _swarm.drones.Select(drone => drone.rb))
                    AreEqual(25f, rb.drag);
            }

            [UnityTest]
            public IEnumerator Swarm_Calculate_Line_Target_Position_For_Each_Drone()
            {
                yield return new WaitForEndOfFrame();
                var n = (uint) _swarm.drones.Count;
                AreEqual(3f, _swarm.drones[0].CalculateTargetPosition(n, TargetPosition).z);
                AreEqual(4f, _swarm.drones[1].CalculateTargetPosition(n, TargetPosition).z);
                AreEqual(5f, _swarm.drones[2].CalculateTargetPosition(n, TargetPosition).z);
                AreEqual(6f, _swarm.drones[3].CalculateTargetPosition(n, TargetPosition).z);
                AreEqual(7f, _swarm.drones[4].CalculateTargetPosition(n, TargetPosition).z);
            }

            [UnityTest]
            public IEnumerator Swarm_Calculate_Line_Target_Position_For_Each_Drone_15_Space_Separation()
            {
                yield return new WaitForEndOfFrame();
                var n = (uint) _swarm.drones.Count;
                AreEqual(-25f, _swarm.drones[0].CalculateTargetPosition(n, TargetPosition, 15f).z);
                AreEqual(-10f, _swarm.drones[1].CalculateTargetPosition(n, TargetPosition, 15f).z);
                AreEqual(5f, _swarm.drones[2].CalculateTargetPosition(n, TargetPosition, 15f).z);
                AreEqual(20f, _swarm.drones[3].CalculateTargetPosition(n, TargetPosition, 15f).z);
                AreEqual(35f, _swarm.drones[4].CalculateTargetPosition(n, TargetPosition, 15f).z);
            }
        }

        public class PositioningTests
        {
            [UnityTest]
            public IEnumerator Swarm_Doing_Turn_To_In_Position_For_1_Drone()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm == null) yield break;
                _swarm.SwarmManagerConstructor(DronePrefab, 1, TargetPosition, 1);
                yield return new WaitForSeconds(.8f);
                var droneAngle = _swarm.drones[0].droneInstance.GetComponent<InputsHandler>().Yaw;
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForSeconds(.8f);
                AreEqual(0, droneAngle);
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            /*[UnityTest]
            public IEnumerator Swarm_Doing_Turn_To_In_Position_For_2_Drone()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm == null) yield break;
                _swarm.SwarmManagerConstructor(DronePrefab, 2, TargetPosition, 1);
                yield return new WaitForSeconds(.8f);
                var droneAngle0 = _swarm.drones[0].droneInstance.GetComponent<InputsHandler>().Yaw;
                var droneAngle1 = _swarm.drones[1].droneInstance.GetComponent<InputsHandler>().Yaw;

                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForSeconds(.8f);
                AreEqual(60, droneAngle0);
                AreEqual(-60, droneAngle1);
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }*/
        }
    }
}