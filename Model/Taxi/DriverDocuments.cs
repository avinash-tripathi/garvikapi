using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class DriverDocuments
    {
        public string documentvalue { get; set; }
        public string documenttype { get; set; }

        public static IDictionary<string, string> ConverttoJson(DriverDocuments obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("documentvalue", obj.documentvalue);
            objDict.Add("documenttype", obj.documenttype);
            
            return objDict;
        }
    }
}
