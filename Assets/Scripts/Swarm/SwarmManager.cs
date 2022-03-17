using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        [SerializeField] private bool _onStandbyAfterSpawn;
        private bool _onDeploy = false;

        public List<Drone> drones { get; } = new List<Drone>();
        public GameState state { get; private set; }
        private bool getComponentsOnce { get; set; } = true;

        #endregion

        #region Constructor

        public void SwarmManagerConstructor(GameObject dronePrefab, int numberOfDrone, Vector3 targetPosition,
            bool onStandByAfterSpawn = false)
        {
            _dronePrefab = dronePrefab;
            _numberOfDrone = numberOfDrone;
            _targetPosition = targetPosition;
            _onStandbyAfterSpawn = onStandByAfterSpawn;
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
                case GameState.Standby:
                    HandleStandby();
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
                case GameState.OnTheWayBack: // TODO
                case GameState.Landing: // TODO
                case GameState.Crashing: // TODO
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion

        #region GameState.SpawningDrones

        private void HandleSpawningDrones()
        {
            SpawnDrones();
            if (_onStandbyAfterSpawn == false)
                state = GameState.TakeOff;
            else
                state = GameState.Standby;
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

        #region GameState.Standby

        private void HandleStandby()
        {
            if (_onStandbyAfterSpawn == false)
                state = GameState.TakeOff;
            // waiting for user to activate The deployment
            if (_onDeploy)
                state = GameState.TakeOff;
        }

        public void OnDeploy() => _onDeploy = true;

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
    Standby = 1,
    TakeOff = 2,
    OnTheWayIn = 3,
    Monitoring = 4,
    OnTheWayBack = 5,
    Landing = 6,
    Crashing = 7
}