using System.IO;
using System.Linq;
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
        private readonly IFileSystem _FileSystem;
        private readonly ILog _Logger;

        public string CommandName
        {
            get { return "gist"; }
        }

        public string Description
        {
            get { return "Download and execute script hosted as a gist."; }
        }

        public GistReplCommand(IFileSystem fileSystem, ILogProvider logProvider)
        {
            _FileSystem = fileSystem;
            _Logger = logProvider.ForCurrentType();
            _Downloader = new GistDownloader(_Logger);
        }

        public object Execute(IRepl repl, object[] args)
        {
            if (repl == null || args == null || args.Length == 0)
            {
                return null;
            }

            var gistId = args[0].ToString();
            var files = _Downloader.DownloadGistFiles(gistId);

            var originalDirectory = _FileSystem.CurrentDirectory;
            var gistDirectory = Path.Combine(@".\gists\", args[0].ToString());
            _Logger.Debug(string.Format("Changing directory to {0}", gistDirectory));
            _FileSystem.CurrentDirectory = gistDirectory;

            var scriptsToExecute = args.Skip(1).Select(s => s.ToString());
            if (scriptsToExecute.Any())
            {
                files = scriptsToExecute.Select(s => files.First(f => f.Contains(s))).ToArray();
            }

            foreach (var file in files)
            {
                _Logger.Debug(string.Format("Executing {0}", file));
                var script = _FileSystem.ReadFile(file);
                repl.Execute(script, null);
            }

            _Logger.Debug(string.Format("Changing directory to {0}", originalDirectory));
            _FileSystem.CurrentDirectory = originalDirectory;

            return null;
        }
    }
}
