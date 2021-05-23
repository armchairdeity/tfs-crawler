using Microsoft.TeamFoundation.Client;
using System.Collections.Generic;

namespace Crawler.lib.TFSData {

    public class TFSGroups : Base {

        private List<TFSGroup> Groups { get; set; }

        public TFSGroups(string projectId, string teamID, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Groups = new List<TFSGroup>();
        }

        public TFSGroups(string projectId, string teamID, List<TFSGroup> initialGroups, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Groups = new List<TFSGroup>();
        }
    }
}