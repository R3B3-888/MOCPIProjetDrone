using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Swarm;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static NUnit.Framework.Assert;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class SwarmTests
    {
        private readonly GameObject _dronePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

        private SwarmManager _swarm;
        private const int NumberOfDrone = 5;
        private GameObject _gameObject;
        private readonly Vector3 _targetPosition = new Vector3(7, 2, 5);

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (_swarm != null) _swarm.SwarmManagerConstructor(_dronePrefab, NumberOfDrone, _targetPosition);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_swarm);
            Object.Destroy(_gameObject);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_0_SpawningDrones()
        {
            yield return new WaitForFixedUpdate();
            AreEqual(5, _swarm.transform.childCount);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_1_TakeOff()
        {
            yield return new WaitUntil(() => _swarm.state == GameState.TakeOff);

            // We pickup the y of all drones of the swarm
            var dronesElevation = _swarm.Drones.Select(drone => drone.transform.position.y).ToList();

            yield return new WaitWhile(() => _swarm.state == GameState.TakeOff);

            // We pickup the y of all drones of the swarm after 1 second
            var dronesElevationAtSpawn = _swarm.Drones.Select(drone => drone.transform.position.y).ToList();

            // We compare the two 2 list simultaneously
            foreach (var (y, ySpawn) in dronesElevationAtSpawn.Zip(dronesElevation, Tuple.Create))
                Greater(y, ySpawn);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_2_On_The_Way_In()
        {
            yield return new WaitUntil(() => _swarm.state == GameState.OnTheWayIn);

            // We calculate the distance DronePosition <===> TargetPosition just at the begin of the new State
            var distanceDroneToTargetBefore = new List<float>(NumberOfDrone);
            distanceDroneToTargetBefore.AddRange(_swarm.Drones.Select(drone =>
                Vector3.Distance(drone.transform.position, _swarm.CalculateTargetPosition(drone))));

            yield return new WaitForSeconds(2);

            // We calculate the distance DronePosition <===> TargetPosition after 2 sec
            var distanceDroneToTargetOnTheWayIn = new List<float>(NumberOfDrone);
            distanceDroneToTargetOnTheWayIn.AddRange(_swarm.Drones.Select(drone =>
                Vector3.Distance(drone.transform.position, _swarm.CalculateTargetPosition(drone))));

            // We comparer the two list simultaneously 
            foreach (var (d, dBefore) in distanceDroneToTargetBefore.Zip(
                         distanceDroneToTargetOnTheWayIn, Tuple.Create))
            {
                Less(dBefore, d);
            }
        }

        [UnityTest]
        public IEnumerator Swarm_Calculate_Target_Position_For_Each_Drone()
        {
            yield return new WaitForEndOfFrame();
            AreEqual(3f, _swarm.CalculateTargetPosition(_swarm.Drones[0]).z);
            AreEqual(4f, _swarm.CalculateTargetPosition(_swarm.Drones[1]).z);
            AreEqual(5f, _swarm.CalculateTargetPosition(_swarm.Drones[2]).z);
            AreEqual(6f, _swarm.CalculateTargetPosition(_swarm.Drones[3]).z);
            AreEqual(7f, _swarm.CalculateTargetPosition(_swarm.Drones[4]).z);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_3_Monitoring_Stabilisation()
        {
            yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
            yield return new WaitForSeconds(.4f);
            foreach (var drone in _swarm.Drones)
            {
                var rb = drone.GetComponent<Rigidbody>();
                AreEqual(100f, rb.drag);
            }
        }
    }
}