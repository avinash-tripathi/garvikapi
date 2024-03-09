using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class BusinessEntityToBusinessType
    {
        public string businessentitycode { get; set; }
        public string businesstypecode { get; set; }
        public static IDictionary<string, string> ConverttoJson(BusinessEntityToBusinessType obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("businessentitycode", obj.businessentitycode);
            objDict.Add("businesstypecode", obj.businesstypecode);
            return objDict;
        }
    }
}
