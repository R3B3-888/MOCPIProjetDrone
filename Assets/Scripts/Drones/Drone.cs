using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drones
{
    [System.Serializable]
    public class Drone
    {
        private int _id { get; }
        private List<Object> _engines { get; }
        private Vector3 _transformPosition;
        public Vector3 TransformPosition { get => _transformPosition; }
        public Drone()
        {
            _id = 1;
            _engines = new List<Object>();
            _transformPosition = Vector3.zero;
        }

        public Drone(int id)
        {
            _id = id;
            _engines = new List<Object>();
            _transformPosition = Vector3.zero;
        }

        public Drone(Vector3 position)
        {
            _id = 1;
            _engines = new List<Object>();
            _transformPosition = position;
        }

        public Drone(int id, Vector3 position)
        {
            _id = id;
            _transformPosition = position;
        }
    }
}