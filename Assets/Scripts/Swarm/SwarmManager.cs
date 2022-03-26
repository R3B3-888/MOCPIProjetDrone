using System;
using System.Collections.Generic;
using System.Linq;
using Drones;
using UnityEngine;

namespace Swarm
{
    public class SwarmManager : MonoBehaviour
    {
        #region Variables

        // Drone Settings
        [SerializeField] private GameObject _dronePrefab;
        [SerializeField, Range(1, 10)] private int _numberOfDrones = 5;

        // Target Position params
        [SerializeField] private LayoutType _layout;
        [SerializeField] private Vector3 _targetPosition = new Vector3(725, 60, 500);
        [SerializeField] private Transform _targetSwimmers;
        [SerializeField] private float _areaLength = 60;
        private Vector2 _distanceFromTarget = new Vector2(30f, 15f);

        // States params
        [SerializeField, Range(2, 20)] private int _startingElevation = 2;
        [SerializeField] private bool _onStandbyAfterSpawn;

        // Optimizations locks
        private bool _isMoveToNotDoneYet = true, _needToCalibrateOnce = true;

        private readonly List<Drone> _dronesNotOnPositionYet = new List<Drone>();
        public List<Drone> dronesLost { get; } = new List<Drone>();
        public List<Drone> drones { get; } = new List<Drone>();
        public GameState state { get; private set; }
        public LayoutType layout => _layout;

        #endregion

        #region Constructor

        public void SwarmManagerConstructor(GameObject dronePrefab, int numberOfDrone, Vector3 targetPosition,
            Vector2 distanceFromTarget, bool onStandByAfterSpawn = false)
        {
            _dronePrefab = dronePrefab;
            _numberOfDrones = numberOfDrone;
            _targetPosition = targetPosition;
            _distanceFromTarget = distanceFromTarget;
            _onStandbyAfterSpawn = onStandByAfterSpawn;
            _layout = LayoutType.Line;
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
                case GameState.Repositioning:
                    HandleRepositioning();
                    break;
                case GameState.OnTheWayBack: // TODO : All drones rotate 180° and went back on platform
                case GameState.Landing: // TODO : Like TakeOff by from elevation to platform ground
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
            var offset = -_numberOfDrones / 2;
            for (var i = 0; i < _numberOfDrones; i++)
            {
                var transform1 = transform;
                var pos = transform1.position;
                pos.z += offset + i;
                var droneInstance = Instantiate(_dronePrefab,
                    pos,
                    Quaternion.identity,
                    transform1);
                var d = new Drone(droneInstance, id: i);
                drones.Add(d);
            }
        }

        #endregion

        #region GameState.Standby

        private void HandleStandby()
        {
            // Waiting for user to raise OnDeploy
        }

        public void OnDeploy() => state = GameState.TakeOff;

        #endregion

        #region GameState.TakeOff

        private void HandleTakeOff()
        {
            if (_isMoveToNotDoneYet)
            {
                TakeOffDronesAt(_startingElevation);
                _isMoveToNotDoneYet = false;
            }

            if (!drones.All(drone => drone.IsInRadiusOfWantedPosition())) return;

            _isMoveToNotDoneYet = true;
            state = GameState.OnTheWayIn;
        }

        private void TakeOffDronesAt(int elevation)
        {
            foreach (var drone in drones)
            {
                var pos = drone.Position();
                pos.y += elevation;
                drone.MoveTo(pos);
            }
        }

        #endregion

        #region GameState.OnTheWayIn

        private void HandleOnTheWayIn()
        {
            if (_isMoveToNotDoneYet)
            {
                foreach (var drone in drones)
                    _dronesNotOnPositionYet.Add(drone);
                DronesMoveToTheirTargetPosition();
                _isMoveToNotDoneYet = false;
            }

            foreach (var drone in drones)
                if (_dronesNotOnPositionYet.Contains(drone) && drone.IsInRadiusOfWantedPosition())
                {
                    drone.Stabilize();
                    _dronesNotOnPositionYet.Remove(drone);
                }

            if (_dronesNotOnPositionYet.Count != 0) return; // While all drone are not in position
            _isMoveToNotDoneYet = true;
            _needToCalibrateOnce = true;
            state = GameState.Monitoring;
        }

        private void DronesMoveToTheirTargetPosition()
        {
            if (_targetSwimmers != null) _targetPosition = _targetSwimmers.position;

            // TRICKY : offset on z axis for the calculation of the line layout
            var targetPosition = new Vector3(_targetPosition.x, _targetPosition.y,
                _targetPosition.z - (_layout == LayoutType.Line ? _areaLength / 2 : 0));

            foreach (var drone in drones)
                drone.MoveTo(
                    drone.CalculateTargetPosition(
                        (uint) drones.Count,
                        targetPosition,
                        _distanceFromTarget,
                        _areaLength,
                        _layout
                    ));
        }

        #endregion

        #region GameState.Monitoring

        private void HandleMonitoring()
        {
            if (_needToCalibrateOnce is false) return;
            foreach (var drone in drones)
                drone.CalibrateTargetCamera(targetPosition:
                    new Vector3(
                        _targetPosition.x,
                        _targetPosition.y - _distanceFromTarget.y * .5f,
                        _targetPosition.z));
            _needToCalibrateOnce = false;
        }

        #endregion

        #region GameState.Repositioning

        private void HandleRepositioning()
        {
            CrashManager();

            foreach (var drone in drones)
                drone.Destabilize();

            state = GameState.OnTheWayIn;
        }

        private void CrashManager()
        {
            Drone droneOnCrashing = null;
            foreach (var drone in drones.Where(drone => !drone.IsStillFlying()))
            {
                dronesLost.Add(drone);
                droneOnCrashing = drone;
                break;
            }

            if (droneOnCrashing == null) return;
            drones.Remove(droneOnCrashing);
            
            _isMoveToNotDoneYet = true;
            foreach (var drone in drones)
                drone.UpdateRankInSwarm(drones.IndexOf(drone));
        }

        #endregion

        #region Event Methods

        // Warning !! : id != index of drones list
        // id is equal to a drone.id
        public void OnCrashing(int id)
        {
            if (dronesLost.Any(drone => drone.id == id)) return;

            foreach (var drone in drones.Where(drone => id == drone.id))
                StartCoroutine(drone.Crash());

            state = GameState.Repositioning;
        }

        public void OnChangingLayout(int layoutTypeIndex)
        {
            _layout = (LayoutType) layoutTypeIndex;
            if (state == GameState.Standby || state == GameState.SpawningDrones) return;
            state = GameState.Repositioning;
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
    Repositioning = 7
}