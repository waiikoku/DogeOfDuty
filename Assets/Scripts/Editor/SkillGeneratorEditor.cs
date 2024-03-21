using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dod
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SkillGenerator))]
    public class SkillGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SkillGenerator sg = target as SkillGenerator;
            if (GUILayout.Button("Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Browse Path", Application.dataPath, "");
                if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                {
                    return;
                }
                sg.path = path;
            }
            if (GUILayout.Button("Generate (Single)"))
            {

            }
            if (GUILayout.Button("Generate (Dummy)"))
            {
                sg.GenerateDummy();
            }
        }
    }
#endif
}
