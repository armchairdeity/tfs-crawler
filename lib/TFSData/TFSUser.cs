using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Services.Identity;

namespace Crawler.lib.TFSData {
    public class TFSUser : BaseTFSIdentity {

        public TFSUser(Identity identity, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(identity, loadDepth, tpc) {
        }
    }
}
