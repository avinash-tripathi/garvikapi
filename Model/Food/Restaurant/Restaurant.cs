using GARVIKService.Model.Taxi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GARVIKService.Model.Food.Restaurant
{
    public class Restaurant
    {
        // General Information
        public string restaurantcode { get; set; }
        public string RestaurantName { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string Category { get; set; }
        public string Cuisine { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public GarvikAddress[] Addresses { get; set; }
        public Documents[] Documents { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Operating Hours
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }

        // Additional Information
        public bool HasDeliveryService { get; set; }
        public bool HasTakeawayService { get; set; }
        public bool HasDineInService { get; set; }
        public bool IsVegetarianFriendly { get; set; }
        public bool IsVeganFriendly { get; set; }
        public bool IsHalalCertified { get; set; }

        // Payment Options
        public bool AcceptsCash { get; set; }
        public bool AcceptsCard { get; set; }
        public bool AcceptsOnlinePayment { get; set; }

        // Ratings and Reviews
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }

        // Other Features
        public bool HasWifi { get; set; }
        public bool HasParking { get; set; }
        public bool IsAlcoholServed { get; set; }
        public bool HasOutdoorSeating { get; set; }

        // Operational Status
        public bool IsOpen { get; set; }

        // Registration Information
        public DateTime RegistrationDate { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegistrationCertificateUrl { get; set; }
        public string CreatedBy { get; set; }
        public string modulecode { get; set; }
    }

}
