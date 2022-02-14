using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace IndiePixelWay
{
    [RequireComponent(typeof(PlayerInput))]
    public class IP_Drone_Inputs : MonoBehaviour
    {
        #region Variables
        private Vector2 _cyclic;
        private float _pedals;
        private float _throttle;

        public Vector2 Cyclic { get => _cyclic; }
        public float Pedals { get => _pedals; }
        public float Throttle { get => _throttle; }

        #endregion

        #region Custom Methods
        void OnCyclic(InputValue v) => _cyclic = v.Get<Vector2>();
        void OnPedals(InputValue v) => _pedals = v.Get<float>();
        void OnThrottle(InputValue v) => _throttle = v.Get<float>();
        #endregion
    }
}
