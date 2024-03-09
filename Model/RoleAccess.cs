using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class RoleAccess
    {
        public string modulecode { get; set; }
        public string modulename { get; set; }
        public string rolecode { get; set; }
        public string rolename { get; set; }
        public Boolean isreadonly { get; set; }
        public Boolean fullaccess { get; set; }
        public string createdby { get; set; }


    }
}
