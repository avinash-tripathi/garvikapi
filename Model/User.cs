using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class User
    {
        public string usercode { get; set; }
        public string username { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }

        public string state { get; set; }
        public string pincode { get; set; }
        public string aadhaarno { get; set; }
        public string usercategory { get; set; }
        public string password { get; set; }
        public Module[] mappedmodule { get; set; }
        public Role[] mappedrole { get; set; }

        public string businessentitycode { get; set; }
        public bool approved { get; set; }










    }
}
