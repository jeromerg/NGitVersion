using System;
using System.Diagnostics;
using System.IO;
using NLog;

namespace Version.Util.Git {
    public class GitRepository {
        private static readonly Logger sLogger = LogManager.GetCurrentClassLogger();
        
        private const string GIT_EXE = "git.exe";

        private readonly Lazy<GitLog> mGitLog;
        private readonly Lazy<bool> mHasLocalChange;

        public GitRepository()
        {
            mGitLog = new Lazy<GitLog>(GetLog);
            mHasLocalChange = new Lazy<bool>(GetHasLocalChange);
        }

        #region public properties

        public GitLog Log
        {
            get { return mGitLog.Value; }
        }

        public bool HasLocalChange
        {
            get { return mHasLocalChange.Value; }
        }
       
        #endregion

        #region private methods
        private GitLog GetLog()
        {
            var rawLog = Execute("--no-pager log --format=format:\"TIME:%ai%nHASH:%H%nSHORT:%h%n\"");
            var gitLog = new GitLog(rawLog);
            return gitLog;
        }

        public bool GetHasLocalChange() {
            string rawStatus = Execute("status --porcelain");
            return !string.IsNullOrWhiteSpace(rawStatus);
        }

        private string Execute(string parameters) {
            sLogger.Info("Starting git process: " + GIT_EXE + " " + parameters);

            var processStartInfo = new ProcessStartInfo(GIT_EXE, parameters) {
                RedirectStandardOutput = true, 
                CreateNoWindow = true, 
                UseShellExecute = false,
                ErrorDialog = false};

            var p = new Process {StartInfo = processStartInfo};
            p.Start();
            StreamReader sr = p.StandardOutput;
            string rawLog = sr.ReadToEnd();
            p.WaitForExit();
            
            sLogger.Info("git process executed");
            return rawLog;
        }
        #endregion
    }
}
