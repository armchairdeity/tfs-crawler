using Microsoft.VisualStudio.Services.Identity;

namespace Crawler.lib.TFSData {

    public class TFSIdentity : BaseTFSIdentity{

        public TFSIdentity(Identity identity, PreloadDepthEnum preloadDepth) : base(identity, preloadDepth) {
        }
    }
}
