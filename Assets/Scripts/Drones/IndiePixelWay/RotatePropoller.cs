using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RotatePropoller
{
    public class RotatePropoller : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Transform propeller;
        [SerializeField] private float _rotationSpeed = 50f;
        #endregion

        #region Main Methods
        void Update()
        {
            propeller.Rotate(Vector3.up, _rotationSpeed);
        }
        #endregion
    }
}
