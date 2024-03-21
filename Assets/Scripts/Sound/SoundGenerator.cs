using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Dod;

public class SoundGenerator : MonoBehaviour
{
    private string ToConstantName(string name)
    {
        return string.Join("", name.Split(new char[] { ' ', '-' })).ToUpper();
    }

    private static string GeneratePreloadBundleConstClass(Dictionary<int, SoundAlbum> albums)
    {
        var generatedCode = "// GENERATED FILE. DO NOT EDIT.\n\n";
        //generatedCode += "using System.Collections.Generic;\n";
        generatedCode += "public class Soundbank\n{\n"; //begin class

        foreach (var album in albums)
        {
            SoundAlbum sound = album.Value;
            generatedCode += string.Format("\tpublic enum {0}", sound.albumName);
            generatedCode += " \n\t{\n";
            foreach (var s in sound.sounds)
            {
                generatedCode += string.Format("\t\t{0},\n", s.Name);
            }
            generatedCode += "\t}\n";
        }

        generatedCode += "\n}\n"; //end class
        return generatedCode;
    }

    public static void GenerateClass(Dictionary<int,SoundAlbum> albums, System.Action callback = null)
    {
        var code = GeneratePreloadBundleConstClass(albums);
        var path = Application.dataPath;
        var fileName = "/Soundbank.cs";
        var fullpath = path + fileName;
        Debug.Log(fullpath);
        File.WriteAllText(fullpath, code);
        callback?.Invoke();
    }
}