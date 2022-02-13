using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DroneTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void _0_Has_Rigidbody()
    {
        // var Drone = new Drone();

        // Assert.AreNotEqual(Drone.GetComponent<Rigidbody>(), null);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DroneTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
