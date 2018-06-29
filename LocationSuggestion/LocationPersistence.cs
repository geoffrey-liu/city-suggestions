using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using LocationSuggestion.Models;
using Microsoft.Extensions.FileProviders;

namespace LocationSuggestion
{
    public class LocationPersistence
    {
        private readonly string CITY_INFORMATION_FILE_NAME = "cities_canada-usa.tsv";

        private static readonly double nameScoreWeight = 0.3;
        private static readonly double distanceScoreWeight = 1 - nameScoreWeight;

        private enum Province
        {
            AB = 1,
            BC,
            MB,
            NB,
            NL,
            NS,
            NT,
            ON,
            PE,
            QC,
            SK,
            YT
        }

        private Dictionary<long, Location> locationDb = new Dictionary<long, Location>();   // Using a simple data structure to simulate a database
        private static LocationPersistence instance = null;

        public static LocationPersistence Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocationPersistence();
                }
                return instance;
            }
        } 

        private LocationPersistence()
        {
            InitializeLocationList();
        }

        /*
         * Returns a list of locations. If latitude and longitude are both null, then this function returns a list of locations that contain the city name as a substring (with no score).
         * If no query is made for a city name substring, then this function returns a list of all the cities in the database.
         */
        public IEnumerable<Location> GetLocations(string cityName, float? latitude, float? longitude)
        {
            List<Location> locations = locationDb.Select(location => location.Value).ToList();

            if (String.IsNullOrEmpty(cityName))
            {
                return locations; // Return list of all locations if city name is left empty
            }
            else
            {
                return GetMatchingLocations(cityName, latitude, longitude);
            }
        }

        /* 
         * Returns a list of locations that contain the city name and have the latitude and/or longitude specified.
         * A score is provided to determine how closely the city matches the query.
         */ 
        private IEnumerable<Location> GetMatchingLocations(string inputCityName, float? latitude, float? longitude)
        {
            List<Location> matchingLocations = locationDb.Select(location => location.Value).Where(location => location.Name.Contains(inputCityName)).ToList();
            List<LocationMatch> results = new List<LocationMatch>();

            LocationMatch locationMatch = null;
            foreach (LocationInfo location in matchingLocations) {
                double score = CalculateScore(location, inputCityName, latitude, longitude);
                locationMatch = new LocationMatch(location.GetFullLocationString(), location.Latitude, location.Longitude, score);
                results.Add(locationMatch);
            }

            return results.OrderByDescending(location => location.Score).ToList();
        }

        /*
         * Calculates a score to determine how closely the city matches the query input by the user.
         */
        private double CalculateScore(LocationInfo location, string inputCityname, float? inputLatitude, float? inputLongitude)
        {
            double nameScore = (double)(inputCityname.Length) / (location.Name.Length); // Calculate score as a percentage of the string match at first

            if (inputLatitude.Equals(null) && inputLongitude.Equals(null))
            {
                return nameScore; // If longitude and latitude are both null, return the score based solely on name
            }

            nameScore = nameScore * nameScoreWeight; // Allocate percentage of score to name string match

            // If only exactly one value between the longitude and latitude was ommitted, set that value to 0 and use that as a default value for the rest of the score calculation
            double queryLatitude = inputLatitude ?? 0;
            double queryLongitude = inputLongitude ?? 0;

            // Clamp values to avoid calulating with nonsensical values (i.e. -190 longitude)
            queryLatitude = (float) Math.Clamp(queryLatitude, -90.0, 90.0);
            queryLongitude = (float) Math.Clamp(queryLongitude, -180.0, 180.0);

            double distanceScore = distanceScoreWeight;

            float locationLatitude = location.Latitude;
            float locationLongitude = location.Longitude;
            double squareVerticalDistance = Math.Pow((locationLatitude - queryLatitude), 2);
            double squareHorizontalDistance = Math.Pow((locationLongitude - queryLongitude), 2);

            double cartesianDistance = Math.Sqrt(squareVerticalDistance + squareHorizontalDistance); // Distance between input coordinates and city's coordinates using Pythagoras

            distanceScore *= Math.Exp(-0.15 * cartesianDistance); // Use an exponential function to scale the distance score according to how far it is (quadratic didn't work as well)
            
            return Math.Round((nameScore + distanceScore), 2);
        }

        /*
         * Initializes the "database" list of cities. Only done once.
         */
        private void InitializeLocationList()
        {
            IFileProvider provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            IDirectoryContents contents = provider.GetDirectoryContents(""); // Application root
            IFileInfo fileInfo = provider.GetFileInfo(CITY_INFORMATION_FILE_NAME);

            LocationInfo location;
            foreach (var line in File.ReadLines(fileInfo.Name).Skip(1))
            {
                var currentLine = line.Split('\t');
                long id = Convert.ToInt64(currentLine[0]);
                string name = currentLine[1];
                float latitude = float.Parse(currentLine[4]);
                float longitude = float.Parse(currentLine[5]);
                string country = currentLine[8];
                string provinceStateCode = String.Empty;
                if (int.TryParse(currentLine[10], out int fips))
                {
                    provinceStateCode = ((Province)fips).ToString();
                }
                else
                {
                    provinceStateCode = currentLine[10];
                }
                long population = long.Parse(currentLine[14]);
                location = new LocationInfo(name, latitude, longitude, country, provinceStateCode, population);
                locationDb.Add(id, location);
            }
        }
    }
}
 