using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class Car
    {
       
        public string attachedtocode { get; set; }
        public string attachedtotype { get; set; }
        public string carcode { get; set; }
        public string make { get; set; }
        public string chassisno { get; set; }
        public string modelno { get; set; }
        public string makeyear { get; set; }
        public string rcnumber { get; set; }
        public string rcexpiredon { get; set; }
        public string permitnumber { get; set; }
        public string permitexpiredon { get; set; }
        public string insurancenumber { get; set; }
        public string insuranceexpiredon { get; set; }
        public DateTime createdate { get; set; }
        public string createdby { get; set; }
        public DateTime? modifydate { get; set; }
        public string modifyby { get; set; }
        public CarDocuments[] Documents { get; set; }
        public string ListDocuments { get; set; }

        public bool approved { get; set; }


        public static IDictionary<string, string> ConvertToJson(Car obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
           
            objDict.Add("attachedtocode", obj.attachedtocode);
            objDict.Add("attachedtotype", obj.attachedtotype);
            objDict.Add("carcode", obj.carcode);
            objDict.Add("make", obj.make);
            objDict.Add("chassisno", obj.chassisno);
            objDict.Add("modelno", obj.modelno);
            objDict.Add("makeyear", obj.makeyear);
            objDict.Add("rcnumber", obj.rcnumber);
            objDict.Add("rcexpiredon", obj.rcexpiredon);
            objDict.Add("permitnumber", obj.permitnumber);
            objDict.Add("permitexpiredon", obj.permitexpiredon);
            objDict.Add("insurancenumber", obj.insurancenumber);
            objDict.Add("insuranceexpiredon", obj.insuranceexpiredon);
            objDict.Add("approved", obj.approved.ToString());
            return objDict;
        }


    }
}
