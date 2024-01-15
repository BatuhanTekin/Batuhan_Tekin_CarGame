using System;
using UnityEngine;

namespace Assets._Game.LevelSystem
{
    [Serializable]
    public class CarLevelData
    {
        public Vector3 StartPosition;
        public Quaternion StartRotation;
        public int Order;
    }
}
