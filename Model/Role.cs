using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class Role
    {
        public string rolecode { get; set; }
        public string rolename { get; set; }
        public string roledescription { get; set; }

        public static IDictionary<string, string> ConverttoJson(Role obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("rolecode", obj.rolecode);
            return objDict;
        }
    }
}
