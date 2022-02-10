using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPlatform
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

    public class Platform
    {
        private BoxCollider _surface;
        private int _swarmNumber;
        private float _distanceFromCost;
        private List<Swarm> _swarmList;

        public Platform()
        {
            _surface = new GameObject().AddComponent<BoxCollider>();
            _surface.transform.localScale = new Vector3(45.0f, 1.0f, 45.0f);
            _swarmNumber = 1;
            _distanceFromCost = 200;
            _swarmList = new List<Swarm>();
            Swarm newSwarm = new Swarm(0);
            _swarmList.Add(newSwarm);
        }

        public Platform(int swarmNumber)
        {
            if (swarmNumber < 1)
                swarmNumber = 1;
            else if (swarmNumber > 5)
                swarmNumber = 5;
            
            _surface = new GameObject().AddComponent<BoxCollider>();
            _surface.transform.localScale = new Vector3(45.0f, 1.0f, 45.0f);
            _swarmNumber = swarmNumber;
            _distanceFromCost = 200;
            _swarmList = new List<Swarm>();

            for (int i = 0; i < swarmNumber; i++)
            {
                Swarm newSwarm = new Swarm(i);
                _swarmList.Add(newSwarm);
            }
        }

        public Platform(int swarmNumber, float distanceFromCost)
        {
            if (swarmNumber < 1)
                swarmNumber = 1;
            else if (swarmNumber > 10)
                swarmNumber = 5;
            
            _surface = new GameObject().AddComponent<BoxCollider>();
            _surface.transform.localScale = new Vector3(45.0f, 1.0f, 45.0f);
            _swarmNumber = swarmNumber;
            _distanceFromCost = distanceFromCost;
            _swarmList = new List<Swarm>();

            for (int i = 0; i < swarmNumber; i++)
            {
                Swarm newSwarm = new Swarm(i);
                _swarmList.Add(newSwarm);
            }
        }

        public int GetNbSwarm()
        {
            return _swarmNumber;
        }

        public float GetDistanceFromCost()
        {
            return _distanceFromCost;
        }

        public float GetPlatformSize()
        {
            return _surface.transform.localScale.z;
        }

        public void SetDistanceFromCost(float size)
        {
            if (size < 150.0f)
                size = 150.0f;
            else if (size > 490.0f)
                size = 490.0f;

            Vector3 scale = _surface.transform.localScale;

            _surface.transform.localScale = new Vector3(scale.x, scale.y, size);
        }

        public void SetPlatformSize(float size)
        {
            if (size < 45.0f)
                size = 45.0f;
            else if (size > 80.0f)
                size = 80.0f;

            Vector3 scale = _surface.transform.localScale;

            _surface.transform.localScale = new Vector3(scale.x, scale.y, size);
        }
    }

    public class Swarm
    {
        private int _id;
        private List<object> _droneList;

        public Swarm(int id)
        {
            _id = id;
            _droneList = new List<object>();

            for (int i = 0; i < 4; i++)
            {
                object newDrone = new object();
                _droneList.Add(newDrone);
            }
        }
    }
}
