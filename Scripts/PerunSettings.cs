using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PerunDrawer
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class PerunSettings : ScriptableObject
    {
        private static PerunSettings _instance;
        private static PerunSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load("PerunSettings") as PerunSettings;
                if (_instance == null)
                    _instance = CreateInstance<PerunSettings>();
                return _instance;
            }
        }
        
        [SerializeField]
        private bool _enabled;
        public static bool Enabled { get { return Instance._enabled; } }
        
        [MenuItem("Window/PerunDrawer/Enabled %&h", false, -101)]
        static void PerunSettingsEnabled() 
        {
            Instance._enabled = !Instance._enabled;
            RepaintInspector();
        }
        
        public static void RepaintInspector()
        {
            Editor[] ed = Resources.FindObjectsOfTypeAll<Editor>();
            for (int i = 0; i < ed.Length; i++)
                ed[i].Repaint();
        }
/*
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/PerunSettings")]
        public static void CreateCommandRoseElementsLibrary()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") 
                path = "Assets";
            else if (Path.GetExtension(path) != "") 
                path = path.Replace(Path.GetFileName(path), "");
            path += Path.DirectorySeparatorChar + "PerunSettings.asset";
            
            var asset = CreateInstance<PerunSettings>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path); 
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            _instance = null;
        }
#endif
*/
    }
}
