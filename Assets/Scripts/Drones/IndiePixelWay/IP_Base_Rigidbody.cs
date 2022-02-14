using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixelWay
{
    [RequireComponent(typeof(Rigidbody))]
    public class IP_Base_Rigidbody : MonoBehaviour
    {
        #region Variables
        [Header("Rigidbody Properties")]
        [SerializeField] private float _weight = 0.25f;
        protected Rigidbody rb;
        #endregion

        #region Methods
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb)
                rb.mass = _weight;
        }
        private void FixedUpdate()
        {
            if (!rb)
                return;
            HandlePhysics();
        }
        protected virtual void HandlePhysics() { }
        #endregion
    }
}
