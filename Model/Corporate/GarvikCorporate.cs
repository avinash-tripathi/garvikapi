using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Corporate
{
    public class GarvikCorporate
    {
        public string corporatecode { get; set; }
        public string companyname { get; set; }
        public string companydomain { get; set; }
        public string contactnumber { get; set; }
        public string mobilenumber { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string createdby { get; set; }
        public static IDictionary<string, string> ConverttoJson(GarvikCorporate obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("corporatecode", obj.corporatecode);
            objDict.Add("companyname", obj.companyname);
            objDict.Add("companydomain", obj.companydomain);
            objDict.Add("contactnumber", obj.contactnumber);
            objDict.Add("mobilenumber", obj.mobilenumber);
            objDict.Add("email", obj.email);
            objDict.Add("address", obj.address);
            objDict.Add("city", obj.city);
            objDict.Add("state", obj.state);
            objDict.Add("createdby", obj.createdby);

            return objDict;
        }

    }
}
