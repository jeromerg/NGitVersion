using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Antlr4.StringTemplate;
using NLog;
using Version.Template;
using Version.Templates;
using Version.Util.Git;

// ReSharper disable UnusedVariable
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162

namespace Version {
    public static class NGitVersion {
        private static readonly Logger sLogger = LogManager.GetCurrentClassLogger();
        
        // dir relative to output project directory ${ProjectDir}\bin
        private const string TEMPLATE_DIR = @"..\Templates\";
        private const string TEMPLATE_FILES = @"*.*.template";
        private const string TEMPLATE_FILES_STRING_TO_REMOVE = @".template";
        // dir relative to output project directory ${ProjectDir}\bin
        private const string OUTPUT_DIR = @"..\Result\";
        private const string OUTPUT_FILES_PATTERN = @"{0}.generated{1}";

        public static void Main(string [] args) {
            
            var gitExe = new GitRepository();
            GitLog gitLog = gitExe.Log;
            int revision = gitLog.Commits.Count;
            string lastCommitHash = gitLog.Commits[0].Hash;
            string lastCommitShortHash = gitLog.Commits[0].ShortHash;
            string oldestCommitHash = gitLog.Commits[gitLog.Commits.Count-1].Hash;
            
            // ASSERT OLDEST COMMIT IF DEFINED
            bool isRepositoryMismatch = CheckRepositoryHistory(oldestCommitHash, ref revision);
            
            // Substitution key-values
            SharedTemplateInfo.AdditionalAttributes.Add("<revision>", revision.ToString(CultureInfo.InvariantCulture));
            SharedTemplateInfo.AdditionalAttributes.Add("<shortHash>", lastCommitShortHash);
            SharedTemplateInfo.AdditionalAttributes.Add("<localChangeInfo>", (hasLocalChange ? "(with local change)" : "") + 
                                                (isRepositoryMismatch ? "(repository/revision mismatch)" : ""));

            // Template
            string[] templateFiles = Directory.GetFiles(TEMPLATE_DIR, TEMPLATE_FILES);
            foreach (var templateFile in templateFiles) {
                ProcessTemplate(templateFile);
            }
        }

        private static void ProcessTemplate(string templateFile)
        {
            sLogger.Info("Processing template file: " + templateFile);


            string fileContent = File.ReadAllText(templateFile);
            var template = new Template(fileContent);
            foreach (var substitute in SharedTemplateInfo.AdditionalAttributes)
            {
                sLogger.Info("... substituting {0} by {1}", substitute.Key, substitute.Value);
                fileContent = fileContent.Replace(substitute.Key, substitute.Value.ToString());
            }

            var targetFile = BuildTargetFile(templateFile);

            sLogger.Info("Exporting result into file: " + targetFile);
            File.WriteAllText(targetFile, fileContent);
        }

        private static bool CheckRepositoryHistory(string oldestCommitHash, ref int revision)
        {
            if (SharedTemplateInfo.EXPECTED_OLDEST_COMMIT_HASH == null)
                return false;

            bool isRepositoryMismatch = oldestCommitHash != SharedTemplateInfo.EXPECTED_OLDEST_COMMIT_HASH;
            if (isRepositoryMismatch)
            {
                revision = Int16.MaxValue;
                sLogger.Warn("Oldest revision has id {0}, but is expected to be {1}. Local git repository is not the original. Revision is set to {2}", oldestCommitHash, SharedTemplateInfo.EXPECTED_OLDEST_COMMIT_HASH, Int16.MaxValue);
            }
            else
            {
                sLogger.Info(string.Format("Oldest revision has id {0} as expected", SharedTemplateInfo.EXPECTED_OLDEST_COMMIT_HASH));
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
