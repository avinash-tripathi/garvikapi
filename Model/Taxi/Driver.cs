using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class Driver
    {
        public string drivercode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AgencyCode { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }

        public DateTime ModifyDate { get; set; }
        public string ModifiedBy { get; set; }

        public GarvikAddress[] Addresses { get; set; }
        public DriverDocuments[] Documents { get; set; }
        public Car[] AttachedCars { get; set; }

        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool approved { get; set; }
        public string drivercategory { get; set; }



        public static IDictionary<string, string> ConverttoJson(Driver obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("firstname", obj.FirstName);
            objDict.Add("middlename", obj.MiddleName);
            objDict.Add("lastname", obj.LastName);
            objDict.Add("email", obj.Email);
            objDict.Add("contactnumber", obj.ContactNumber);
            objDict.Add("dateofbirth", obj.DateOfBirth.ToString());
            objDict.Add("agencycode", obj.AgencyCode);
            objDict.Add("approved", obj.approved.ToString());

            return objDict;
        }

    }
}
