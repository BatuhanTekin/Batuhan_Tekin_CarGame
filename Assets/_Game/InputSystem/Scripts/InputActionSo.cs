using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Game.InputSystem
{
    [CreateAssetMenu(fileName = "InputActionSo", menuName = "ScriptableObjects/Input/InputActionSo", order = 1)]
    public class InputActionSo : ScriptableObject
    {
        public event Action OnInputFinish = delegate { };
        public event Action<bool> OnInputTouch = delegate { };

        public void AddTouchListener(Action<bool> action)
        {
            OnInputTouch += action;
        }

        public void AddFinishListener(Action action)
        {
            OnInputFinish += action;
        }

        public void RemoveTouchListener(Action<bool> action) 
        {
            OnInputTouch -= action;
        }

        public void RemoveFinishListener(Action action)
        {
            OnInputFinish -= action;
        }

        public void InvokeTouch(bool isRight)
        {
            OnInputTouch?.Invoke(isRight);
        }

        public void InvokeFinish()
        {
            OnInputFinish?.Invoke();
        }

        public void Clear()
        {
            OnInputFinish = delegate { };
            OnInputTouch = delegate { };
        }
    }
}
