using Microsoft.TeamFoundation.Client;
using System.Collections.Generic;

namespace Crawler.lib.TFSData {
    public class TFSUsers : Base {

        private List<TFSUser> Users { get; set; }

        public TFSUsers(string projectId, string teamID, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Users = new List<TFSUser>();
        }

        public TFSUsers(string projectId, string teamID, List<TFSUser> initialList, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Users = initialList;
        }
    }
}