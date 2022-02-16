using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixelWay
{
    [RequireComponent(typeof(BoxCollider))]
    public class IP_Drone_Engine : MonoBehaviour, IEngine
    {
        #region Variables
        [Header("Engine Properties")]
        [SerializeField] private float _maxPower = 4f;

        private float _forceCompensation;
        private Rigidbody _rb;
        private IP_Drone_Inputs _input;
        private Vector3 _engineForce;
        #endregion

        void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();
            _input = GetComponentInParent<IP_Drone_Inputs>();
            _forceCompensation = _rb.mass * Physics.gravity.magnitude;
        }

        #region Interfaces Methods
        public void InitEngine()
        {
            _engineForce = Vector3.zero;

        }

        public void UpdateEngine()
        {
            Vector3 upVec = transform.up;
            upVec.y = 1f;
            _engineForce = upVec * (_forceCompensation + (_input.Throttle * _maxPower)) / 4f;
            _rb.AddForce(_engineForce);
        }
        #endregion
    }
}
