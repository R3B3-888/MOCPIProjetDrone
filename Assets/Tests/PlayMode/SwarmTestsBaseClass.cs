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

namespace Tests.PlayMode
{
    public class SwarmTestsBaseClass
    {
        protected static GameObject _gameObject;
        protected static SwarmManager Swarm;

        protected static readonly GameObject DronePrefab =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");

        protected const int NumberOfDrone = 5;
        protected static Vector3 TargetPosition = new Vector3(7, 2, 5);
        protected static readonly Vector2 DistanceFromTarget = Vector2.zero;

        [UnitySetUp]
        public IEnumerator UnitySetUpBase()
        {
            yield return null;
            _gameObject = new GameObject();
        }

        [TearDown]
        public void TearDownBase()
        {
            UnityEngine.Object.Destroy(_gameObject);
        }
    }


    internal class StandbyOn : SwarmTestsBaseClass
    {
        private const bool OnStandByAfterSpawn = true;

        [UnitySetUp]
        public IEnumerator StandbyOnUnitySetUp()
        {
            Swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (Swarm != null)
                Swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget,
                    OnStandByAfterSpawn);

            yield return new WaitUntil(() => Swarm.state == GameState.Standby);
        }

        [UnityTearDown]
        public IEnumerator StandbyOnUnityTearDown()
        {
            UnityEngine.Object.Destroy(Swarm);
            yield return null;
        }

        [UnityTest]
        public IEnumerator HandleStandby_Drone_Dont_Move()
        {
            var positions1SecAgo = Swarm.drones.Select(drone => drone.Position());
            yield return new WaitForSeconds(1);
            var positions = Swarm.drones.Select(drone => drone.Position());
            AreEqual(positions, positions1SecAgo);
        }

        [UnityTest]
        public IEnumerator OnDeploy_Activate_TakeOff()
        {
            AreEqual(GameState.Standby, Swarm.state);
            Swarm.OnDeploy();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            AreEqual(GameState.TakeOff, Swarm.state);
        }
    }

    internal class StandbyOff : SwarmTestsBaseClass
    {
        [SetUp]
        public void StandbyOffSetUp()
        {
            Swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (Swarm != null)
                Swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget);
        }

        [TearDown]
        public void StandbyOffTearDown()
        {
            UnityEngine.Object.Destroy(Swarm);
        }
    }

    internal class SwarmStandbyOff : StandbyOff
    {
        [UnityTest]
        public IEnumerator Swarm_Doing_State_0_SpawningDrones()
        {
            yield return new WaitForFixedUpdate();
            AreEqual(5, Swarm.transform.childCount);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_2_TakeOff()
        {
            yield return new WaitUntil(() => Swarm.state == GameState.TakeOff);

            // We pickup the y of all drones of the swarm
            var dronesElevation = Swarm.drones.Select(drone => drone.Position().y).ToList();

            yield return new WaitWhile(() => Swarm.state == GameState.TakeOff);

            // We pickup the y of all drones of the swarm after 1 second
            var dronesElevationAtSpawn = Swarm.drones.Select(drone => drone.Position().y).ToList();

            // We compare the two 2 list simultaneously
            foreach (var (y, ySpawn) in dronesElevationAtSpawn.Zip(dronesElevation, Tuple.Create))
                Greater(y, ySpawn);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_3_On_The_Way_In()
        {
            var target = new Vector3(24f, 40, 0);
            yield return new WaitUntil(() => Swarm.state == GameState.OnTheWayIn);

            // We calculate the distance DronePosition <===> TargetPosition just at the begin of the new State
            var distanceDroneToTargetBefore = new List<float>(NumberOfDrone);
            distanceDroneToTargetBefore.AddRange(Swarm.drones.Select(drone =>
                Vector3.Distance(drone.Position(), drone.CalculateTargetPosition(
                    NumberOfDrone, target, DistanceFromTarget, NumberOfDrone + 1))));

            yield return new WaitForSeconds(1);

            // We calculate the distance DronePosition <===> TargetPosition after 2 sec
            var distanceDroneToTargetOnTheWayIn = new List<float>(NumberOfDrone);
            distanceDroneToTargetOnTheWayIn.AddRange(Swarm.drones.Select(drone =>
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
            yield return new WaitUntil(() => Swarm.state == GameState.Monitoring);
            yield return new WaitForEndOfFrame();
            foreach (var rb in Swarm.drones.Select(drone => drone.droneInstance.GetComponent<Rigidbody>()))
                AreEqual(25f, rb.drag);
        }

        [UnityTest]
        public IEnumerator Swarm_Calculate_Line_Target_Position_For_Each_Drone_Area_Length_N_Plus_1()
        {
            yield return new WaitForEndOfFrame();
            var n = (uint) Swarm.drones.Count;
            for (var i = 0; i < NumberOfDrone; i++)
            {
                AreEqual(i + 6f,
                    Swarm.drones[i]
                        .CalculateTargetPosition(n, TargetPosition, DistanceFromTarget, NumberOfDrone + 1).z);
            }
        }

        [UnityTest]
        public IEnumerator Swarm_Calculate_Line_Target_Position_For_Each_Drone_Area_Length_18()
        {
            yield return new WaitForEndOfFrame();
            var n = (uint) Swarm.drones.Count;
            for (var i = 0; i < NumberOfDrone; i++)
            {
                AreEqual(i * 3f + 8f,
                    Swarm.drones[i].CalculateTargetPosition(n, TargetPosition, DistanceFromTarget,
                        (NumberOfDrone + 1) * 3f).z);
            }
        }
    }

    internal class AfterMonitoringTests : SwarmTestsBaseClass
    {
        [UnitySetUp]
        public IEnumerator AfterMonitoringTestsUnitySetUp()
        {
            Swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (Swarm != null)
                Swarm.SwarmManagerConstructor(DronePrefab, NumberOfDrone, TargetPosition, DistanceFromTarget);
            yield return new WaitUntil(() => Swarm.state == GameState.Monitoring);
        }

        [TearDown]
        public void AfterMonitoringTestsTearDown() => UnityEngine.Object.Destroy(Swarm);
    }

    internal class HandleMonitoringMethod : AfterMonitoringTests
    {
        [UnityTest]
        public IEnumerator Drone_Target_Cameras_Equals_Target_Position()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            foreach (var drone in Swarm.drones)
            {
                var t = TargetPosition;
                t.y -= DistanceFromTarget.y * .5f;
                AreEqual(t, drone.droneInstance.transform.Find("DroneTarget").position);
            }
        }
    }

    internal class DroneCrashingMethods : AfterMonitoringTests
    {
        [UnityTest]
        public IEnumerator Swarm_Drone_Crashing_Disappearing_During_Monitoring()
        {
            yield return new WaitForSeconds(1);
            var droneYBeforeCrash = Swarm.drones[0].droneInstance.transform.position.y;
            Swarm.OnCrashing(0);
            yield return new WaitForSeconds(1);
            var droneYAfterCrash = Swarm.dronesLost[0].droneInstance.transform.position.y;
            Less(droneYAfterCrash, droneYBeforeCrash);
            yield return new WaitForSeconds(5.1f);
            AreEqual(4, Swarm.transform.childCount);
        }

        [Test]
        public void Drone_Testing_Flying()
        {
            var drone = Swarm.drones[3];
            Swarm.OnCrashing(3);
            False(drone.IsStillFlying());
        }
    }

    internal class SwarmRepositioning : AfterMonitoringTests
    {
        [UnityTest]
        public IEnumerator Swarm_Cant_Crash_Same_Drone_Id()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            AreEqual(NumberOfDrone - 1, Swarm.drones.Count);
        }

        [UnityTest]
        public IEnumerator Swarm_Crashing_Modify_Drones_Ranking_Positions()
        {
            for (var i = 0; i < Swarm.drones.Count; i++)
            {
                AreEqual(i + 1, Swarm.drones[i].rankInSwarm);
            }

            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            for (var i = 0; i < Swarm.drones.Count; i++)
            {
                AreEqual(i + 1, Swarm.drones[i].rankInSwarm);
            }
        }

        [UnityTest]
        public IEnumerator Swarm_Crashing_Minus_1_At_DronesList()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            AreEqual(NumberOfDrone - 1, Swarm.drones.Count);
        }

        [UnityTest]
        public IEnumerator Swarm_Crashing_Changing_State()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            AreEqual(GameState.Repositioning, Swarm.state);
        }

        [UnityTest]
        public IEnumerator Swarm_Crashing_Checking_Drones_Staying_In_Swarm_GameObject()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(6.1f);
            var dronesNameInScene = new List<string>();
            for (var i = 0; i <= 3; i++)
            {
                var n = Swarm.transform.GetChild(i).gameObject.name;
                Debug.Log($"name {n}");
                dronesNameInScene.Add(n);
            }

            False(dronesNameInScene.Contains("Drone 4"));
        }

        [UnityTest]
        public IEnumerator Swarm_Crashing_Drone_By_Id()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            var dronesIdInScene = new List<int>();
            for (var i = 0; i < 4; i++)
                dronesIdInScene.Add(Swarm.drones[i].id);
            False(dronesIdInScene.Contains(3));
        }

        [UnityTest]
        public IEnumerator Swarm_Crashing_Drone_In_A_Drone_Lost_List()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            AreEqual(3, Swarm.dronesLost.Last().id);
        }

        [UnityTest]
        public IEnumerator Swarm_Repositioning_To_On_The_Way_In()
        {
            Swarm.OnCrashing(3);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            AreEqual(GameState.OnTheWayIn, Swarm.state);
            foreach (var drone in Swarm.drones)
                AreEqual(1, drone.droneInstance.GetComponent<Rigidbody>().drag);
        }
    }

    public class LayoutTests : SwarmTestsBaseClass
    {
        [UnitySetUp]
        public IEnumerator LayoutTestsSetUp()
        {
            TargetPosition = new Vector3(25f, 60, 0);
            Swarm = _gameObject.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (Swarm != null)
                Swarm.SwarmManagerConstructor(DronePrefab, 1, TargetPosition, DistanceFromTarget);
            yield return new WaitUntil((() => Swarm.state == GameState.Monitoring));
        }

        [TearDown]
        public void LayoutTestsTearDown()
        {
            UnityEngine.Object.Destroy(Swarm);
        }


        [UnityTest]
        public IEnumerator Swarm_Arc_Calculate_N_Plus_One()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (Swarm != null)
            {
                AreEqual(0f,
                    Swarm.drones[0]
                        .CalculateTargetPosition(1, TargetPosition, new Vector2(10f, 15f), 6, LayoutType.Arc).z);
            }
        }

        [UnityTest]
        public IEnumerator Set_Layout_Type()
        {
            Swarm.OnChangingLayout(LayoutType.Line);
            AreEqual(LayoutType.Line, Swarm.layout);
            Swarm.OnChangingLayout(LayoutType.Arc);
            AreEqual(LayoutType.Arc, Swarm.layout);
            yield return null;
        }
    }
}