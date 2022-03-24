using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Drones
{
    public class Drone
    {
        private readonly DroneController _controller;
        public uint id { get; set; }

        public Rigidbody rb { get; }
        public GameObject droneInstance { get; }

        public Vector3 Position() => droneInstance.transform.position;

        public Drone(GameObject droneInstance, uint id)
        {
            this.id = id;
            droneInstance.name = $"Drone {id + 1}";
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
            Vector3 distanceFromTarget, float distanceBetweenDrones = 1f, LayoutType layout = LayoutType.Line)
        {
            switch (layout)
            {
                case LayoutType.Line:
                    var t = baseTargetPosition;
                    var offset = -dronesCount / 2;
                    t.z += (offset + id) * distanceBetweenDrones;
                    t.y = distanceFromTarget.y;
                    t.x -= distanceFromTarget.x;
                    return t;
                case LayoutType.Arc:
                    var angle = GetAngleFromIndex(id, dronesCount);
                    var z = Mathf.Cos(angle) * distanceBetweenDrones * distanceFromTarget.z;
                    var x = Mathf.Sin(angle) * distanceFromTarget.x;
                    return new Vector3(baseTargetPosition.x + x, distanceFromTarget.y, baseTargetPosition.z + z);
                default:
                    return Vector3.zero;
            }
        }

        public static float GetAngleFromIndex(uint index, uint n) => Mathf.PI + (index + 1) * Mathf.PI / (n + 1);

        public IEnumerator Crash()
        {
            _controller.enabled = false;
            droneInstance.GetComponent<InputsHandler>().enabled = false;
            droneInstance.GetComponent<PlayerInput>().enabled = false;
            rb.drag = 3;
            rb.angularDrag = 3;
            yield return new WaitForSeconds(6f);
            Object.Destroy(droneInstance);
        }

        public bool IsStillFlying() => _controller.enabled;
    }
}

public enum LayoutType
{
    Line,
    Arc
}