using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Drones
{
    [RequireComponent(typeof(DroneInputs))]
    public class DroneController : BaseRigidbody
    {
        #region Variables
        [Header("Control Properties")]
        [SerializeField] private float _minMaxPitch = 30f;
        [SerializeField] private float _minMaxRoll = 30f;
        [SerializeField] private float _yawPower = 3f;
        [SerializeField] private float _lerpSpeed = 2f;
        
        private float _yaw;
        private float _finalPitch;
        private float _finalYaw;
        private float _finalRoll;
        private float _forceCompensation; 

        private DroneInputs _input;
        private float _maxPower = 4f;

        #endregion

        #region Main Methods
        void Start()
        {
            _input = GetComponent<DroneInputs>();
            _forceCompensation = rb.mass * Physics.gravity.magnitude;
        }
        #endregion

        #region Custom Methods
        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }

        protected virtual void HandleEngines()
        {
            Vector3 upVec = transform.up;
            upVec.y = 1f;
            rb.AddForce(upVec * (_forceCompensation + (_input.Throttle * _maxPower)));
        }

        protected virtual void HandleControls()
        {
            float pitch = _input.Cyclic.y * _minMaxPitch;
            float roll = -_input.Cyclic.x * _minMaxRoll;
            _yaw += _input.Pedals * _yawPower;

            _finalPitch = Mathf.Lerp(_finalPitch, pitch, Time.deltaTime * _lerpSpeed);
            _finalYaw = Mathf.Lerp(_finalYaw, _yaw, Time.deltaTime * _lerpSpeed);
            _finalRoll = Mathf.Lerp(_finalRoll, roll, Time.deltaTime * _lerpSpeed);
            Quaternion rotation = Quaternion.Euler(_finalPitch, _finalYaw, _finalRoll);
            rb.MoveRotation(rotation);
        }
        #endregion
    }
}
