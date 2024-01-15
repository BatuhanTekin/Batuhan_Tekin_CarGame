using UnityEngine;

namespace _Game.CarSystem.Scripts
{
    public class AutomaticCar : CarBase
    {
        private (Quaternion , Vector3)[] _savedData;
        private int _nodeOrder;
        private (Quaternion, Vector3) _node;

        public void SetCar((Quaternion , Vector3)[] savedData, (Vector3 pos, Quaternion rot) startPosition)
        {
            transform.SetPositionAndRotation(startPosition.pos, startPosition.rot);
            _savedData = savedData;
            InitCar();
        }

        public override void ResetCar()
        {
            base.ResetCar();
            _nodeOrder = 0;
        }

        public override void OnRetry()
        {
            base.OnRetry();
            _nodeOrder = 0;
        }


        protected override void Move()
        {
            if (_nodeOrder >= _savedData.Length - 1)
            {
                StopCar(true);
                return;
            }

            _node = _savedData[_nodeOrder];
            _transform.SetPositionAndRotation(_node.Item2, _node.Item1);
            _nodeOrder++;
        }
    }
}