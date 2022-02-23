using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Client.Differences;
using ICSharpCode.NRefactory.Ast;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Drones
{
    public class DroneController : MonoBehaviour
    {
        #region Variables

        private DroneInputs _input;
        private Rigidbody _rb;
        private const float Threshold = .2f;

        public Vector3 wantedPosition { get; set; }

        #endregion

        #region Main Methods

        public void Awake()
        {
            _input = GetComponent<DroneInputs>();
            _rb = GetComponent<Rigidbody>();
        }

        private Vector3 _basePosition;

        private void Start()
        {
            _basePosition = transform.position;
            Debug.Log(_basePosition);
        }

        void FixedUpdate()
        {
            var direction = GetDirection();

            var t = Mathf.Lerp(0, 1f, Vector3.Distance(wantedPosition, transform.position)*.2f);
            _input.Throttle = GetDirection().y*t;

            Debug.Log("Real pos :" + transform.position + " wanted :" + wantedPosition + " Throttle :" +
                      _input.Throttle + " Direction y: " + direction.y);
        }

        #endregion

        #region Custom Methods

        public void MoveTo(Vector3 pos)
        {
            wantedPosition = pos;
        }

        #endregion

        public bool IsAtWantedPosition()
        {
            // In Sphere
            return transform.position == wantedPosition;
        }

        public void TurnTo(Quaternion angle)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetDirection() => Vector3.Normalize(wantedPosition - transform.position);
    }
}