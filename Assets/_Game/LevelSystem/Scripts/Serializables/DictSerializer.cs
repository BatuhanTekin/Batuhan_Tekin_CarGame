using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.SerializedObjects
{
    [Serializable]
    public class DictSerializer
    {
        [SerializeField, HorizontalGroup("Dict"), DisableInEditorMode] public List<int> Keys;
        [SerializeField, HorizontalGroup("Dict"), DisableInEditorMode] public List<GameObject> Parts;

        public bool TryGetPart(int key, ref GameObject part)
        {
            var index = Keys.IndexOf(key);
            if (index == -1)
            {
                part = null;
                return false;
            }

            part = Parts[index];
            return true;
        }
        
#if UNITY_EDITOR
        [SerializeField, HorizontalGroup("Add"), LabelWidth(48), Space] private int _key;
        [SerializeField, HorizontalGroup("Add"), LabelWidth(48), Space] private GameObject _part;
        
        public bool TryGetKey(GameObject part, out int key)
        {
            key = -1;
            if (part == null) return false;
            var prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(part);
            var index = Parts.IndexOf(prefab);
            if (index == -1) return false;

            key = Keys[index];
            return true;
        }
        
        private bool Add(int key, GameObject part)
        {
            if (part == null || Parts.Contains(part))
            {
                Debug.Log("Part null or already exists!");
                return false;
            }

            if (Keys.Contains(key))
            {
                Debug.Log("Key already exists!");
                return false;
            }

            Keys.Add(key);
            Parts.Add(part);
            return true;
        }

        [Button(ButtonSizes.Medium)]
        private void Add()
        {
            var success = Add(_key, _part);
            if(success) _key++;
        }
        
        [Button(ButtonSizes.Medium)]
        private void AddSelectedAssets()
        {
            var gos = UnityEditor.Selection.gameObjects;
            UnityEditor.Undo.RecordObject(UnityEditor.Selection.activeGameObject, "CoderBulkAddKey");
            for (var i = 0; i < gos.Length; i++)
            {
                var success = Add(_key, gos[i]);
                if(success) _key++;
            }
            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ButtonGroup]
        private void RemoveKey()
        {
            if(! Keys.Contains(_key)) return;
            var index = Keys.IndexOf(_key);
            Keys.RemoveAt(index);
            Parts.RemoveAt(index);
        }
        
        [ButtonGroup]
        private void RemovePart()
        {
            if(_part == null) return;
            if(! Parts.Contains(_part)) return;
            var index = Parts.IndexOf(_part);
            Parts.RemoveAt(index);
            Keys.RemoveAt(index);
        }
        
#endif
    }
}