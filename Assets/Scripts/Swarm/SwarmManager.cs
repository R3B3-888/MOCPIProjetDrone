using System;
using System.Collections.Generic;
using System.Linq;
using Drones;
using JetBrains.Annotations;
using UnityEngine;

namespace Swarm
{
    public class SwarmManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _dronePrefab;
        [SerializeField] private int _numberOfDrone;

        [SerializeField, Range(2, 20)] private int _startingElevation = 10;
        private readonly List<GameObject> _drones = new List<GameObject>();

        public List<GameObject> Drones => _drones;
        public GameState State { get; private set; }
        

        #endregion

        #region Constructor

        public void SwarmManagerConstructor(GameObject dronePrefab, int numberOfDrone)
        {
            _dronePrefab = dronePrefab;
            _numberOfDrone = numberOfDrone;
        }

        #endregion

        #region Main Methods

        void Start() => ChangeState(GameState.SpawningDrones);

        #endregion

        private void ChangeState(GameState newState)
        {
            State = newState;
            switch (newState)
            {
                case GameState.SpawningDrones:
                    HandleSpawningDrones();
                    break;
                case GameState.TakeOff:
                    HandleTakeOff();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }


        private void HandleSpawningDrones()
        {
            SpawnDrones();
            ChangeState(GameState.TakeOff);
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
            TakeOffDrones(elevation: _startingElevation);
            if (AllDronesAreAtWantedPosition())
                ChangeState(GameState.OnTheWayIn);
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
        }

        private bool AllDronesAreAtWantedPosition()
        {
            return _drones.All(drone => drone.GetComponent<DroneController>().IsInRadiusOfWantedPosition());
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
    Crashing = 7
}