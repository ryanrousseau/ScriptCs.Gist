using System.Linq;
using ScriptCs.Contracts;

namespace ScriptCs.Gist
{
    /// <summary>
    /// Process "#gist $gistId" lines by downloading .csx files in the gist,
    /// saving to a gists folder, and then parsing the new file
    /// </summary>
    public class GistLineProcessor : DirectiveLineProcessor
    {
        private readonly GistDownloader _Downloader;
        private readonly ILog _Logger;

        protected override string DirectiveName
        {
            get { return "gist"; }
        }

        public GistLineProcessor(ILogProvider logProvider)
        {
            _Logger = logProvider.ForCurrentType();
            _Downloader = new GistDownloader(_Logger);
        }

        protected override bool ProcessLine(IFileParser parser, FileParserContext context, string line)
        {
            var args = GetDirectiveArgument(line).Split();

            var gistId = args[0];
            var scriptsToExecute = args.Skip(1);

            var files = _Downloader.DownloadGistFiles(gistId);

            if (scriptsToExecute.Any())
            {
                files = scriptsToExecute.Select(s => files.First(f => f.Contains(s))).ToArray();
            }

            foreach (var file in files)
            {
                _Logger.Debug(string.Format("Parsing {0}", file));
                parser.ParseFile(file, context);
            }

            return true;
        }
    }
}
