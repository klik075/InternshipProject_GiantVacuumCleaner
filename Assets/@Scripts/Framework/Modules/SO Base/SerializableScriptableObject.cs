using UnityEditor;
using UnityEngine;

namespace Scripts.Framework.Modules.SO_Base
{
    public abstract class SerializableScriptableObject : ScriptableObject
    {
        #region Fields

        [SerializeField, HideInInspector] private string _guid;
    
        // Property
        public string Guid => _guid;

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            var path = AssetDatabase.GetAssetPath(this);
            _guid = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
}