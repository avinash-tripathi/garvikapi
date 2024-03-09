using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class City
    {
        public string uniquecode { get; set; }
        public string statecode { get; set; }
        public string citycode { get; set; }
        public string city { get; set; }

        public static IDictionary<string, string> ConverttoJson(City obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("uniquecode", obj.uniquecode);
            objDict.Add("statecode", obj.statecode);
            objDict.Add("citycode", obj.citycode);
            objDict.Add("cityname", obj.city);
            return objDict;
        }
    }
}
