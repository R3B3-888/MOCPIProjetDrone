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
        [SerializeField] private Transform _targetPosition;

        private readonly List<GameObject> _drones = new List<GameObject>();

        public List<GameObject> Drones
        {
            get => _drones;
        }

        public GameState State { get; private set; }

        private bool doMoveToOnce { get; set; }

        #endregion

        #region Constructor

        public void SwarmManagerConstructor(GameObject dronePrefab, int numberOfDrone)
        {
            _dronePrefab = dronePrefab;
            _numberOfDrone = numberOfDrone;
        }

        #endregion

        #region Main Methods

        private void Start() => State = GameState.SpawningDrones;

        private void Update()
        {
            switch (State)
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(State), State, null);
            }
        }

        #endregion

        private void HandleSpawningDrones()
        {
            SpawnDrones();
            State = GameState.TakeOff;
            doMoveToOnce = true;
        }

        private void SpawnDrones()
        {
            var offset = -_numberOfDrone / 2;
            for (var i = 0; i < _numberOfDrone; i++)
            {
                var pos = transform.position;
                pos.z += offset + i;
                var drone = Instantiate(_dronePrefab,
                    pos,
                    Quaternion.identity,
                    transform);
                drone.name = $"Drone {i}";
                _drones.Add(drone);
            }
        }

        private void HandleTakeOff()
        {
            if (doMoveToOnce)
                TakeOffDrones(_startingElevation);
            if (!AreAllDronesInRadiusOfWantedPosition()) return;
            State = GameState.OnTheWayIn;
            doMoveToOnce = true;
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

            doMoveToOnce = false;
        }


        public bool AreAllDronesInRadiusOfWantedPosition()
        {
            return _drones.All(drone => drone.GetComponent<DroneController>().IsInRadiusOfWantedPosition());
        }

        private void HandleOnTheWayIn()
        {
            if (doMoveToOnce)
            {
                OnTheWayInDrones();
            }

            if (!AreAllDronesInRadiusOfWantedPosition()) return;
            State = GameState.Monitoring;
            doMoveToOnce = true;
        }

        private void OnTheWayInDrones()
        {
            foreach (var drone in _drones)
            {
                var controller = drone.GetComponent<DroneController>();
                controller.MoveTo(CalculateTargetPosition(drone));
            }

            doMoveToOnce = false;
        }

        public Vector3 CalculateTargetPosition(GameObject drone)
        {
            var targetPosition = new Vector3(725, 61, 496);
            var i = _drones.IndexOf(drone);
            var offset = -_numberOfDrone / 2;
            targetPosition.z += 2 * (offset + i);
            return targetPosition;
        }
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