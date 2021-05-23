using Microsoft.VisualStudio.Services.Identity;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Crawler.lib.TFSData {

    /*
        * For users get:
        PIN                     members[0].Properties["uid"][0]
        DisplayName             members[0].Properties["displayname"][0]
        Mail                    members[0].Properties["mail"][0]
        Division                members[0].Properties["division"][0]
        Company                 members[0].Properties["company"][0]
        Department              members[0].Properties["department"][0]
        DepartmentNumber        members[0].Properties["departmentnumber"][0]
        Manager                 members[0].Properties["manager"][0]
    */

    public class ADGroup {

        public string Name { get => (string)RawGroupResult.Properties ["name"][0]; }

        public string DisplayName { get => (string)RawGroupResult.Properties["displayname"][0]; }

        public Identity TFSIdent { get; set; }
        public string TFSID { get => TFSIdent.Id.ToString(); }

        private DirectoryEntry Entry { get; set; }

        private string GroupName { get; set; }

        private DirectorySearcher GroupSearch { get; set; }

        private DirectorySearcher MemberSearch { get; set; }

        private DirectorySearcher ManagerSearch { get; set; }

        private SearchResult RawGroupResult { get; set; }

        private SearchResultCollection RawUserResults { get; set; }
        
        public List<ADUser> Members { get; set; }

        public ADGroup(string groupName) {

            GroupName = groupName;

            Entry = new DirectoryEntry("LDAP://mmm.com", "mmm\\a80fbzz", "Gallium^2018");
            Members = new List<ADUser>();

            GroupSearch = new DirectorySearcher(Entry) {
                Filter = "(&(objectClass=group)(cn=" + GroupName + "))"
            };
            RawGroupResult = GroupSearch.FindOne();

            MemberSearch = new DirectorySearcher(Entry, "(objectClass=person)") {
                Filter = "(&(objectClass=User)(memberOf=" + RawGroupResult.Properties["distinguishedname"][0] + "))"
            };
            RawUserResults = MemberSearch.FindAll();

            foreach (SearchResult m in RawUserResults) {

                // Find the user's manager
                ManagerSearch = new DirectorySearcher(Entry, "(objectClass=person)") {
                    Filter = "(distinguishedname=" + m.Properties["manager"][0] + ")"
                };
                SearchResult managerResult = ManagerSearch.FindOne();

                if (managerResult != null) {
                    Members.Add(new ADUser(m.Properties, managerResult.Properties));
                }
                else {
                    Members.Add(new ADUser(m.Properties));
                }
                ManagerSearch.Dispose();
            }
        }
    }
}
