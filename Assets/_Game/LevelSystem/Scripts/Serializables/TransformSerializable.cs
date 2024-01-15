using System;
using UnityEngine;

namespace _game.Scripts.SerializedObjects
{
    [Serializable]
    public struct TransformSerializable
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformSerializable(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }

        public TransformSerializable(TransformSerializable transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.scale;
        }

        public void Apply(Transform transform, Space space = Space.World)
        {
            if (space == Space.World)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            transform.localScale = scale;
        }

        public void ApplyLocal(Transform transform)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
            transform.localScale = scale;
        }

        public TransformSerializable SetLocal(Transform transform)
        {
            position = transform.localPosition;
            rotation = transform.localRotation;
            scale = transform.localScale;
            return this;
        }
    }
}