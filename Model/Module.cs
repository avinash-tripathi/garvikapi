using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class Module
    {
        public string modulecode { get; set; }
        public string modulename { get; set; }

        public string moduledescription { get; set; }
        public static IDictionary<string, string> ConverttoJson(Module obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("modulecode", obj.modulecode);
            return objDict;
        }

    }
}
