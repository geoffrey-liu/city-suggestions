using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LocationSuggestion.Models;
using Microsoft.Extensions.FileProviders;
using NUnit.Framework;

namespace LocationSuggestion.Tests
{
    [TestFixture]
    public class LocationSuggestionTests
    {
        private LocationPersistence locationPersistence = null;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            locationPersistence = LocationPersistence.Instance;
        }

        [Test]
        public void GetLocationsWithCoordsTest()
        {
            List<Location> locations = locationPersistence.GetLocations("Loca", 19.0f, -100.0f).ToList();

            Assert.AreEqual(2, locations.Count);

            LocationMatch match1 = (LocationMatch)locations.Find(location => location.Name.Contains("Location1"));
            LocationMatch match2 = (LocationMatch)locations.Find(location => location.Name.Contains("Location2"));

            Assert.Greater(match2.Score, match1.Score);
        }

        [Test]
        public void GetLocationsWithoutCoordsTest()
        {
            List<Location> locations = locationPersistence.GetLocations("a", null, null).ToList();

            Assert.AreEqual(3, locations.Count);
        }
    }
}
