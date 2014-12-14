using System.Diagnostics;
using System.IO;
using NLog;

namespace Version.Util.Git {
    public class GitExe {
        private static readonly Logger logger_ = LogManager.GetCurrentClassLogger();

        string gitExe = "git.exe";

        public GitLog Log() {
            var rawLog = Execute("--no-pager log --format=format:\"TIME:%ai%nHASH:%H%nSHORT:%h%n\"");
            var gitLog = new GitLog(rawLog);
            return gitLog;
        }

        public bool HasLocalChange() {
            string rawStatus = Execute("status --porcelain");
            return !string.IsNullOrWhiteSpace(rawStatus);
        }

        private string Execute(string parameters) {
            logger_.Info("Starting git process: " + gitExe + " " + parameters);

            var processStartInfo = new ProcessStartInfo(gitExe, parameters) {
                RedirectStandardOutput = true, 
                CreateNoWindow = true, 
                UseShellExecute = false,
                ErrorDialog = false};

            var p = new Process() {StartInfo = processStartInfo};
            p.Start();
            StreamReader sr = p.StandardOutput;
            string rawLog = sr.ReadToEnd();
            p.WaitForExit();
            
            logger_.Info("git process executed");
            return rawLog;
        }

    }
}
