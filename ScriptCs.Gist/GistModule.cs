using ScriptCs.Contracts;

namespace ScriptCs.Gist
{
    [Module("gist", Extensions = "csx")]
    public class GistModule : IModule
    {
        public void Initialize(IModuleConfiguration config)
        {
            config.LineProcessor<GistLineProcessor>();
        }
    }
}
