using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Taxi
{
    public class GarvikReview
    {
        public string reviewcode { get; set; }
        public string bookingcode { get; set; }

        public string referencecode { get; set; }
        public float rating { get; set; }
        public string comment { get; set; }
        public string referencecategory { get; set; }
        public Booking bookinginfo { get; set; }
        public string reviewdate { get; set; }


        public static IDictionary<string, string> ConverttoJson(GarvikReview obj)
        {
            IDictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("reviewcode", obj.reviewcode);
            objDict.Add("bookingcode", obj.bookingcode);
            objDict.Add("referencecode", obj.referencecode);
            objDict.Add("rating", obj.rating.ToString());
            objDict.Add("comment", obj.comment);
            objDict.Add("referencecategory", obj.referencecategory);

            return objDict;
        }

    }
}
