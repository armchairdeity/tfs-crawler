using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Identity;
using Microsoft.VisualStudio.Services.Directories.DirectoryService.Client;
using Microsoft.VisualStudio.Services.Identity.Client;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.Profile.Client;
using Microsoft.VisualStudio.Services.Security.Client;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace Crawler.lib.TFSData {
    public class Base {



        // TFS Collection Object
        protected TfsTeamProjectCollection TFSCollection { get; set; }

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

        protected PreloadDepthEnum PreloadDepth { get; set; }

        public Base() {
        }

        public void Configure(PreloadDepthEnum loadDepth, TfsTeamProjectCollection tpc) {
            PreloadDepth = loadDepth;
            TFSCollection = tpc;
        }

        public static Base GetTfsObjectInstance(string type) {

            Base retVal = null;

            switch (type) {
                case "projects":
                    retVal = new Projects();
                    break;
            }

            return retVal;

        }

    }
}