using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class BusinessEntity
    {
        public string businessentitycode { get; set; }
        public string businessentityname { get; set; }
        public string businessentitydescription { get; set; }
        public BusinessEntityDoc[] businessentitydocs { get; set; }
        public BusinessType[] businesstypes { get; set; }


        public static IDictionary<string, string> ConverttoJson(BusinessEntity obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("businessentitycode", obj.businessentitycode);
            objDict.Add("businessentityname", obj.businessentityname);
            return objDict;
        }
    }
}
