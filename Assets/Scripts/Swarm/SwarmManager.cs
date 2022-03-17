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
        [SerializeField] private Vector3 _targetPosition = new Vector3(725, 60, 500);

        public List<Drone> drones { get; } = new List<Drone>();
        public GameState state { get; private set; }
        private bool getComponentsOnce { get; set; } = true;

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
                case GameState.OnTheWayBack:// TODO
                case GameState.Landing:     // TODO
                case GameState.Crashing:    // TODO
                case GameState.Standby:     // TODO
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
                var droneInstance = Instantiate(_dronePrefab,
                    pos,
                    Quaternion.identity,
                    transform1);
                droneInstance.name = $"Drone {i}";
                var d = new Drone(droneInstance);
                drones.Add(d);
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
            foreach (var drone in drones)
            {
                var pos = drone.Position();
                pos.y += elevation;
                drone.MoveTo(pos);
            }

            getComponentsOnce = false;
        }

        #endregion


        private bool AreAllDronesInRadiusOfWantedPosition() => drones.All(drone => drone.IsInRadiusOfWantedPosition());

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
            foreach (var drone in drones)
                drone.MoveTo(
                    drone.CalculateTargetPosition(
                        drones,
                        _targetPosition
                    ));

            getComponentsOnce = false;
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
            foreach (var drone in drones)
                drone.Stabilize();

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