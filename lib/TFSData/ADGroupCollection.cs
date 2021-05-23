using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.lib.TFSData {
    public class ADGroupCollection {

        public List<ADGroup> Groups { get; set; }

        /// <summary>
        /// Inits a new empty instance of ADGroupCollection
        /// </summary>
        public ADGroupCollection() {
            Configure();
        }

        /// <summary>
        /// Inits a new instance of ADGroupCollection
        /// </summary>
        /// <param name="group">The initial group added to the Groups collection</param>
        public ADGroupCollection(ADGroup group) {
            Configure();
            this.Add(group);
        }

        /// <summary>
        /// Inits a new instance of ADGroupCollection
        /// </summary>
        /// <param name="name">Resolves the string to an AD group and inserts as the first into the Groups collection</param>
        public ADGroupCollection(string name) {
            Configure();
            this.Add(name);
        }

        public void Add(string name) {
            Groups.Add(new ADGroup(name));
        }

        public void Add(ADGroup group) {
            Groups.Add(group);
        }

        private void Configure() {
            Groups = new List<ADGroup>();
        }

        public string[] GetGroupNames() {
            return Groups.Select(g => g.Name).ToArray();
        }

        public string[] GetGroupNamesSorted() {
            return Groups.Select(g => g.Name).OrderBy(g=>g).ToArray();
        }

        public ADGroup GetByName(string name) {
            return Groups.Where( g => g.Name == name).First();
        }

        public ADGroup GetByTFSID(string tfsId) {
            return Groups.Where(g => g.TFSID == tfsId).First();
        }
    }
}