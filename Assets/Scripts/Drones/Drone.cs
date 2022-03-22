using UnityEngine;

namespace Drones
{
    public class Drone
    {
        private readonly DroneController _controller;
        private readonly uint _id;
        public static readonly Vector3 DistanceFromTarget = new Vector3(30, 60, 3);

        public Rigidbody rb { get; }
        public GameObject droneInstance { get; }

        public Vector3 Position() => droneInstance.transform.position;

        public Drone(GameObject droneInstance, uint id)
        {
            _id = id;
            this.droneInstance = droneInstance;
            _controller = droneInstance.GetComponent<DroneController>();
            rb = droneInstance.GetComponent<Rigidbody>();
        }

        public Drone()
        {
            // Tests purpose
        }

        public void Stabilize() => _controller.Stabilize();

        public void MoveTo(Vector3 pos) => _controller.MoveTo(pos);

        public bool IsInRadiusOfWantedPosition() => _controller.IsInRadiusOfWantedPosition(radiusThreshold:1f);

        public Vector3 CalculateTargetPosition(uint dronesCount, Vector3 baseTargetPosition,
            float distanceBetweenDrones = 1f, LayoutType layout = LayoutType.Line)
        {
            switch (layout)
            {
                case LayoutType.Line:
                    var t = baseTargetPosition;
                    var offset = -dronesCount / 2;
                    t.z += (offset + _id) * distanceBetweenDrones;
                    t.y = DistanceFromTarget.y;
                    t.x -= DistanceFromTarget.x;
                    return t;
                case LayoutType.Arc:
                    var angle = GetAngleFromIndex(_id, dronesCount);
                    var z = Mathf.Cos(angle) * distanceBetweenDrones * DistanceFromTarget.z;
                    var x = Mathf.Sin(angle) * DistanceFromTarget.x;
                    return new Vector3(baseTargetPosition.x + x, DistanceFromTarget.y, baseTargetPosition.z + z);
                default:
                    return Vector3.zero;
            }
        }

        public static float GetAngleFromIndex(uint index, uint n) => Mathf.PI + (index + 1) * Mathf.PI / (n + 1);
        
    }
}

public enum LayoutType
{
    Line,
    Arc
}