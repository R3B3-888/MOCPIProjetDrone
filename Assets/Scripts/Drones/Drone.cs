using System.Collections.Generic;
using UnityEngine;

namespace Drones
{
    public class Drone
    {
        private readonly GameObject _droneInstance;
        private readonly DroneController _controller;

        public Rigidbody rb { get; }

        public Vector3 Position() => _droneInstance.transform.position;
        
        public Drone(GameObject droneInstance)
        {
            _droneInstance = droneInstance;
            _controller = droneInstance.GetComponent<DroneController>();
            rb = droneInstance.GetComponent<Rigidbody>();
        }
        

        public void Stabilize() => _controller.Stabilize();

        public void MoveTo(Vector3 pos) => _controller.MoveTo(pos);
        
        public bool IsInRadiusOfWantedPosition() => _controller.IsInRadiusOfWantedPosition();

        public Vector3 CalculateTargetPosition(List<Drone> drones, Vector3 baseTargetPosition)
        {
            var t = baseTargetPosition;
            var offset = -drones.Count / 2;
            t.z += (offset + drones.IndexOf(this));
            return t;
        }
    }
}