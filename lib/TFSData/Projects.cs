using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.lib.TFSData {
    public class Projects : Base {

        private List<Project> ProjectList { get; set; }

        public string[] ProjectNames {
            get => ProjectList.Select(p => p.Name).ToArray();
        }
        public string[] ProjectNamesSorted {
            get => ProjectList.Select(p => p.Name).OrderBy(p => p).ToArray();
        }
    
        public Projects() {
        }

        public void Configure(PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) {
            ProjectList = new List<Project>();

            if (PreloadDepth >= PreloadDepthEnum.Project) LoadAllProjects();
        }

        private async void LoadAllProjects() {
            ProjectList = (await Task.WhenAll((await ProjectClient.GetProjects()).Select(async x => await ProjectClient.GetProject(x.Id.ToString())))).Select(p => new Project(p, PreloadDepth, TFSCollection)).ToList();

        }

        public Project Project(Guid id) {
            return ProjectList.Where(p => id == p.Id).FirstOrDefault();
        }

        public Project Project(string name) {
            return ProjectList.Where(p => name == p.Name).FirstOrDefault();
        }

        public string[] GetProjectNames() {
            return ProjectList.Select(p => p.Name).ToArray();
        }

        public string[] GetProjectNamesSorted() {
            return ProjectList.Select(p => p.Name).OrderBy(p => p).ToArray();
        }
    }
}