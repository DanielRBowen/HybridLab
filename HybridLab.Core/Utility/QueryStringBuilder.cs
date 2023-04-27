using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace HybridLab.Core.Utility
{
    /// <summary>
    /// Create a c# function which takes a comma delimited string list. 
    /// This function should build a query string from the delimited string with each of 
    /// two values of the delimited string be the key value pair to add to the query string. 
    /// The function should throw an error if there is not at least two values in the string or if there is an odd number.
    /// </summary>
    public static class QueryStringBuilder
    {
        /// <summary>
        /// Builds a query string from a comma-delimited string.
        /// Code from Bing Chat.
        /// </summary>
        /// <param name="input">A comma-delimited string with each two values being a key value pair.</param>
        /// <returns>A query string from the comma-delimited string values</returns>
        /// <exception cref="ArgumentException">Throws if the input string has less than 2 values or doesn't have an even number</exception>
        public static string BuildQueryString(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.");

            var values = input.Split(',');
            if (values.Length < 2 || values.Length % 2 != 0)
                throw new ArgumentException("Input must contain at least two values and an even number of values.");

            var keyValuePairs = new Dictionary<string, string>();
            for (int i = 0; i < values.Length; i += 2)
                keyValuePairs.Add(values[i], values[i + 1]);

            var values0 = keyValuePairs.Values.ToArray();
            var keys = keyValuePairs.Keys.ToArray();

            for (int index = 0; index < values0.Length; index++)
            {
                if (string.IsNullOrEmpty(values0[index]) || values0[index] == "0")
                {
                    var removed = keyValuePairs.Remove(keys[index]);

                    if (!removed)
                        throw new InvalidOperationException("Did not remove an empty key when building a query string.");
                }
            }

            if (keyValuePairs.Count == 0)
            {
                return string.Empty;
            }

            return ("?" + string.Join("&", keyValuePairs.Select(kvp => $"{kvp.Key}={kvp.Value}"))).Replace(" ", "");
        }

        public static string BuildQueryString(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            if (args.Length < 2 || args.Length % 2 != 0)
                throw new ArgumentException("Input must contain at least two values and an even number of values.");

            return BuildQueryString(string.Join(",", args));
        }
    }
}
