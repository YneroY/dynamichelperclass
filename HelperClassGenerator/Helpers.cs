using System;
using System.Collections.Generic;

namespace HelperClassGenerator
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Measure the similarity between 2 strings using the calculated edit distance, and maximum string length between the 2.
        /// </summary>
        /// <param name="editDistance">Edit distance calculation.</param>
        /// <param name="maxStringLength">Maximum string length among the 2 strings compared.</param>
        /// <returns>Similarity value.</returns>
        public static double MeasureSimilarity(int editDistance, int maxStringLength)
        {
            // https://stackoverflow.com/questions/14260126/how-python-levenshtein-ratio-is-computed
            // https://stackoverflow.com/questions/45783385/normalizing-the-edit-distance

            return 1 - (double)editDistance / maxStringLength;
        }

        /// <summary>
        /// A dictionary containing possible method name and it's corresponding implementation information.
        /// </summary>
        public static readonly Dictionary<string, MethodInfo> MethodImplementationReference = new Dictionary<string, MethodInfo>()
        {
            { "GetAsyncMethodName", Constants.GETASYNCMETHODNAME },
            { "GetMethodName", Constants.GETASYNCMETHODNAME },
            { "ShowMethodName", Constants.GETASYNCMETHODNAME },
            { "GetCrc16", Constants.GETCRC16 },
            { "ShowCrc16", Constants.GETCRC16 },
            { "IWantCrc16", Constants.GETCRC16 }
        };
    }
}
