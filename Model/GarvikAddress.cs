using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model
{
    public class GarvikAddress
    {
        public string BuildingNo { get; set; }
        public string AppartmentName { get; set; }
        public string Landmark { get; set; }
        public string Address1 { get; set; }
        public string Street { get; set; }
        public string CityCode { get; set; }
        public string StateCode { get; set; }
        public string Pincode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public static IDictionary<string, string> ConverttoJson(GarvikAddress obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("buildingno", obj.BuildingNo);
            objDict.Add("appartmentname", obj.AppartmentName);
            objDict.Add("landmark", obj.Landmark);
            objDict.Add("address1", obj.Address1);
            objDict.Add("street", obj.Street);
            objDict.Add("citycode", obj.CityCode);
            objDict.Add("statecode", obj.StateCode);
            objDict.Add("pincode", obj.Pincode);
            objDict.Add("latitude", obj.Latitude.ToString());
            objDict.Add("longitude", obj.Longitude.ToString());


            return objDict;
        }
    }
}
