using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationSuggestion.Models
{
    public class LocationInfo : Location
    {
        public string Country { get; }
        public string ProvinceStateCode { get; }
        public long Population { get; }

        public LocationInfo(string name, float latitude, float longitude, string country, string provinceStateCode, long population) : base(name, latitude, longitude)
        {
            Country = country;
            ProvinceStateCode = provinceStateCode;
            Population = population;
        }

        public string GetFullLocationString()
        {
            return Name + ", " + ProvinceStateCode + ", " + Country;
        }
    }
}