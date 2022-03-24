using UnityEngine;

namespace Drones
{
    public class DroneController : MonoBehaviour
    {
        #region Variables

        private DroneInputs _input;
        private Rigidbody _rb;
        private InputsHandler _inputsHandler;
        
        public const float Threshold = 1f;

        public Vector3 wantedPosition { get; private set; }
        public float wantedRotation { get; private set; }

        #endregion

        #region Main Methods

        public void Awake()
        {
            _input = GetComponent<DroneInputs>();
            _rb = GetComponent<Rigidbody>();
            _inputsHandler = GetComponent<InputsHandler>();
            wantedPosition = this.transform.position;
        }

        private void Update()
        {
            if (IsAtWantedRotation())
                _input.Pedals = 0;
            else
                _input.Pedals = wantedRotation > _inputsHandler.Yaw ? 1 : -1;


            if (IsInRadiusOfWantedPosition())
            {
                _input.Throttle = 0;
                _input.Cyclic = Vector2.zero;
            }
            else
            {
                var dir = GetDirection();
                _input.Throttle = dir.y;
                _input.Cyclic = new Vector2(dir.x, dir.z);
            }
            // var t = Mathf.Lerp(0, 1f, Vector3.Distance(wantedPosition, transform.position)*.2f);
            // _input.Throttle = GetDirection().y*t;

            // Debug.Log(
            //     $"pos {transform.position} wanted : {wantedPosition} Cyclic: {_input.Cyclic}"
            //     + $" vel: {_rb.velocity}");
        }

        #endregion

        #region Move & Turn Methods

        public void MoveTo(Vector3 pos) => wantedPosition = pos;

        public void TurnTo(float angle) => wantedRotation = angle % 360; // Correspond to transform.rotation.y

        #endregion

        #region Transform Checks

        public bool IsInRadiusOfWantedPosition(float radiusThreshold = Threshold) =>
            Vector3.Distance(transform.position, wantedPosition) <= radiusThreshold;

        public bool IsAtWantedRotation() => _inputsHandler.Yaw == wantedRotation;

        #endregion

        public Vector3 GetDirection() => Vector3.Normalize(wantedPosition - transform.position);

        public void Stabilize() => _rb.drag = 25;

        public void Destabilize() => _rb.drag = 1;

        public void FallOff()
        {
            _rb.drag = 3;
            _rb.angularDrag = 3;
        }
    }
}