using System.IO;
using System.Linq;
using Common.Logging;
using ScriptCs.Contracts;

namespace ScriptCs.Gist
{
    /// <summary>
    /// Process ":gist "gistId"" lines by downloading .csx files in the gist,
    /// saving to a gists folder, and then parsing the new file
    /// </summary>
    public class GistReplCommand : IReplCommand
    {
        private readonly GistDownloader _Downloader;

        public string CommandName
        {
            get { return "gist"; }
        }

        public string Description
        {
            get { return "Download and execute script hosted as a gist."; }
        }

        public GistReplCommand(ILog log)
        {
            _Downloader = new GistDownloader(log);
        }

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null || args == null || args.Length == 0)
            {
                return null;
            }

            var gistId = args[0].ToString();
            var files = _Downloader.DownloadGistFiles(gistId);

            foreach (var script in files.Select(File.ReadAllText))
            {
                repl.Execute(script, null);
            }

            return null;
        }
    }
}
