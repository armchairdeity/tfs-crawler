using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.lib.TFSData {

    public class TFSTeams : Base {

        private Project Project { get; set; }
        private string DefaultTeamId { get; set; }
        private List<TFSTeam> Teams { get; set; }

        public TFSTeams(Project project, string defaultTeamId, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Project = project;
            DefaultTeamId = defaultTeamId;

            if (PreloadDepth >= PreloadDepthEnum.Teams) {
                Task.WhenAll(LoadAllTeams());
                /*
                Task.WaitAll(new Task[] {
                    LoadAllTeams()
                });
                */
            }
            else {
             Teams = new List<TFSTeam>();
            }
        }

        private Task<Task> LoadAllTeams() {
            return Task.Factory.StartNew(async () => {
               Teams = (await TeamClient.GetTeamsAsync(Project.Id.ToString())).Select(t => new TFSTeam(Project, t, PreloadDepth, TFSCollection)).ToList();
            });
        }

        public TFSTeam GetTeamById(string id) {
            return Teams.Where(x => id == x.Id.ToString()).FirstOrDefault();
        }

        public TFSTeam GetTeamByName(string teamName) {
            return Teams.Where(x => teamName == x.Name).FirstOrDefault();
        }

        public string[] GetTeamNames() {
            return Teams.Select(t => t.Name).ToArray();
        }
        public string[] GetTeamNamesSorted() {
            return Teams.Select(t => t.Name).OrderBy(t => t).ToArray();
        }
    }
}