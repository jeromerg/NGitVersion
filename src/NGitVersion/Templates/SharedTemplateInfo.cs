using System.Collections.Generic;

namespace Version.Templates
{
    public static class SharedTemplateInfo
    {
        /// <summary>
        /// OPTIONAL: SET YOUR EXPECTED OLDEST COMMIT HASH 
        /// --> ensures that local git rep has full history. Ensures incremental version is correct
        /// </summary>
        public const string EXPECTED_OLDEST_COMMIT_HASH = null; 

        /// <summary> Key-Value pair used to inflate the templates </summary>
        public static readonly Dictionary<string, string> AdditionalAttributes = new Dictionary<string, string>
        {
            {"MAJOR", "1"},
            {"MINOR", "1"},
            {"BUILD", "1"},

            {"COMPANY", "My Company"},
            {"PRODUCT", "My Product"},
            {"COPYRIGHT", "My Copyright"},
        };
    }
}