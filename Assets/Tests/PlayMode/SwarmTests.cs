﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private readonly GameObject _dronePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

        private SwarmManager _swarm;
        private const int NumberOfDrone = 5;
        private GameObject _gameObject;
        private readonly Vector3 _targetPosition = new Vector3(7, 2, 5);
        private bool _onStandByAfterSpawn = true;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (_swarm != null)
                _swarm.SwarmManagerConstructor(_dronePrefab, NumberOfDrone, _targetPosition, _onStandByAfterSpawn);
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
            yield return new WaitForSeconds(.4f);
            Debug.Log(_swarm.state);
            AreEqual(GameState.TakeOff, _swarm.state);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_2_TakeOff()
        {
            _swarm.OnDeploy();
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
            _swarm.OnDeploy();
            yield return new WaitUntil(() => _swarm.state == GameState.OnTheWayIn);

            // We calculate the distance DronePosition <===> TargetPosition just at the begin of the new State
            var distanceDroneToTargetBefore = new List<float>(NumberOfDrone);
            distanceDroneToTargetBefore.AddRange(_swarm.drones.Select(drone =>
                Vector3.Distance(drone.Position(), drone.CalculateTargetPosition(
                    _swarm.drones, _targetPosition))));

            yield return new WaitForSeconds(2);

            // We calculate the distance DronePosition <===> TargetPosition after 2 sec
            var distanceDroneToTargetOnTheWayIn = new List<float>(NumberOfDrone);
            distanceDroneToTargetOnTheWayIn.AddRange(_swarm.drones.Select(drone =>
                Vector3.Distance(drone.Position(), drone.CalculateTargetPosition(_swarm.drones
                    , _targetPosition))));

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
            AreEqual(3f, _swarm.drones[0].CalculateTargetPosition(_swarm.drones, _targetPosition).z);
            AreEqual(4f, _swarm.drones[1].CalculateTargetPosition(_swarm.drones, _targetPosition).z);
            AreEqual(5f, _swarm.drones[2].CalculateTargetPosition(_swarm.drones, _targetPosition).z);
            AreEqual(6f, _swarm.drones[3].CalculateTargetPosition(_swarm.drones, _targetPosition).z);
            AreEqual(7f, _swarm.drones[4].CalculateTargetPosition(_swarm.drones, _targetPosition).z);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_4_Monitoring_Stabilisation()
        {
            _swarm.OnDeploy();
            yield return new WaitUntil(() => _swarm.state == GameState.Monitoring);
            yield return new WaitForSeconds(.4f);
            foreach (var drone in _swarm.drones)
            {
                var rb = drone.rb;
                AreEqual(100f, rb.drag);
            }
        }
    }
}