using System;

namespace HelperClassGenerator
{
    /// <summary>
    /// Class which contains the method to calculate the distance between 2 strings.
    /// </summary>
    public static class EditDistanceAlgorithm
    {
        /// <summary>
        /// Minimum distance for string 1 to reach string 2.
        /// </summary>
        /// <param name="word1">String 1.</param>
        /// <param name="word2">String 2.</param>
        /// <returns>Minimum distance.</returns>
        public static int MinDistance(string word1, string word2)
        {
            if (string.IsNullOrEmpty(word1))
                return word2.Length;

            if (string.IsNullOrEmpty(word2))
                return word1.Length;


            int[,] dp = new int[word1.Length + 1, word2.Length + 1];

            for (int i = 0; i <= word1.Length; i++)
            {
                dp[i, 0] = i;
            }

            for (int j = 0; j <= word2.Length; j++)
            {
                dp[0, j] = j;
            }

            for (int i = 1; i <= word1.Length; i++)
                for (int j = 1; j <= word2.Length; j++)
                {
                    if (char.ToUpper(word1[i - 1]) == char.ToUpper(word2[j - 1]))
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {
                        dp[i, j] = 1 + FindMin(dp[i - 1, j], dp[i, j - 1], dp[i - 1, j - 1]);
                    }
                }

            return dp[word1.Length, word2.Length];
        }

        private static int FindMin(int a, int b, int c)
        {
            int d = Math.Min(a, b);
            return Math.Min(c, d);
        }

    }
}
