using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Directories.DirectoryService.Client;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.Identity.Client;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.Profile.Client;
using Microsoft.VisualStudio.Services.Security.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using Crawler.lib.TFSData;
using Crawler.lib;

namespace Crawler.Controllers {
    public class ProjectsController : Controller {

        // TFS Collection Object
        private TfsTeamProjectCollection TFSCollection { get; set; }

        // HTTP CLIENTS START HERE
        public TeamHttpClient TeamClient {
            get => TFSCollection.GetClient<TeamHttpClient>();
        }
        public ProjectHttpClient ProjectClient {
            get => TFSCollection.GetClient<ProjectHttpClient>();
        }
        public SecurityHttpClient SecurityClient {
            get => TFSCollection.GetClient<SecurityHttpClient>();
        }
        public DirectoryHttpClient DirectoryClient {
            get => TFSCollection.GetClient<DirectoryHttpClient>();
        }
        public IdentityHttpClient IdentityClient {
            get => TFSCollection.GetClient<IdentityHttpClient>();
        }
        public OrganizationHttpClient OrgClient {
            get => TFSCollection.GetClient<OrganizationHttpClient>();
        }
        public ProfileHttpClient ProfileClient {
            get => TFSCollection.GetClient<ProfileHttpClient>();
        }
        public DirectorySearcher ADSearcher {
            get => new DirectorySearcher(new DirectoryEntry("LDAP://mmm.com", "mmm\\a80fbzz", "Gallium^2018"));
        }
        // END CLIENT OBJECTS

        public ProjectsController() {
            TFSCollection = lib.TFSHelper.GetTFSCollectionObject();
        }

        public ActionResult Index() {
            return View();
        }

        Project TestProj { get; set; }

        public async Task<ActionResult> List() {

            // Fetch TeamProjectReference and convert to full TeamProject,
            // sorted by {proj}.Name, and hand it off to ViewBag
            ViewBag.Projects = (await Task.WhenAll((
                                    await ProjectClient.GetProjects()
                                ).Select(
                                    async x => await ProjectClient.GetProject(x.Id.ToString())
                                ))).OrderBy(x => x.Name).ToList();

            return View();
        }

        public async Task<ActionResult> Details(string id) {

            /* == PROJECT == */
            TeamProject Project = await ProjectClient.GetProject(id);

            /* == DEFAULT TEAM == */
            // Grab the DefaultTeam ID from the Project and use to fetch a WebApiTeam
            WebApiTeam DefaultTeam = await TeamClient.GetTeamAsync(id, Project.DefaultTeam.Id.ToString());

            /* == DEFAULT TEAM MEMBERS (PEOPLE AND GROUPS) == */
            // Fetch IdentityRefs and convert to full Identity objects (w/expanded membership info)
            List<Identity> DTMembers =  (await Task.WhenAll((
                                            await TeamClient.GetTeamMembersAsync( id, Project.DefaultTeam.Id.ToString() )
                                        ).Select( async
                                            x => await IdentityClient.ReadIdentityAsync(x.Id, QueryMembership.Expanded)
                                        ))).OrderBy(x => x.DisplayName).ToList();

            /* == TFS GROUPS == */
            IOrderedEnumerable<Identity> TFSGroups = (await IdentityClient.ListGroupsAsync(new Guid[]{Project.Id})).OrderBy(x=>x.DisplayName);

            /* == TFS TEAMS == */
            List<WebApiTeam> ProjectTeams = (await TeamClient.GetTeamsAsync(Project.Id.ToString())).OrderBy(x=>x.Name).ToList();

            // Put them all in the ViewBag
            ViewBag.Project = Project;
            ViewBag.DefaultTeam = DefaultTeam;
            ViewBag.DTMembers = DTMembers;
            ViewBag.TFSGroups = TFSGroups;
            ViewBag.ProjectTeams = ProjectTeams;


            TestProj = new Project(Project, PreloadDepthEnum.Teams, TFSCollection);
            //ADGroupCollection test = new ADGroupCollection("US-ETFS-CBG-ACoE-Contributors");



            /*
            var val = ADSearcher;
            val.Filter = "(&(objectClass=user)(lastName=Rypka-Hauer))";
            var test = val.FindAll();
            test.Dispose();
            ADSearcher.Dispose();
            */

            return View();
        }
    }
}