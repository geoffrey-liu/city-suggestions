using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationSuggestion.Models
{
    public class LocationMatch : Location
    {
        public double Score { get; }

        public LocationMatch(string name, float latitude, float longitude, double score) : base(name, latitude, longitude)
        {
            Score = score;
        }
    }
}
