using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.CarSystem.Scripts
{
    public class PositionSaver
    {
        private List<(Quaternion , Vector3)> _positionList = new();
            
        public void Save((Quaternion , Vector3) data)
        {
            _positionList.Add(data);
        }
        public void Reset()
        {
            _positionList.Clear();
        }

        public (Quaternion , Vector3)[] GetList()
        {
            return _positionList.ToArray();
        }
    }
}