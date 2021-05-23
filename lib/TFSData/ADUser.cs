using Microsoft.VisualStudio.Services.Identity;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace Crawler.lib.TFSData {
    public class ADUser {

        public string PIN { get => (string)Properties["uid"][0];  }
        public string DisplayName { get => (string)Properties["displayname"][0]; }
        public string Mail { get => (string)Properties["mail"][0]; }
        public string Division { get => (string)Properties["division"][0]; }
        public string Company { get => (string)Properties["company"][0]; }
        public string Department { get => (string)Properties["department"][0]; }
        public string DepartmentNumber { get => (string)Properties["departmentnumber"][0]; }

        public Identity TFSIdent { get; set; }
        public string TFSID { get => TFSIdent.Id.ToString(); }

        public ADUser Manager { get; set; }

        private ResultPropertyCollection Properties { get; set; }

        public ADUser(ResultPropertyCollection properties) {
            Properties = properties;
        }

        public ADUser(ResultPropertyCollection properties, ResultPropertyCollection manager) {
            Properties = properties;
            Manager = new ADUser(manager);
        }

        public object GetRawProperty(string propName) {
            return Properties[propName];
        }
    }
}
