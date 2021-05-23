using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crawler.lib {

    public class TFSHelper {

        public TfsTeamProjectCollection TPC {
            get;
            set;
        }

        public string TeamURL {
            get {
                return "https://tfs.mmm.com/tfs/DefaultCollection";
            }
        }

        public Uri TeamURI {
            get {
                return new Uri(TeamURL);
            }
        }

        public string PAT {
            get {
                return "chnqulyjlvjkfhk5d6sdyltrfz2sbcdcvum5zt5enebyhylq6dsa";
            }
        }

        public VssBasicCredential Credential {
            get {
                return new VssBasicCredential(string.Empty, PAT);
            }
        }
        public TFSHelper() {

        }

        public static TfsTeamProjectCollection GetTFSCollectionObject() {
            TFSHelper helper = new TFSHelper();
            TfsTeamProjectCollection TPC = new TfsTeamProjectCollection(helper.TeamURI, helper.Credential);
            TPC.EnsureAuthenticated();
            return TPC;
        }
    }

}