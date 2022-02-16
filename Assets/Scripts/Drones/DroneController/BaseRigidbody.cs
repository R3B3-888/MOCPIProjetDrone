using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drones
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseRigidbody : MonoBehaviour
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
            {
                rb.mass = _weight;
                rb.drag = 0.5f;
            }
        }
        private void FixedUpdate()
        {
            if(!rb)
                return;
            HandlePhysics();
        }
        protected virtual void HandlePhysics() { }
        #endregion
    }
}
