using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSuggestion.Models
{
    public abstract class Location
    {
        public string Name { get; }
        public float Latitude { get; }
        public float Longitude { get; }

        public Location(string name, float latitude, float longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
