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
        private const int NumberOfDrone = 3;
        private GameObject _go;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject();
            _swarm = _go.AddComponent(typeof(SwarmManager)) as SwarmManager;
            if (_swarm != null) _swarm.SwarmManagerConstructor(_dronePrefab, NumberOfDrone);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_swarm);
            Object.Destroy(_go);
        }

        [UnityTest]
        public IEnumerator _5_Drones_Spawned_By_SwarmManager()
        {
            _swarm.SwarmManagerConstructor(_dronePrefab, 5);
            yield return new WaitForFixedUpdate();
            AreEqual(_swarm.transform.childCount, NumberOfDrone);
        }

        [UnityTest]
        public IEnumerator Swarm_Doing_State_TakeOff()
        {
            var dronesElevation = _swarm.Drones.Select(drone => drone.transform.position.y).ToList();
            yield return new WaitForSeconds(1);
            var dronesElevationAtSpawn = _swarm.Drones.Select(drone => drone.transform.position.y).ToList();
            AreEqual(GameState.TakeOff, _swarm.State);
            foreach (var nw in dronesElevationAtSpawn.Zip(dronesElevation, Tuple.Create)) 
            {
                Greater(nw.Item1, nw.Item2);
            } 
        }
    }
}