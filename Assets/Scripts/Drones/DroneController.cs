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
        public const float Threshold = .1f;

        public Vector3 wantedPosition { get; set; }

        #endregion

        #region Main Methods

        public void Awake()
        {
            _input = GetComponent<DroneInputs>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (IsInRadiusOfWantedPosition())
            {
                _rb.drag = 50f;
                _input.Throttle = 0;
                if (Mathf.Abs(_rb.velocity.y) <= 0.1f)
                {
                    _rb.drag = .5f;
                }
                return;
            }

            var dir = GetDirection();
            _input.Throttle = dir.y;

            // var t = Mathf.Lerp(0, 1f, Vector3.Distance(wantedPosition, transform.position)*.2f);
            // _input.Throttle = GetDirection().y*t;

            // Debug.Log(
            //     $"pos {transform.position} wanted : {wantedPosition} Throttle: {_input.Throttle}"
            //     + $" vel: {_rb.velocity}");
        }

        #endregion

        #region Custom Methods

        public void MoveTo(Vector3 pos)
        {
            wantedPosition = pos;
        }

        #endregion

        #region Position Checks

        public bool IsAtWantedPosition() => transform.position == wantedPosition;

        public bool IsInRadiusOfWantedPosition(float radiusThreshold = Threshold) =>
            Vector3.Distance(transform.position, wantedPosition) <= radiusThreshold;

        #endregion

        public void TurnTo(Quaternion angle)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetDirection() => Vector3.Normalize(wantedPosition - transform.position);
    }
}