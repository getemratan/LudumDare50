using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace GameFramework
{
    public class ScriptTemplateMenu : EditorWindow
    {
        static string TEMP_SCRIPT_NAME = "TEMP_SCRIPT_NAME";
        static string TEMP_OBJ_NAME = "TEMP_OBJ_NAME";
        static GameObject _obj;
        static ScriptType _scriptType;
        static string _scriptName;
        static Rect _popupRect;
        static ScriptTemplateMenu popup;

        bool isInitialized;
        bool isScriptCreated;
        bool isDone;

        private void OnLostFocus()
        {
            if (isScriptCreated)
                return;

            Close();
        }

        private void OnGUI()
        {
            if (!isInitialized)
            {
                Vector2 pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                Vector2 size = new Vector2(250, 100);
                _popupRect = new Rect(pos.x - (size.x * 0.5f), pos.y, size.x, size.y);
                popup.position = _popupRect;
                isInitialized = true;
            }

            GUILayout.BeginVertical();
            _scriptName = GUILayout.TextField(_scriptName);
            GUILayout.Space(50);
            if (GUILayout.Button("Create and Add"))
            {
                CreateScript();
            }
            GUILayout.EndVertical();
        }

        private void Update()
        {
            if (isDone)
                return;

            if (HasCompiled())
            {
                AddScript();
            }
        }

        private bool HasCompiled()
        {
            if (isScriptCreated && !EditorApplication.isCompiling)
            {
                return true;
            }
            return false;
        }

        private void AddScript()
        {
            isDone = true;

            MonoScript monoScript = AssetDatabase.LoadAssetAtPath($"Assets/{EditorPrefs.GetString(TEMP_SCRIPT_NAME)}.cs", typeof(MonoScript)) as MonoScript;
            Type type = monoScript.GetClass();

            GameObject.Find(EditorPrefs.GetString(TEMP_OBJ_NAME)).AddComponent(type);

            EditorPrefs.DeleteKey(TEMP_SCRIPT_NAME);
            EditorPrefs.DeleteKey(TEMP_OBJ_NAME);

            Close();
        }

        private void CreateScript()
        {
            if (string.IsNullOrEmpty(_scriptName) || string.IsNullOrWhiteSpace(_scriptName))
                return;

            TextAsset scriptAsset = GetScriptAsset();
            if (scriptAsset == null)
                return;

            string text = scriptAsset.text;
            text = text.Replace("#SCRIPTNAME#", _scriptName);
            text = text.Replace("#NOTRIM#", string.Empty);

            string filePath = $"{Application.dataPath}/{_scriptName}.cs";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(text);
            }

            EditorPrefs.SetString(TEMP_SCRIPT_NAME, _scriptName);
            EditorPrefs.SetString(TEMP_OBJ_NAME, _obj.name);
            AssetDatabase.Refresh();
            isDone = false;
            isScriptCreated = true;
        }

        private TextAsset GetScriptAsset()
        {
            string assetName = string.Empty;
            switch (_scriptType)
            {
                case ScriptType.MonoBehaviour: assetName = "81-C# Script-NewScript.cs"; break;
                case ScriptType.Serializable: assetName = "81-C# Serializable Script-NewScript.cs"; break;
                case ScriptType.Scriptable: assetName = "81-C# Scriptable Object Script-SOScript.cs"; break;
                case ScriptType.Interface: assetName = "81-C# Interface Script-IInterface.cs"; break;
            }
            return AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/ScriptTemplates/{assetName}.txt");
        }

        static void ShowWindow(ScriptType scriptType)
        {
            _obj = Selection.activeGameObject;
            if (_obj == null)
                return;

            _scriptType = scriptType;

            popup = CreateInstance<ScriptTemplateMenu>();
            popup.ShowPopup();
        }

        [MenuItem("Component/Script Templates/New Script", false, 100)]
        static void NewScript()
        {
            ShowWindow(ScriptType.MonoBehaviour);
        }

        [MenuItem("Component/Script Templates/New Serializable Script", false, 100)]
        static void NewSerializableScript()
        {
            ShowWindow(ScriptType.Serializable);
        }

        [MenuItem("Component/Script Templates/New ScriptableObject", false, 100)]
        static void NewScriptableObject()
        {
            ShowWindow(ScriptType.Scriptable);
        }

        [MenuItem("Component/Script Templates/New Interface", false, 100)]
        static void NewInterface()
        {
            ShowWindow(ScriptType.Interface);
        }
    }

    public enum ScriptType
    {
        MonoBehaviour,
        Serializable,
        Scriptable,
        Interface,
    }
}