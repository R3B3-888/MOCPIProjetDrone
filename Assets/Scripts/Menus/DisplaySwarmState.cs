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

        private void Update()
        {
            _text.text = $"{_swarm.state}";
        }
    }
}