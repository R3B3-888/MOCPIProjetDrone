using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Drones
{
    public class Drone
    {
        #region Variables

        private readonly DroneController _controller;
        public int id { get; }
        public GameObject droneInstance { get; }
        public int rankInSwarm { get; private set; } // correspond to index in SwarmManager.drones

        #endregion

        #region Constructors

        public Drone(GameObject droneInstance, int id)
        {
            this.id = id;
            rankInSwarm = id + 1;
            droneInstance.name = $"Drone {id + 1}";
            this.droneInstance = droneInstance;
            _controller = droneInstance.GetComponent<DroneController>();
        }

        public Drone()
        {
            // Tests purpose
        }

        #endregion

        public Vector3 Position() => droneInstance.transform.position;

        public void Stabilize() => _controller.Stabilize();

        public void Destabilize() => _controller.Destabilize();

        public bool IsStillFlying() => _controller.enabled;

        public void MoveTo(Vector3 pos) => _controller.MoveTo(pos);

        public bool IsInRadiusOfWantedPosition() => _controller.IsInRadiusOfWantedPosition();

        public Vector3 CalculateTargetPosition(uint dronesCount, Vector3 baseTargetPosition,
            Vector3 distanceFromTarget, float areaLength, LayoutType layout = LayoutType.Line)
        {
            switch (layout)
            {
                case LayoutType.Line:
                    var t = baseTargetPosition;
                    t.x -= distanceFromTarget.x;
                    t.y = distanceFromTarget.y;
                    t.z += areaLength * ((float) rankInSwarm / (dronesCount + 1));
                    return t;
                case LayoutType.Arc:
                    var angle = GetAngleFromIndex(id, dronesCount);
                    var z = Mathf.Cos(angle) * (areaLength / 2f) * distanceFromTarget.z;
                    var x = Mathf.Sin(angle) * distanceFromTarget.x;
                    return new Vector3(baseTargetPosition.x + x, distanceFromTarget.y, baseTargetPosition.z + z);
                default:
                    return Vector3.zero;
            }
        }

        public static float GetAngleFromIndex(int index, uint n) => Mathf.PI + (index + 1) * Mathf.PI / (n + 1);

        public IEnumerator Crash()
        {
            _controller.FallOff();
            _controller.enabled = false;
            droneInstance.GetComponent<InputsHandler>().enabled = false;
            droneInstance.GetComponent<PlayerInput>().enabled = false;
            yield return new WaitForSeconds(6f);
            Object.Destroy(droneInstance);
        }

        public void UpdateRankInSwarm(int rank) => rankInSwarm = rank + 1;
    }
}

public enum LayoutType
{
    Line,
    Arc
}