namespace Version.Util.Git {
    public class GitCommit {

        public string Hash { get; private set; }
        public string ShortHash { get; private set; }
        public string Time { get; private set; }

        public GitCommit(string hash, string shortHash, string time) {
            Hash = hash;
            ShortHash = shortHash;
            Time = time;
        }
    }
}