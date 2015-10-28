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
            var gistId = GetDirectiveArgument(line);
            var files = _Downloader.DownloadGistFiles(gistId);

            foreach (var file in files)
            {
                parser.ParseFile(file, context);
            }

            return true;
        }
    }
}
