using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Drones
{
    [RequireComponent(typeof(DroneInputs))]
    public class InputsHandler : MonoBehaviour
    {
        #region Variables

        [Header("Control Properties")] [SerializeField]
        private float _minMaxPitch = 30f;

        [SerializeField] private float _minMaxRoll = 30f;
        private readonly float _yawPower = 1f;
        private readonly float _throttlePower = 4f;
        private readonly float _lerpSpeed = 5f;

        private float _yaw;
        private float _finalPitch;
        private float _finalYaw;
        public float Yaw { get => _yaw; }
        private float _finalRoll;
        private float _forceCompensation;

        private DroneInputs _input;
        private Rigidbody _rb;

        #endregion

        #region Main Methods

        void Awake()
        {
            _rb =  GetComponent<Rigidbody>();
            _input = GetComponent<DroneInputs>();
        }
        void Start()
        {
            _forceCompensation = _rb.mass * Physics.gravity.magnitude;
        }

        #endregion

        #region Custom Methods

        void FixedUpdate()
        {
            HandleEngines();
            HandleControls();
        }

        private void HandleEngines()
        {
            Vector3 upVec = transform.up;
            upVec.y = 1f;
            _rb.AddForce(upVec * (_forceCompensation + (_input.Throttle * _throttlePower)));
        }

        private void HandleControls()
        {
            float pitch = _input.Cyclic.y * _minMaxPitch;
            float roll = -_input.Cyclic.x * _minMaxRoll;
            _yaw += _input.Pedals * _yawPower;

            _finalPitch = Mathf.Lerp(_finalPitch, pitch, Time.deltaTime * _lerpSpeed);
            _finalYaw = Mathf.Lerp(_finalYaw, _yaw, Time.deltaTime * _lerpSpeed);
            _finalRoll = Mathf.Lerp(_finalRoll, roll, Time.deltaTime * _lerpSpeed);
            Quaternion rotation = Quaternion.Euler(_finalPitch, _finalYaw, _finalRoll);
            _rb.MoveRotation(rotation);
        }

        #endregion
    }
}