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
        public const float Threshold = .2f;

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
                _input.Throttle = 0;
                _input.Cyclic = Vector2.zero;
                return;
            }

            var dir = GetDirection();
            _input.Throttle = dir.y;
            _input.Cyclic = new Vector2(dir.x, dir.z);

            // var t = Mathf.Lerp(0, 1f, Vector3.Distance(wantedPosition, transform.position)*.2f);
            // _input.Throttle = GetDirection().y*t;

            // Debug.Log(
            //     $"pos {transform.position} wanted : {wantedPosition} Cyclic: {_input.Cyclic}"
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

        public void TurnTo(float angle)
        {
        }

        public Vector3 GetDirection() => Vector3.Normalize(wantedPosition - transform.position);
    }
}