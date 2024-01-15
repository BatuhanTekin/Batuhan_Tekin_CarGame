using System;
using UnityEngine;

namespace Assets._Game.InputSystem
{
    public class InputController : MonoBehaviour
    {

        [SerializeField] private InputActionSo m_inputAction;
        private float _screenCenterX;

        private void Start()
        {
            _screenCenterX = Screen.width * 0.5f;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_inputAction.InvokeTouch(Input.mousePosition.x > _screenCenterX);
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_inputAction.InvokeFinish();
                return;
            }
        }
    }
}
