using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class State
    {
        public string uniquecode { get; set; }
        public string statecode { get; set; }
        public string statename { get; set; }
        public City[] cities { get; set; }


        public static IDictionary<string, string> ConverttoJson(State obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("uniquecode", obj.uniquecode);
            objDict.Add("statecode", obj.statecode);
            objDict.Add("statename", obj.statename);
            return objDict;
        }
    }
}
