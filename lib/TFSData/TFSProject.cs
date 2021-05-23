using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using System;

namespace Crawler.lib.TFSData {

    public class Project : Base {

        public Guid Id { get => TFSProject.Id; }
        public string Name { get => TFSProject.Name; }

        private TeamProject TFSProject { get; set; }
        private TFSTeams Teams { get; set; }

        public Project(TeamProject project, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {

            TFSProject = project;
            Teams = new TFSTeams(this, TFSProject.DefaultTeam.Id.ToString(), PreloadDepth, TFSCollection);
            
        }

        public string[] GetTeamNames() {
           return Teams.GetTeamNames();
        }
        public string[] GetTeamNamesSorted() {
            return Teams.GetTeamNamesSorted();
        }
    }
}