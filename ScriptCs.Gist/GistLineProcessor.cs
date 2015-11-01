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

        protected override string DirectiveName
        {
            get { return "gist"; }
        }

        public GistLineProcessor(ILogProvider logProvider)
        {
            _Downloader = new GistDownloader(logProvider.ForCurrentType());
        }

        protected override bool ProcessLine(IFileParser parser, FileParserContext context, string line)
        {
            var args = GetDirectiveArgument(line).Split();

            var gistId = args[0];
            var scriptToExecute = args.Length > 1 ? args[1] : null;

            var files = _Downloader.DownloadGistFiles(gistId);

            if (!string.IsNullOrEmpty(scriptToExecute))
            {
                var file = files.First(f => f.Contains(scriptToExecute));
                parser.ParseFile(file, context);
            }
            else
            {
                foreach (var file in files)
                {
                    parser.ParseFile(file, context);
                }
            }

            return true;
        }
    }
}
