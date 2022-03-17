using System;
using System.Collections.Generic;
using System.Linq;
using Drones;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Swarm
{
    public class SwarmManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _dronePrefab;
        [SerializeField] private int _numberOfDrone = 5;

        [SerializeField, Range(2, 20)] private int _startingElevation = 2;

        // [SerializeField] private Transform _targetPosition;
        [SerializeField] private Vector3 _targetPosition = new Vector3(725, 60, 500);

        private readonly List<GameObject> _drones = new List<GameObject>();

        public List<GameObject> Drones
        {
            get => _drones;
        }

        public GameState state { get; private set; }
        private bool getComponentsOnce { get; set; }

        #endregion

        #region Constructor

        public void SwarmManagerConstructor(GameObject dronePrefab, int numberOfDrone, Vector3 targetPosition)
        {
            _dronePrefab = dronePrefab;
            _numberOfDrone = numberOfDrone;
            _targetPosition = targetPosition;
        }

        #endregion

        #region Main Methods

        private void Start() => state = GameState.SpawningDrones;

        private void Update()
        {
            switch (state)
            {
                case GameState.SpawningDrones:
                    HandleSpawningDrones();
                    break;
                case GameState.TakeOff:
                    HandleTakeOff();
                    break;
                case GameState.OnTheWayIn:
                    HandleOnTheWayIn();
                    break;
                case GameState.Monitoring:
                    HandleMonitoring();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion

        #region GameState.SpawningDrones

        private void HandleSpawningDrones()
        {
            SpawnDrones();
            state = GameState.TakeOff;
            getComponentsOnce = true;
        }

        private void SpawnDrones()
        {
            var offset = -_numberOfDrone / 2;
            for (var i = 0; i < _numberOfDrone; i++)
            {
                var transform1 = transform;
                var pos = transform1.position;
                pos.z += offset + i;
                var drone = Instantiate(_dronePrefab,
                    pos,
                    Quaternion.identity,
                    transform1);
                drone.name = $"Drone {i}";
                _drones.Add(drone);
            }
        }

        #endregion

        #region GameState.TakeOff

        private void HandleTakeOff()
        {
            if (getComponentsOnce)
                TakeOffDrones(_startingElevation);
            if (!AreAllDronesInRadiusOfWantedPosition()) return;
            state = GameState.OnTheWayIn;
            getComponentsOnce = true;
        }

        private void TakeOffDrones(int elevation)
        {
            foreach (var drone in _drones)
            {
                var controller = drone.GetComponent<DroneController>();
                var pos = drone.transform.position;
                pos.y += elevation;
                controller.MoveTo(pos);
            }

            getComponentsOnce = false;
        }

        #endregion


        private bool AreAllDronesInRadiusOfWantedPosition() => _drones.All(drone =>
            drone.GetComponent<DroneController>().IsInRadiusOfWantedPosition());

        #region GameState.OnTheWayIn

        private void HandleOnTheWayIn()
        {
            if (getComponentsOnce)
                OnTheWayInDrones();

            if (!AreAllDronesInRadiusOfWantedPosition()) return;
            state = GameState.Monitoring;
            getComponentsOnce = true;
        }

        private void OnTheWayInDrones()
        {
            foreach (var drone in _drones)
            {
                var controller = drone.GetComponent<DroneController>();
                controller.MoveTo(CalculateTargetPosition(drone));
            }

            getComponentsOnce = false;
        }

        public Vector3 CalculateTargetPosition(GameObject drone)
        {
            var targetPosition = _targetPosition;
            var i = _drones.IndexOf(drone);
            var offset = -_numberOfDrone / 2;
            targetPosition.z += (offset + i);
            return targetPosition;
        }

        #endregion

        #region GameState.Monitoring

        private void HandleMonitoring()
        {
            if (getComponentsOnce)
                StabilizedOnDrones();
            // TODO:the camera surveillance   
        }

        private void StabilizedOnDrones()
        {
            foreach (var drone in _drones)
            {
                var controller = drone.GetComponent<DroneController>();
                controller.Stabilize();
            }

            getComponentsOnce = false;
        }

        #endregion
    }
}

[Serializable]
public enum GameState
{
    SpawningDrones = 0,
    TakeOff = 1,
    OnTheWayIn = 2,
    Monitoring = 3,
    OnTheWayBack = 4,
    Landing = 6,
    Crashing = 7,
    Standby = 8
}