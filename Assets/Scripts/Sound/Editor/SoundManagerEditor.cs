using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dod
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SoundManager db = (SoundManager)target;
            if (GUILayout.Button("Update"))
            {
                Dictionary<int, SoundAlbum> albums = new Dictionary<int, SoundAlbum>(3);
                SoundAlbum[] ab = db.GetAlbums();
                albums.Add(0, ab[0]);
                albums.Add(1, ab[1]);
                albums.Add(2, ab[2]);
                SoundGenerator.GenerateClass(albums, () =>
                {
                    AssetDatabase.Refresh();
                });
            }
        }
    }
}
