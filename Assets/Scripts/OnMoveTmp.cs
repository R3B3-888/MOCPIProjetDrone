using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class OnMoveTmp : MonoBehaviour
    {
        #region Variables
        
        #endregion

        #region Main Methods
        void Start()
        {
            
        }

        void Update()
        {
            
        }
        #endregion

        #region Custom Methods
        private void OnCyclic(InputValue v)
	{
		Debug.Log(v.Get<Vector2>());
	}
        #endregion
	  
    }

