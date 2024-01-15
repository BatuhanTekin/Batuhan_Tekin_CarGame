using _game.Scripts.LevelSystem;
using UnityEngine;

namespace _game.LevelSystem.Serializables
{
    public class LevelObjectBase : MonoBehaviour, IObjectData, IResetable
    {
        protected ObjectData _objectData = new ObjectData(); 
        public virtual ObjectData GetObjectData()
        {
            return _objectData;
        }

        public virtual void SetData(string data)
        {
            _objectData.Data = data;
        }

        public virtual void SetData()
        {
            
        }

        public virtual void LoadData(string data)
        {
            _objectData.Data = data;
        }

        public virtual void ResetObject()
        {
            
        }
    }
}