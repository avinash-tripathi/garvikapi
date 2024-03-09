using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class BusinessType
    {
        public string businesstypecode { get; set; }
        public string businesstypename { get; set; }
        public string businesstypedescription { get; set; }
        public BusinessTypeDoc[] businesstypedocs { get; set; }


        public static IDictionary<string, string> ConverttoJson(BusinessType obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("businesstypecode", obj.businesstypecode);
            objDict.Add("businesstypename", obj.businesstypename);
            return objDict;
        }

    }
}
