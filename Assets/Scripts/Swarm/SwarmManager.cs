using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Swarm
{
    public class SwarmManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject _dronePrefab;
        [SerializeField] private int _numberOfDrone;

        private List<GameObject> _drones = new List<GameObject>();

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