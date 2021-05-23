using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Services.Identity;

namespace Crawler.lib.TFSData {
    public class BaseTFSIdentity : Base {

        private Identity Ident { get; set; }

        public BaseTFSIdentity(Identity identity, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Ident = identity;
        }

    }
}