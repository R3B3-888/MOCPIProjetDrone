using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BeachObjects;

public class PlatformTests
{
    private Platform _platform;

    [Test]
    public void _0_Sets_Platform_to_Max_size()
    {
        float maxSize = 80f;
        _platform = new Platform();

        _platform.SetPlatformSize(maxSize);

        Assert.AreEqual(maxSize, _platform.GetPlatformSize());
    }

    [Test]
    public void _1_Sets_Platform_to_Min_size()
    {
        float minSize = 45f;
        _platform = new Platform();

        _platform.SetPlatformSize(minSize);

        Assert.AreEqual(minSize, _platform.GetPlatformSize());
    }

    [Test]
    public void _2_Sets_Number_of_Swarm_to_Max()
    {
        int max = 5;
        _platform = new Platform(max);

        Assert.AreEqual(max, _platform.GetNbSwarm());
    }

    [Test]
    public void _3_Sets_Number_of_Swarm_to_Min()
    {
        int min = 1;
        _platform = new Platform(min);

        Assert.AreEqual(min, _platform.GetNbSwarm());
    }

    [Test]
    public void _4_Sets_Distance_From_Cost_to_Max()
    {
        float maxDistance = 490;
        _platform = new Platform(1, maxDistance);

        Assert.AreEqual(maxDistance, _platform.GetDistanceFromCost());
    }

    [Test]
    public void _5_Sets_Distance_From_Cost_to_Max()
    {
        float minDistance = 150;
        _platform = new Platform(1, minDistance);

        Assert.AreEqual(minDistance, _platform.GetDistanceFromCost());
    }
}
