using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeachObjects
{
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
}