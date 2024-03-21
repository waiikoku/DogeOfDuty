using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dod
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ExpDatabase1))]
    public class ExpDatabase1Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ExpDatabase1 db = target as ExpDatabase1;
            if (GUILayout.Button("Rearrange"))
            {
                db.Rearrange();
            }
            if (GUILayout.Button("Filter"))
            {
                db.FilterAvailable(null);
            }
            if (GUILayout.Button("RAS"))
            {
                db.RandomAccessSkill();
            }
        }
    }
#endif
}

