using System;
using System.IO;
using System.Linq;
using Antlr4.StringTemplate;
using LibGit2Sharp;

namespace NGitVersion {
    public static class NGitVersion
    {

        private const string MODEL_VAR = @"m";

        // directories are relative to output project directory ${ProjectDir}\bin
        private const string TEMPLATE_DIR = @"..\..\Templates\";
        private const string OUTPUT_DIR = @"..\..\Generated\";
        private const string MAIN_TEMPLATE_NAME = @"MainTemplate";

        /// <summary> expected: 1 argument providing the path to the git repository </summary>
        public static void Main(string[] args)
        {
            if(args.Length != 1)
                throw new ArgumentException("expected: 1 argument providing the path to the git repository");

            var model = new Model.Model(new Repository(args[0]));

            Directory.GetFiles(TEMPLATE_DIR, "*.stg")
                     .Select(Path.GetFullPath)
                     .ToList()
                     .ForEach(templateFile => ProcessTemplate(templateFile, model));
        }

        private static void ProcessTemplate(string templateFile, Model.Model model)
        {
            Template template = new TemplateGroupFile(templateFile)
                .GetInstanceOf(MAIN_TEMPLATE_NAME);

            template.Add(MODEL_VAR, model);

            File.WriteAllText(BuildTargetFileName(templateFile), template.Render());
        }

        private static string BuildTargetFileName(string templateFile)
        {
            string outputFileName = Path.GetFileNameWithoutExtension(templateFile);
            return OUTPUT_DIR + outputFileName;
        }
    }
}
