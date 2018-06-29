# City Suggestions REST Endpoint
REST API with location suggestions for Coveo backend challenge

Codebase for city suggestions REST endpoint. Code for core implementation is in the LocationSuggestion submodule located here: https://github.com/geoffrey-liu/city-location-suggestions.
I accidentally initially created the repo at the wrong level of the solution with no unit test solution.
I felt that the git commit history might have some relevance for the reviewers, so I left the project as is instead of pushing everything all at once into a new repo.

Coded using C#. There is no database hooked into the project; it stores all the cities in the tsv file into a Dictionary.
Side note: I created a base abstract Location class because of the absence of a database.

Call the endpoint like so: http://city-location-suggestion.azurewebsites.net/suggestions?q={CityName}&latitude={SomeLatitude}&longitude={SomeLongitude}

The latitude and longitude can be left blank, in which case a score is returned solely based on how well the name matches the list of locations containing that substring.
If the name is left blank, the entire list of locations will be returned with basic info about the city (no scores).

The algorithm isn't perfect (it's a little too picky and returns lower than normal scores for close cities, in my opinion).
I had another equation in mind, but it was too generous and didn't distinguish enough between close and far locations.
