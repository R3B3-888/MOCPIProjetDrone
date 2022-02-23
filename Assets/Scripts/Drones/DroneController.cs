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

        void FixedUpdate()
        {
            if (transform.position.y > wantedPosition.y)
            {
                StopMove(); //TODO:Cancel this Drag force somewhere
                _rb.drag = 50;
                Debug.Log("Set here");
            }
            else
            {
                // MoveTo(wantedPosition);
                if ((wantedPosition.y - transform.position.y) > Threshold)
                {
                    _input.Throttle = 1;
                }
                // else if ((wantedPosition.y - transform.position.y) < Threshold)
                // {
                //     _input.Throttle = -1;
                // }
            }

            Debug.Log("Real pos :" + transform.position + " And wanted :" + wantedPosition + " Throttle :" +
                      _input.Throttle);
        }

        #endregion

        #region Custom Methods

        public void StopMove()
        {
            _input.Cyclic = Vector2.zero;
            _input.Pedals = 0;
            _input.Throttle = 0;
        }

        public void MoveTo(Vector3 pos)
        {
            wantedPosition = pos;
        }

        #endregion

        #region Utilities

        public void GoUp() => _input.Throttle = 1f;

        public void GoDown() => _input.Throttle = -1f;

        public void GoForward()
        {
            var c = _input.Cyclic;
            _input.Cyclic = new Vector2(c.x, 1);
        }

        public void GoBackward()
        {
            var c = _input.Cyclic;
            _input.Cyclic = new Vector2(c.x, -1);
        }

        public void GoRight()
        {
            var c = _input.Cyclic;
            _input.Cyclic = new Vector2(1, c.y);
        }

        public void GoLeft()
        {
            var c = _input.Cyclic;
            _input.Cyclic = new Vector2(-1, c.y);
        }

        public void TurnRight() => _input.Pedals = 1;

        public void TurnLeft() => _input.Pedals = -1;

        #endregion

        public bool IsAtWantedPosition()
        {
            return GetDirection() == Vector3.zero;
        }

        public void TurnTo(Quaternion angle)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetDirection()
        {
            Vector3 direction = Vector3.zero;
            var position = transform.position;
            direction.x = GetDirectionComponent(wantedPosition.x, position.x);
            direction.y = GetDirectionComponent(wantedPosition.y, position.y);
            direction.z = GetDirectionComponent(wantedPosition.z, position.z);
            return direction;
        }

        private int GetDirectionComponent(float wantedComponent, float positionComponent)
        {
            int c = 0;
            if (wantedComponent < positionComponent)
                c = -1;
            else if (wantedComponent > positionComponent)
                c = 1;
            return c;
        }
    }
}