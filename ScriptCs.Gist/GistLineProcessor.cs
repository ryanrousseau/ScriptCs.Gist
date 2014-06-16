using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScriptCs.Contracts;

namespace ScriptCs.Gist
{
    /// <summary>
    /// Process "#gist $gistId" lines by downloading .csx files in the gist,
    /// saving to a gists folder, and then parsing the new file
    /// </summary>
    public class GistLineProcessor : DirectiveLineProcessor
    {
        private readonly ILog _Log;

        protected override string DirectiveName
        {
            get { return "gist"; }
        }

        public GistLineProcessor(ILog log)
        {
            _Log = log;
        }

        private dynamic GetGist(string gistId)
        {
            var url = string.Format("https://api.github.com/gists/{0}", gistId);
            var client = new HttpClient(new HttpClientHandler());
            client.DefaultRequestHeaders.Add("User-Agent", "ScriptCs.Gist");

            _Log.Debug(string.Format("Requesting {0}", url));
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<JObject>(result);
        }

        private IEnumerable<string> DownloadGistFiles(string gistId)
        {
            var files = new List<string>();
            var gist = GetGist(gistId);

            var gistDir = Path.Combine(Environment.CurrentDirectory, "gists", gistId);

            if (!Directory.Exists(gistDir))
            {
                _Log.Debug(string.Format("Creating {0}", gistDir));
                Directory.CreateDirectory(gistDir);
            }

            foreach (var gistFile in gist["files"])
            {
                var name = (string)gistFile.Name;
                if (!name.EndsWith(".csx"))
                {
                    _Log.Debug(string.Format("Skipping non-csx file {0}", name));
                    continue;
                }

                var filePath = Path.Combine(gistDir, name);
                files.Add(filePath);

                if (File.Exists(filePath))
                {
                    _Log.Debug(string.Format("{0} exists. Loading local script, delete this file to refresh from gist", name));
                    continue;
                }

                var script = (string)gist["files"][name]["content"];
                _Log.Debug(string.Format("Creating file {0}", filePath));
                File.WriteAllText(filePath, script);
            }

            return files;
        }

        protected override bool ProcessLine(IFileParser parser, FileParserContext context, string line)
        {
            var gistId = GetDirectiveArgument(line);
            var files = DownloadGistFiles(gistId);

            foreach (var file in files)
            {
                parser.ParseFile(file, context);
            }

            return true;
        }
    }
}
