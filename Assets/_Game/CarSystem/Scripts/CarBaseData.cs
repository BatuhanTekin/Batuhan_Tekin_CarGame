using System;
using UnityEngine;

namespace _Game.CarSystem.Scripts
{
    public struct CarBaseData
    {
        public bool IsPlayer;
        public Vector3 StartPosition;
        public Quaternion StartRotation;
        public int Order;
    }
}