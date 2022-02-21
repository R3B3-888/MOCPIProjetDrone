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
            if (transform.position == wantedPosition)
            {
                StopMove(); //TODO:Add Drag force on stop moving
                _rb.drag = 5;
            }
            Debug.Log("Real pos :" + transform.position + " And wanted :" + wantedPosition);
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
    }
}