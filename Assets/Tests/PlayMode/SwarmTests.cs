using System.Collections;
using NUnit.Framework;
using Swarm;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests.PlayMode
{
    public class SwarmTests
    {
        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator _5_Drones_Spawned_By_SwarmManager()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Drones/Drone.prefab");
            var go = new GameObject();
            var swarm = go.AddComponent(typeof(SwarmManager)) as SwarmManager;
            const int numberOfDrone = 5;
            swarm.SwarmManagerConstructor(prefab, numberOfDrone);
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(swarm.transform.childCount, numberOfDrone);
        }

        [UnityTest]
        public IEnumerator Swarm()
        {
            yield return null;
        }
    }
}