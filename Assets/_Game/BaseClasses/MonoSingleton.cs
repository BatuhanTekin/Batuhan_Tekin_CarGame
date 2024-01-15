using UnityEngine;

namespace Assets._Game.BaseClasses
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                if (_instance == null)
                {
                    Debug.LogError($"Instance Couldn't Find {typeof(T)}");
                }

                return _instance;
            }
        }
    }
}
