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
        [SerializeField, Range(1, 10)] private int _numberOfDrone = 5;
        [SerializeField, Range(2, 20)] private int _startingElevation = 2;
        [SerializeField] private Vector3 _targetPosition = new Vector3(725, 60, 500);
        [SerializeField] private bool _onStandbyAfterSpawn;
        [SerializeField] private Transform _swimmers;
        [SerializeField] private LayoutType _layout;
        [SerializeField] private float _distanceBetweenDronesLayout = 15f;
        private bool _onDeploy = false;
        private List<Drone> _dronesNotOnPositionYet = new List<Drone>();

        public List<Drone> drones { get; } = new List<Drone>();
        public GameState state { get; private set; }
        private bool getComponentsOnce { get; set; } = true;

        #endregion

        #region Constructor

        public void SwarmManagerConstructor(GameObject dronePrefab, int numberOfDrone, Vector3 targetPosition,
            float distanceBetweenDronesLayout, bool onStandByAfterSpawn = false)
        {
            _dronePrefab = dronePrefab;
            _numberOfDrone = numberOfDrone;
            _targetPosition = targetPosition;
            _distanceBetweenDronesLayout = distanceBetweenDronesLayout;
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
                case GameState.Repositioning: // TODO : if a drone is crashing or come closer to someone
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
            state = !_onStandbyAfterSpawn ? GameState.TakeOff : GameState.Standby;
        }

        private void SpawnDrones()
        {
            var offset = -_numberOfDrone / 2;
            for (uint i = 0; i < _numberOfDrone; i++)
            {
                var transform1 = transform;
                var pos = transform1.position;
                pos.z += offset + i;
                var droneInstance = Instantiate(_dronePrefab,
                    pos,
                    Quaternion.identity,
                    transform1);
                droneInstance.name = $"Drone {i}";
                var d = new Drone(droneInstance, i);
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
                TakeOffDronesAt(_startingElevation);
            if (!drones.All(drone => drone.IsInRadiusOfWantedPosition())) return;
            state = GameState.OnTheWayIn;
            getComponentsOnce = true;
        }

        private void TakeOffDronesAt(int elevation)
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

        #region GameState.OnTheWayIn

        private void HandleOnTheWayIn()
        {
            if (getComponentsOnce)
            {
                foreach (var drone in drones)
                    _dronesNotOnPositionYet.Add(drone);
                OnTheWayInDrones();
            }

            foreach (var drone in drones)
                if (_dronesNotOnPositionYet.Contains(drone) && drone.IsInRadiusOfWantedPosition())
                {
                    drone.Stabilize();
                    _dronesNotOnPositionYet.Remove(drone);
                }

            if (_dronesNotOnPositionYet.Count != 0) return;
            state = GameState.Monitoring;
            getComponentsOnce = true;
        }

        private void OnTheWayInDrones()
        {
            if (_swimmers != null) _targetPosition = _swimmers.position;
            foreach (var drone in drones)
                drone.MoveTo(
                    drone.CalculateTargetPosition(
                        (uint) drones.Count,
                        _targetPosition,
                        _distanceBetweenDronesLayout,
                        _layout
                    ));

            getComponentsOnce = false;
        }

        #endregion

        #region GameState.Monitoring

        private void HandleMonitoring()
        {
            // TODO: foreach drone, TurnTowards target
            // TODO: TargetCamera calculé par rapport à son index
            // TODO: the camera surveillance   
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
    Crashing = 7,
    Repositioning = 8
}