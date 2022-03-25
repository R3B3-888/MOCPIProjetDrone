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
        private static readonly Vector2 DistanceFromTarget = Vector2.zero;

        public class SwarmStandbyOn
        {
            private const bool OnStandByAfterSpawn = true;

            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget,
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
                var positions1SecAgo = _swarm.drones.Select(drone => drone.Position());
                yield return new WaitForSeconds(1);
                var positions = _swarm.drones.Select(drone => drone.Position());
                AreEqual(positions, positions1SecAgo);
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

        public class DroneCrashingMethods
        {
            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget);
            }

            [TearDown]
            public void TearDown()
            {
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            [UnityTest]
            public IEnumerator Swarm_Drone_Crashing_Disappearing_During_Monitoring()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForSeconds(1);
                var droneYBeforeCrash = _swarm.drones[0].droneInstance.transform.position.y;
                _swarm.OnCrashing(0);
                yield return new WaitForSeconds(1);
                var droneYAfterCrash = _swarm.dronesLost[0].droneInstance.transform.position.y;
                Less(droneYAfterCrash, droneYBeforeCrash);
                yield return new WaitForSeconds(5.1f);
                AreEqual(4, _swarm.transform.childCount);
            }

            [UnityTest]
            public IEnumerator Drone_Testing_Flying()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                var drone = _swarm.drones[3];
                _swarm.OnCrashing(3);
                False(drone.IsStillFlying());
            }
        }

        public class SwarmRepositioning
        {
            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget);
            }

            [TearDown]
            public void TearDown()
            {
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            [UnityTest]
            public IEnumerator Swarm_Cant_Crash_Same_Drone_Id()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                AreEqual(NumberOfDrone - 1, _swarm.drones.Count);
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_Modify_Drones_Ranking_Positions()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                for (var i = 0; i < _swarm.drones.Count; i++)
                {
                    AreEqual(i + 1, _swarm.drones[i].rankInSwarm);
                }

                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                for (var i = 0; i < _swarm.drones.Count; i++)
                {
                    AreEqual(i + 1, _swarm.drones[i].rankInSwarm);
                }
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_Minus_1_At_DronesList()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                AreEqual(NumberOfDrone - 1, _swarm.drones.Count);
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_Changing_State()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                AreEqual(GameState.Repositioning, _swarm.state);
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_Checking_Drones_Staying_In_Swarm_GameObject()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(6.1f);
                var dronesNameInScene = new List<string>();
                for (var i = 0; i <= 3; i++)
                {
                    var n = _swarm.transform.GetChild(i).gameObject.name;
                    Debug.Log($"name {n}");
                    dronesNameInScene.Add(n);
                }

                False(dronesNameInScene.Contains("Drone 4"));
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_Drone_By_Id()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                var dronesIdInScene = new List<int>();
                for (var i = 0; i < 4; i++)
                    dronesIdInScene.Add(_swarm.drones[i].id);
                False(dronesIdInScene.Contains(3));
            }

            [UnityTest]
            public IEnumerator Swarm_Crashing_Drone_In_A_Drone_Lost_List()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                AreEqual(3, _swarm.dronesLost.Last().id);
            }

            [UnityTest]
            public IEnumerator Swarm_Repositioning_To_On_The_Way_In()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                _swarm.OnCrashing(3);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                AreEqual(GameState.OnTheWayIn, _swarm.state);
                foreach (var drone in _swarm.drones)
                    AreEqual(1, drone.droneInstance.GetComponent<Rigidbody>().drag);
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
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget);
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
                var target = new Vector3(24f, 40, 0);
                yield return new WaitUntil(() => _swarm.state == GameState.OnTheWayIn);

                // We calculate the distance DronePosition <===> TargetPosition just at the begin of the new State
                var distanceDroneToTargetBefore = new List<float>(NumberOfDrone);
                distanceDroneToTargetBefore.AddRange(_swarm.drones.Select(drone =>
                    Vector3.Distance(drone.Position(), drone.CalculateTargetPosition(
                        NumberOfDrone, target, DistanceFromTarget, NumberOfDrone + 1))));

                yield return new WaitForSeconds(1);

                // We calculate the distance DronePosition <===> TargetPosition after 2 sec
                var distanceDroneToTargetOnTheWayIn = new List<float>(NumberOfDrone);
                distanceDroneToTargetOnTheWayIn.AddRange(_swarm.drones.Select(drone =>
                    Vector3.Distance(drone.Position(),
                        drone.CalculateTargetPosition(NumberOfDrone, target, DistanceFromTarget, NumberOfDrone + 1))));

                // We comparer the two list simultaneously 
                foreach (var (d, dBefore) in distanceDroneToTargetBefore.Zip(
                             distanceDroneToTargetOnTheWayIn, Tuple.Create))
                    Less(dBefore, d);
            }

            [UnityTest]
            public IEnumerator Swarm_Doing_State_4_Monitoring_Stabilisation()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForEndOfFrame();
                foreach (var rb in _swarm.drones.Select(drone => drone.droneInstance.GetComponent<Rigidbody>()))
                    AreEqual(25f, rb.drag);
            }

            [UnityTest]
            public IEnumerator Swarm_Calculate_Line_Target_Position_For_Each_Drone_Area_Length_N_Plus_1()
            {
                yield return new WaitForEndOfFrame();
                var n = (uint) _swarm.drones.Count;
                for (var i = 0; i < NumberOfDrone; i++)
                {
                    AreEqual(i + 6f,
                        _swarm.drones[i]
                            .CalculateTargetPosition(n, TargetPosition, DistanceFromTarget, NumberOfDrone + 1).z);
                }
            }

            [UnityTest]
            public IEnumerator Swarm_Calculate_Line_Target_Position_For_Each_Drone_Area_Length_18()
            {
                yield return new WaitForEndOfFrame();
                var n = (uint) _swarm.drones.Count;
                for (var i = 0; i < NumberOfDrone; i++)
                {
                    AreEqual(i * 3f + 8f,
                        _swarm.drones[i].CalculateTargetPosition(n, TargetPosition, DistanceFromTarget,
                            (NumberOfDrone + 1) * 3f).z);
                }
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
                _swarm.SwarmManagerConstructor(DronePrefab, 1, TargetPosition, DistanceFromTarget);
                yield return new WaitForSeconds(.8f);
                var droneAngle = _swarm.drones[0].droneInstance.GetComponent<InputsHandler>().Yaw;
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForSeconds(.8f);
                AreEqual(0, droneAngle);
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }
        }

        public class LayoutTesting
        {
            [UnityTest]
            public IEnumerator Swarm_Arc_Calculate_N_Plus_One()
            {
                var targetPosition = new Vector3(25f, 60, 0);
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, 1, targetPosition, DistanceFromTarget);

                yield return new WaitUntil((() => _swarm.state == GameState.Monitoring));
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                AreEqual(0f,
                    _swarm.drones[0]
                        .CalculateTargetPosition(1, targetPosition, new Vector2(10f, 15f), 6, LayoutType.Arc).z);

                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }
        }

        public class DroneCamerasTests
        {
            [SetUp]
            public void SetUp()
            {
                _gameObject = new GameObject();
                _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
                if (_swarm != null)
                    _swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget);
            }

            [TearDown]
            public void TearDown()
            {
                UnityEngine.Object.Destroy(_swarm);
                UnityEngine.Object.Destroy(_gameObject);
            }

            [UnityTest]
            public IEnumerator Drone_Target_Cameras_Equals_Target_Position()
            {
                yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                foreach (var drone in _swarm.drones)
                {
                    var t = TargetPosition;
                    t.y -= DistanceFromTarget.y * .5f;
                    AreEqual(t, drone.droneInstance.transform.Find("DroneTarget").position);
                }
            }
        }
    }
}