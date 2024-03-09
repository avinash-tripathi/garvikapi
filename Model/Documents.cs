using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class Documents
    {
            public string documentvalue { get; set; }
            public string documenttype { get; set; }
        public string documenturl { get; set; }
        public static IDictionary<string, string> ConverttoJson(Documents obj)
            {
                IDictionary<string, string> objDict = new Dictionary<string, string>();
                objDict.Add("documentvalue", obj.documentvalue);
                objDict.Add("documenttype", obj.documenttype);
            objDict.Add("documenturl", obj.documenturl);
            return objDict;
            }
        }
   
}
