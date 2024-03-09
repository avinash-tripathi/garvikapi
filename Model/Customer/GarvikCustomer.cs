using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Customer
{
    public class GarvikCustomer
    {
        public string customercode { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string preferredlanguage { get; set; }
        public bool mobilenoverified { get; set; }
        public bool emailverified { get; set; }

        public string createdby { get; set; }

       


        public static IDictionary<string, string> ConverttoJson(GarvikCustomer obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("customercode", obj.customercode);
            objDict.Add("mobileno", obj.mobileno);
            objDict.Add("email", obj.email);
            return objDict;
        }
    }
    public class GarvikCustomerBasic
    {
        public string customercode { get; set; }
        public string email { get; set; }
        public string   firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string preferredlanguage { get; set; }


        public static IDictionary<string, string> ConverttoJson(GarvikCustomerBasic obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("customercode", obj.customercode);
          
            objDict.Add("email", obj.email);
            objDict.Add("firstname", obj.firstname);
            objDict.Add("middlename", obj.middlename);
            objDict.Add("lastname", obj.lastname);
            objDict.Add("preferredlanguage", obj.preferredlanguage);
            return objDict;
        }
    }
    public class GarvikCustomerSettings
    {
        public string customercode { get; set; }
        public string homelocation { get; set; }
        public string worklocation { get; set; }
        public bool twostepverification { get; set; }
        
        public static IDictionary<string, string> ConverttoJson(GarvikCustomerSettings obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("customercode", obj.customercode);
            objDict.Add("homelocation", obj.homelocation);
            objDict.Add("worklocation", obj.worklocation);
            return objDict;
        }
    }
}
