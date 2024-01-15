using _game.Scripts.SerializedObjects;
using UnityEngine;

namespace _game.Scripts.SOs
{
    [CreateAssetMenu(fileName = "PoolingKeyStore", menuName = "ScriptableObjects/Level/PoolingKeyStore", order = 1)]
    public class PoolingKeyStore : ScriptableObject
    {
        public ObjectType Type;
        public DictSerializer _serializer;
    }

    public enum ObjectType
    {
        Obstacle = 0,
    }
}