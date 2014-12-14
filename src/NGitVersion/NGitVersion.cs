using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Version.Util.Git;

// ReSharper disable UnusedVariable
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace Version {
    public static class NGitVersion {
        private static readonly Logger sLogger = LogManager.GetCurrentClassLogger();

        // TODO: SET MANUALLY MAJOR VERSION
        private const string MAJOR = "0";
        
        // TODO: SET MANUALLY MINOR VERSION 
        private const string MINOR = "1";
        
        // TODO: SET MANUALLY BUILD VERSION 
        private const string BUILD = "0";
        
        // TODO OPTIONAL: SET EXPECTED OLDEST COMMIT HASH
        private const string EXPECTED_OLDEST_COMMIT_HASH = null; // ex: "7af546ed5b4562fbf207e23bd91539aea6b6c0c6";
        // --> ensures that local git rep has full history. Ensures incremental version is correct
        
        // dir relative to output project directory ${ProjectDir}\bin
        private const string TEMPLATE_DIR = @"..\Template\";
        private const string TEMPLATE_FILES = @"*.*.template";
        private const string TEMPLATE_FILES_STRING_TO_REMOVE = @".template";
        // dir relative to output project directory ${ProjectDir}\bin
        private const string OUTPUT_DIR = @"..\Result\";
        private const string OUTPUT_FILES_PATTERN = @"{0}.generated{1}";

        public static void Main(string [] args) {
            
            var gitExe = new GitExe();
            GitLog gitLog = gitExe.Log();
            int revision = gitLog.Commits.Count;
            string lastCommitHash = gitLog.Commits[0].Hash;
            string lastCommitShortHash = gitLog.Commits[0].ShortHash;
            string oldestCommitHash = gitLog.Commits[gitLog.Commits.Count-1].Hash;
            bool hasLocalChange = gitExe.HasLocalChange();

            // ASSERT OLDEST COMMIT IF DEFINED
            bool isRepositoryMismatch = CheckRepositoryHistory(oldestCommitHash, ref revision);
            
            // Substitution key-values
            var substitutes = new Dictionary<string, object>();
            substitutes.Add("<major>", MAJOR);
            substitutes.Add("<minor>", MINOR);
            substitutes.Add("<build>", BUILD);
            substitutes.Add("<revision>", revision);
            substitutes.Add("<shortHash>", lastCommitShortHash);
            substitutes.Add("<hasLocalChange>", (hasLocalChange ? "(with local change)" : "") + 
                                                (isRepositoryMismatch ? "(repository/revision mismatch)" : ""));

            // Template
            string[] templateFiles = Directory.GetFiles(TEMPLATE_DIR, TEMPLATE_FILES);
            foreach (var templateFile in templateFiles) {
                sLogger.Info("Processing template file: " + templateFile);

                string fileContent = File.ReadAllText(templateFile);

                foreach (var substitute in substitutes) {
                    sLogger.Info("... substituting {0} by {1}", substitute.Key, substitute.Value);
                    fileContent = fileContent.Replace(substitute.Key, substitute.Value.ToString());
                }

                var targetFile = BuildTargetFile(templateFile);

                sLogger.Info("Exporting result into file: " + targetFile);
                File.WriteAllText(targetFile, fileContent);
            }
        }

        private static bool CheckRepositoryHistory(string oldestCommitHash, ref int revision)
        {
            if (EXPECTED_OLDEST_COMMIT_HASH == null)
                return false;

            bool isRepositoryMismatch = oldestCommitHash != EXPECTED_OLDEST_COMMIT_HASH;
            if (isRepositoryMismatch)
            {
                revision = Int16.MaxValue;
                sLogger.Warn("Oldest revision has id {0}, but is expected to be {1}. Local git repository is not the original. Revision is set to {2}", oldestCommitHash, EXPECTED_OLDEST_COMMIT_HASH, Int16.MaxValue);
            }
            else
            {
                sLogger.Info(string.Format("Oldest revision has id {0} as expected", EXPECTED_OLDEST_COMMIT_HASH));
            }
            return isRepositoryMismatch;
        }

        private static string BuildTargetFile(string templateFile)
        {
            string targetFile = templateFile.Replace(TEMPLATE_FILES_STRING_TO_REMOVE, "");
            targetFile = OUTPUT_DIR + string.Format(OUTPUT_FILES_PATTERN,
                Path.GetFileNameWithoutExtension(targetFile),
                Path.GetExtension(targetFile));
            return targetFile;
        }
    }
}
