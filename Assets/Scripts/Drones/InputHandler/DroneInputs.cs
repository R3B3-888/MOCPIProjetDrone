using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Drones
{
    [RequireComponent(typeof(PlayerInput))]
    public class DroneInputs : MonoBehaviour
    {
        #region Variables

        private Vector2 _cyclic;
        private float _pedals;
        private float _throttle;

        public Vector2 Cyclic
        {
            get => _cyclic;
            set => _cyclic = value;
        }

        public float Pedals
        {
            get => _pedals;
            set => _pedals = value;
        }

        public float Throttle
        {
            get => _throttle;
            set => _throttle = value;
        }

        #endregion

        #region Custom Methods

        void OnCyclic(InputValue v) => _cyclic = v.Get<Vector2>();
        void OnPedals(InputValue v) => _pedals = v.Get<float>();
        void OnThrottle(InputValue v) => _throttle = v.Get<float>();

        #endregion
    }
}