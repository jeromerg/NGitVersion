using System.Collections.Generic;
using System.Text.RegularExpressions;
using NLog;

namespace Version.Util.Git {
    public class GitLog {
        private static readonly Logger logger_ = LogManager.GetCurrentClassLogger();

        private const string LOG_REGEX = @"^TIME:(?<time>[^\r\n]*)(\r\n|\n)^HASH:(?<hash>[^\r\n]*)(\r\n|\n)^SHORT:(?<short>[^\r\n]*)(\r\n|\n)";

        public List<GitCommit> Commits { get; private set; }

        public GitLog(string gitLog) {

            Commits = new List<GitCommit>();
            
            logger_.Info("Parsing git log with regex: " + LOG_REGEX);
            var regexObj = new Regex(LOG_REGEX, RegexOptions.Singleline | RegexOptions.Multiline);
            Match matchResult = regexObj.Match(gitLog);
            
            while (matchResult.Success) {
                string hash = matchResult.Groups["hash"].Value;
                string shortHash = matchResult.Groups["short"].Value;
                string time = matchResult.Groups["time"].Value;

                logger_.Debug("commit parsed with commit id " + hash);

                Commits.Add(new GitCommit(hash, shortHash, time));
                matchResult = matchResult.NextMatch();
            }            
        }

    }
}
