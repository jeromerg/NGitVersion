using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.StringTemplate;
using LibGit2Sharp;

namespace NGitVersion {
    public static class NGitVersion {

        // directories are relative to output project directory ${ProjectDir}\bin
        private const string TEMPLATE_DIR = @"..\Templates\";
        private const string TEMPLATE_FILE_PREFIX = @"File_";
        private const string OUTPUT_DIR = @"..\Result\";

        public static void Main(string [] args)
        {
            //git rev-list HEAD --count
            var builtInVars = new Dictionary<string, object> { { "git", new LibGit2Sharp.Repository(".") } };

            foreach (var templateFile in Directory.GetFiles(TEMPLATE_DIR, TEMPLATE_FILE_PREFIX + "*.stg"))
                ProcessTemplate(templateFile, builtInVars);
        }

        private static void ProcessTemplate(string templateFile, Dictionary<string, object> builtInVars)
        {
            Template template = new TemplateGroupFile(templateFile)
                                        .GetInstanceOf(Path.GetFileNameWithoutExtension(templateFile));

            foreach (var v in builtInVars)
                template.Add(v.Key, v.Value);

            File.WriteAllText(BuildTargetFile(templateFile), template.Render());
        }

        private static string BuildTargetFile(string templateFile)
        {
            string outputFileName
                = Path.GetFileNameWithoutExtension(templateFile)
                      .Replace(TEMPLATE_FILE_PREFIX, "")
                      .Replace("_", ".");

            return OUTPUT_DIR + outputFileName;
        }
    }
}
