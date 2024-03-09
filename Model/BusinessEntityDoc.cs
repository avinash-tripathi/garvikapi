using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class BusinessEntityDoc
    {
        public string businessentitydoccode { get; set; }
        public string businessentitydocname { get; set; }
        public string businessentitydocdescription { get; set; }

        public static IDictionary<string, string> ConverttoJson(BusinessEntityDoc obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("businessentitydoccode", obj.businessentitydoccode);
            objDict.Add("businessentitydocname", obj.businessentitydocname);
            return objDict;
        }
    }
}
