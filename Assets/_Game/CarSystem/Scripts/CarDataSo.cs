using UnityEngine;

namespace Assets._Game.CarController
{
    [CreateAssetMenu(fileName = "CarDataSo", menuName = "ScriptableObjects/Car/CarDataSo", order = 1)]
    public class CarDataSo : ScriptableObject
    {
        public float moveSpeed;
        public float rotateSpeed;
        public float accelerationTime;
        public AnimationCurve accelerationCurve;
    }
}
