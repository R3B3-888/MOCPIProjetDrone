using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeachObjects
{
    public class Swarm
    {
        private int _id;
        private List<object> _droneList;

        public Swarm(int id)
        {
            _id = id;
            _droneList = new List<object>();

            for (int i = 0; i < 4; i++)
            {
                object newDrone = new object();
                _droneList.Add(newDrone);
            }
        }
    }
}
