using System;
using Swarm;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class DisplaySwarmState : MonoBehaviour
    {
        [SerializeField] private SwarmManager _swarm;
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _sideMenu;

        private void Awake()
        {
            _text.text = "Spawning";
        }

        private void Update()
        {
            // if (_sideMenu.GetComponent<SideMenu>().isShown is false) return;
            var stateInString = "";
            switch (_swarm.state)
            {
                case GameState.SpawningDrones:
                    stateInString = "SpawningDrones";
                    break;
                case GameState.Standby:
                    stateInString = "Standby";
                    break;
                case GameState.TakeOff:
                    stateInString = "TakeOff";
                    break;
                case GameState.OnTheWayIn:
                    stateInString = "On The Way In";
                    break;
                case GameState.Monitoring:
                    stateInString = "Monitoring";
                    break;
                case GameState.OnTheWayBack:
                    stateInString = "On The Way Back";
                    break;
                case GameState.Landing:
                    stateInString = "";
                    break;
                case GameState.Crashing:
                    stateInString = "";
                    break;
                case GameState.Repositioning:
                    stateInString = "Repositioning";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _text.text = stateInString;
        }
    }
}