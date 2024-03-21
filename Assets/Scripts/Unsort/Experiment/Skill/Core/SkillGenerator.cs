using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Dod
{
    public class SkillGenerator : MonoBehaviour
    {
        public int baseId = 1000;
        public int maxId = 9999;
        public string path;
        public Sprite defaultIcon;
        public Sprite[] icons;

        public int dummyAmount = 10;
        private const string prefix = "Skill_";
        private const string postfix = "";
        private const string APP = "Assets";
        private const string EXTENSION = ".asset";
        private const char SLASH = '/';
        private SkillCard CreateVirtualCard(int id, string name, Sprite icon)
        {
            SkillCard card = ScriptableObject.CreateInstance<SkillCard>();
            card.Identity.ID = id;
            card.Identity.Name = name;
            card.SkillIcon = icon;
            StringBuilder customName = new StringBuilder();
            customName.AppendFormat("{0}{1}{2}", prefix, name, postfix);
            card.name = customName.ToString();
            return card;
        }

        private void CreateAsset(SkillCard card,string appPath)
        {
            string fileName = card.name;
            StringBuilder fullPath = new StringBuilder();
            fullPath.AppendFormat("{0}/{1}{2}", appPath, fileName, EXTENSION);
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(card, fullPath.ToString());
#endif
        }
        #region Random

        private int RandomID()
        {
            return UnityEngine.Random.Range(baseId, maxId);
        }
        private const int ALPHABETSIZE = 26;
        private const int UPPERASCII = 65;
        private const int LOWERASCII = 97;
        private char[] AlphaBets_Uppercase()
        {
            char[] alphabet = new char[ALPHABETSIZE];
            for (int i = 0; i < ALPHABETSIZE; i++)
            {
                alphabet[i] = (char)(i + UPPERASCII); //65 is the offset for capital A in the ascaii table
            }
            return alphabet;
        }

        private char[] AlphaBets_Lowercase()
        {
            char[] alphabet = new char[ALPHABETSIZE];
            for (int i = 0; i < ALPHABETSIZE; i++)
            {
                alphabet[i] = (char)(i + LOWERASCII); //97 is the offset for a in the ascaii table
            }
            return alphabet;
        }

        private char Alphabet(int i)
        {
            return (char)(i + (int)('A'));
        }
        private char Alphabet_Lowercase(int i)
        {
            return (char)(i + (int)('a'));
        }

        private string RandomName()
        {
            return ExpSyllableGenerator1.GenerateName(3);
        }

        private Sprite RandomIcon()
        {
            return defaultIcon;
        }

        private void ManualIteratePath()
        {
            string[] directories = path.Split('/');
            int index = -1;
            for (int i = 0; i < directories.Length; i++)
            {
                if (directories[i] == APP)
                {
                    index = i;
                    break;
                }
            }
            string newPath = "";
            for (int i = index; i < directories.Length; i++)
            {
                newPath += directories[i] + "/";
            }
        }

        public void GenerateDummy()
        {
            string[] directories = path.Split(SLASH);
            int index = Array.IndexOf(directories, APP);
            if (index != -1)
            {
                string newPath = string.Join(SLASH, directories, index, directories.Length - index);
                print(newPath);
                int half = Mathf.RoundToInt((float)dummyAmount / 2f);
                for (int i = 0; i < dummyAmount; i++)
                {
                    var card = CreateVirtualCard(RandomID(), RandomName(), RandomIcon());
                    if (i > half)
                    {
                        card.SkillType = SkillType.Active;
                    }
                    else
                    {
                        card.SkillType = SkillType.Passive;
                    }
                    CreateAsset(card, newPath);
                }
            }
      
        }
        #endregion
    }
}
