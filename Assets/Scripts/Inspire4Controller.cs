using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

namespace DroneController
{
    public class Inspire4Controller : MonoBehaviour
    {
        #region Variables
        private GameObject targetObj;
        private GameObject cam;
        
        public float speedCam = 1;
        private Animator animOpen;
        private float speedRigthLeft = 0;
        private float speedFrontBack = 0;
        private float speedUpDown = 0;
        public float maxSpeed = 3.0f;
        public float maxSpeedRotateLR = 80f;
        public float maxAngleUI = 10;
        private float angleRightleft = 0;
        private float rotateLR = 0;
        private float angleFrontBack = 0;
        float minSpeed = 0;
        float minAngle = 0.0f;
        public float acceleration = 0.01f;
        public float accelerationUI = 1.0f;
        public float rotateAccel = 1.5f;
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
        
        #endregion
    }
}
