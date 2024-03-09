using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class BusinessTypeDoc
    {
        public string businesstypedoccode { get; set; }
        public string businesstypedocname { get; set; }
        public string businesstypedocdescription { get; set; }

        public static IDictionary<string, string> ConverttoJson(BusinessTypeDoc obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("businesstypedoccode", obj.businesstypedoccode);
            objDict.Add("businesstypedocname", obj.businesstypedocname);
            return objDict;
        }
    }
}
