using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IndiePixelWay
{
    [RequireComponent(typeof(IP_Drone_Inputs))]
    public class IP_Drone_Controller : IP_Base_Rigidbody
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

        private IP_Drone_Inputs _input;
        private List<IEngine> _engines = new List<IEngine>();
        #endregion

        #region Main Methods
        void Start()
        {
            _input = GetComponent<IP_Drone_Inputs>();
            _engines = GetComponentsInChildren<IEngine>().ToList<IEngine>();
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
            foreach (var engine in _engines)
                engine.UpdateEngine();
        }

        protected virtual void HandleControls()
        {
            float pitch = -_input.Cyclic.y * _minMaxPitch;
            float roll = _input.Cyclic.x * _minMaxRoll;
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
