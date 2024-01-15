using System;
using _game.Scripts.SerializedObjects;

namespace _game.LevelSystem.Serializables
{
    [Serializable]
    public class ObjectData
    {
        public int key;
        public TransformSerializable transformSerializable;
        public string Data;
    }
}