using System;
using Assets._Game.InputSystem;
using _Game.CarSystem.Scripts;
using UnityEngine;

namespace Assets._Game.CarSystem
{
    public class PlayerCar : CarBase
    {
        [SerializeField] private InputActionSo m_inputActionSo;

        private bool _isTouchRight;
        private bool _onTouch;
        private PositionSaver _positionSaver;
        private float _time;

        private void Awake()
        {
            _positionSaver = new PositionSaver();
        }

        private void OnEnable()
        {
            m_inputActionSo.AddFinishListener(OnTouchFinish);
            m_inputActionSo.AddTouchListener(OnTouch);
        }

        private void OnDisable()
        {
            m_inputActionSo.RemoveFinishListener(OnTouchFinish);
            m_inputActionSo.RemoveTouchListener(OnTouch);
        }

        public void SetCar(int order)
        {
            InitCar();
            CarBaseData.IsPlayer = true;
            CarBaseData.Order = order;
        }


        protected override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (_transform.position != CarBaseData.StartPosition)
            {
                _positionSaver.Save((_transform.rotation, _transform.position));
            }
        }

        protected override void Move()
        {
            _direction = transform.rotation;
            if (_onTouch)
            {
                var direction = _isTouchRight ? _transform.up : -_transform.up;
                _direction = Quaternion.Euler((direction * m_carDataSo.rotateSpeed) + transform.rotation.eulerAngles);
            }

            base.Move();
        }

        protected override (Quaternion , Vector3)[] GetSavedData()
        {
            return _positionSaver.GetList();
        }

        public override void ResetCar()
        {
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.isKinematic = true;
            _positionSaver.Reset();
        }

        public (Vector3 pos, Quaternion rot) GetStartPosition()
        {
            return (CarBaseData.StartPosition, CarBaseData.StartRotation);
        }

        public override void OnRetry()
        {
            ResetCar();
            base.OnRetry();
        }

        private void OnTouchFinish()
        {
            _onTouch = false;
        }

        private void OnTouch(bool enable)
        {
            _isTouchRight = enable;
            _onTouch = true;
        }
#if UNITY_EDITOR
        public (Quaternion , Vector3)[] Test_GetData()
        {
            return _positionSaver.GetList();
        }
#endif
    }
}