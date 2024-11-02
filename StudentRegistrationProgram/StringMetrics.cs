using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistrationProgram
{
    public static class StringMetrics
    {

        /*
         To succesfully compare two strings and check for similarity, one has to use a string metric:
         a metric that measures distance between two text strings for approximate string matching or comparison.

         There are many, and my choice went between the Levenshtein Distance and Jaro-Winkler Similarity.

         Levenshtein distance seemed too complicated, and I wouldn't have had the time to implement it, so I went with
         Jaro-Winkler Similarity. Jaro-Winkler similarity is the Jaro Similarity multiplied with a prefix scale.

         https://en.wikipedia.org/wiki/Jaro%E2%80%93Winkler_distance

         Here are implemented two versions, an efficient one, built from exemples from internet, and a custom one, way
         */

        public static double JaroWinklerSimilarity(string first, string second)
        {
            first = first.ToLower();
            second = second.ToLower();

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
                return 0;

            double jaroSimilarity = CustomJaroSimilarity(first, second);
            int prefixLength = GetCommonPrefixLength(first, second);

            const double prefixScalingFactor = 0.07;

            return jaroSimilarity + (prefixLength * prefixScalingFactor * (1 - jaroSimilarity));
        }


        public static double CustomJaroSimilarity(string s1, string s2)
        {

            int matchingWindow = (Math.Max(s1.Length, s2.Length)/2) -1;

            StringBuilder matchedString1 = new StringBuilder();
            StringBuilder matchedString2 = new StringBuilder();

            char[] matchedCharacters1 = new char[s1.Length];
            char[] matchedCharacters2 = new char[s2.Length];

            bool[] matchedMap2 = new bool[s2.Length];

            int numberMatches=0;

            for(int i = 0; i<s1.Length; i++)
            {
                for (int j = 0; j<s2.Length; j++)
                {
                    if (matchedMap2[j]) continue;

                    if (s1[i] == s2[j] && IsWithinWindow(i,j,matchingWindow))
                    {
                        matchedString1.Append(s1[i]);
                        matchedMap2[j] = true;
                        numberMatches++;
                        break;
                    }
                }
            }

            if (numberMatches == 0)
                return 0;

            for(int i = 0; i<s2.Length; i++)
            {
                if (matchedMap2[i])
                    matchedString2.Append(s2[i]);
            }

            int count = matchedString1.Length;
            int numberTranspositions = 0;

            for(int i = 0; i < count; i++)
            {
                if (matchedString1[i] != matchedString2[i])
                    numberTranspositions++;
            }


            return ((numberMatches / (double)s1.Length) +
                    (numberMatches / (double)s2.Length) +
                    ((numberMatches - numberTranspositions / 2.0) / numberMatches)) / 3.0;
        }

        public static bool IsWithinWindow(int position1, int position2, int window)
        {
            if (position2 > position1 - window && position2 < position1 + window+1)
                return true;

            return false;
        }

        // Efficient version
        private static double GetJaroDistance(string s1, string s2)
        {
            int length1 = s1.Length;
            int length2 = s2.Length;

            if (length1 == 0 || length2 == 0)
                return 0.0;

            int matchDistance = Math.Max(length1, length2) / 2 - 1;

            bool[] s1Matches = new bool[length1];
            bool[] s2Matches = new bool[length2];

            int matches = 0;
            int transpositions = 0;

            // Count matches
            for (int i = 0; i < length1; i++)
            {
                int start = Math.Max(0, i - matchDistance);
                int end = Math.Min(i + matchDistance + 1, length2);

                for (int j = start; j < end; j++)
                {
                    if (s2Matches[j] || s1[i] != s2[j]) continue;
                    s1Matches[i] = s2Matches[j] = true;
                    matches++;
                    break;
                }
            }

            if (matches == 0)
                return 0;

            // Count transpositions
            int k = 0;
            for (int i = 0; i < length1; i++)
            {
                if (!s1Matches[i]) continue;
                while (!s2Matches[k]) k++;
                if (s1[i] != s2[k])
                    transpositions++;
                k++;
            }

            return ((matches / (double)length1) +
                    (matches / (double)length2) +
                    ((matches - transpositions / 2.0) / matches)) / 3.0;
        }

        private static int GetCommonPrefixLength(string s1, string s2, int maxPrefixLength = 4)
        {
            int n = Math.Min(Math.Min(s1.Length, s2.Length), maxPrefixLength);
            for (int i = 0; i < n; i++)
            {
                if (s1[i] != s2[i])
                    return i;
            }
            return n;
        }
    }
}
