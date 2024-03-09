using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class Agency
    {
        public string agencycode { get; set; }
        public string AgencyName { get; set; }
        public string RegistrationNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Facebookpage { get; set; }
        public string Linkedinpage { get; set; }
        public string ContactNumber { get; set; }
        public string AnotherContactNumber { get; set; }
        public DateTime Registrationdate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }

        public GarvikAddress[] Addresses { get; set; }
        public AgencyDocuments[] Documents { get; set; }
        public Car[] AttachedCars { get; set; }

        public Driver[] AttachedDrivers { get; set; }
        public bool approved { get; set; }
        public string agencycategory { get; set; }
        public string businessentitycode { get; set; }


        public static IDictionary<string, string> ConverttoJson(Agency obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("agencyname", obj.AgencyName);
            objDict.Add("registrationnumber", obj.RegistrationNumber);
            objDict.Add("firstname", obj.FirstName);
            objDict.Add("middlename", obj.MiddleName);
            objDict.Add("lastname", obj.LastName);
            objDict.Add("email", obj.Email);
            objDict.Add("facebookpage", obj.Facebookpage);
            objDict.Add("linkedinpage", obj.Linkedinpage);
            objDict.Add("website", obj.Website);
            objDict.Add("contactnumber", obj.ContactNumber);
            objDict.Add("anothercontactnumber", obj.AnotherContactNumber);
            objDict.Add("registrationdate", obj.Registrationdate.ToString());
            objDict.Add("approved", obj.approved.ToString());
            return objDict;
        }
    }
}
