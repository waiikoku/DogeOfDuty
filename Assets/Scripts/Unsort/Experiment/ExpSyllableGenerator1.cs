using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Dod
{
    public class ExpSyllableGenerator1 : MonoBehaviour
    {
        private static readonly List<string> vowels = new List<string> { "a", "e", "i", "o", "u" };
        private static readonly List<string> consonants = new List<string> { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "r", "s", "t", "v", "w", "y", "z" };

        private static string GenerateSyllable(int i)
        {
            Random rand = new Random();
            return GetRandomConsonant(rand) + GetRandomVowel(rand) + GetRandomConsonant(rand);
        }

        private static string GetRandomConsonant(Random rand)
        {
            return consonants[rand.Next(consonants.Count)];
        }

        private static string GetRandomVowel(Random rand)
        {
            return vowels[rand.Next(vowels.Count)];
        }

        public static string GenerateName(int syllableCount)
        {
            if (syllableCount <= 0)
            {
                throw new ArgumentException("Syllable count must be greater than zero.");
            }

            string generatedName = "";

            for (int i = 0; i < syllableCount; i++)
            {
                generatedName += GenerateSyllable(i);
            }

            return generatedName;
        }
    }
}
