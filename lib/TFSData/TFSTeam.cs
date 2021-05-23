using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.lib.TFSData {

    public class TFSTeam : Base {

        // Properties of this team defined here
        public string Name { get => Team.Name; }
        public Guid Id { get => Team.Id; }

        // MS Extended Client Team
        private WebApiTeam Team { get; set; }

        // These are OUR objects
        private Project Project { get; set; }
        private TFSUsers Users { get; set; }
        private ADGroupCollection ADGroups { get; set; }
        private TFSGroups Groups { get; set; }

        public TFSTeam(Project project, WebApiTeam team, PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) : base(loadDepth, tpc) {
            Project = project;
            Team = team;

            Users = new TFSUsers(Project.Id.ToString(), Team.Id.ToString(), PreloadDepth, TFSCollection);
            Groups = new TFSGroups(Project.Id.ToString(), Team.Id.ToString(), PreloadDepth, TFSCollection);
            ADGroups = new ADGroupCollection();

            if (PreloadDepth >= PreloadDepthEnum.Members) {
                Task.WaitAll(new Task[] {
                    LoadMembershipListsAsync()
                });
            }
        }
        //Task.Run(() => this.LoadMembershipListsAsync()).Wait();

        /*
         * (Identity.IsContainer = true && Identity.Descriptor.IdentityType == "System.Security.Principal.WindowsIdentity") == AD Group
         * (Identity.IsContainer = false && Identity.Descriptor.IdentityType == "System.Security.Principal.WindowsIdentity") == AD User
         * (Identity.IsContainer = true && Identity.Descriptor.IdentityType == "Microsoft.TeamFoundation.Identity") == TFS Group
         * (Identity.IsContainer = false && Identity.Descriptor.IdentityType == "Microsoft.TeamFoundation.Identity") == TFS Group
        */
        public Task LoadMembershipListsAsync() {
            return Task.Factory.StartNew(async () => {
                List<Identity> memberships = (await Task.WhenAll((
                                    await TeamClient.GetTeamMembersAsync(Project.Id.ToString(), Id.ToString())
                                    ).Select(async
                                    x => await IdentityClient.ReadIdentityAsync(x.Id, QueryMembership.Expanded)
                                ))).OrderBy(x => x.DisplayName).ToList();

                List<TFSUser> users = memberships.Where(m => !m.IsContainer && m.Descriptor.IdentityType == "Microsoft.TeamFoundation.Identity")
                                            .Select(m => new TFSUser(m, PreloadDepth, TFSCollection)).ToList();
                Users = new TFSUsers(Project.Id.ToString(), Team.Id.ToString(), users, PreloadDepth, TFSCollection);

                List<TFSGroup> groups = memberships.Where(g => g.IsContainer && g.Descriptor.IdentityType == "Microsoft.TeamFoundation.Identity")
                                            .Select(g => new TFSGroup(g, PreloadDepth, TFSCollection)).ToList();
                Groups = new TFSGroups(Project.Id.ToString(), Team.Id.ToString(), groups, PreloadDepth, TFSCollection);
            });
            /*
            List<ADGroup> adgroups = memberships.Where(
                                        a => a.IsContainer && a.Descriptor.IdentityType == "System.Security.Principal.WindowsIdentity"
                                    ).Select(a => new ADGroup(a.DisplayName)).ToList();
            */

        }

    }
}
