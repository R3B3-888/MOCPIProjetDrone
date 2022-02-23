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
            var direction = GetDirection();
            _input.Cyclic = new Vector2(direction.x, direction.z);
            _input.Throttle = direction.y;
            Debug.Log("Real pos :" + transform.position + " And wanted :" + wantedPosition + " Throttle :" +
                      _input.Throttle);
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

        public Vector3 GetDirection()
        {
            var direction = Vector3.zero;
            var position = transform.position;
            direction.x = GetDirectionComponent(wantedPosition.x, position.x);
            direction.y = GetDirectionComponent(wantedPosition.y, position.y);
            direction.z = GetDirectionComponent(wantedPosition.z, position.z);
            return direction;
        }

        private static int GetDirectionComponent(float wantedComponent, float positionComponent)
        {
            var c = 0;
            if (wantedComponent < positionComponent)
                c = -1;
            else if (wantedComponent > positionComponent)
                c = 1;
            return c;
        }
    }
}