# az-func-weather-app
### Azure functions app for fetching weather data from an external API and displaying it.

Task was to fetch weather data for Turku, Finland from an external API, and then exposing an API where this data could be retrieved, given a timestamp. 

The fetching is done through https://api.open-meteo.com, with config values for Turku coordinates. It's done in WeatherPoller, a timer triggered function doing the fetch once every hour. The data is stored in an Azure SQL database. 

To make a GET request, you need to provide a "timestamp" query variable. Example request:
https://weatherfunctions333.azurewebsites.net/api/temperature?timestamp=2025-08-27T17:00:00Z&code=[Ping me if you want the key to test this function]

Example response:
{"TimestampUtc":"2025-08-27T17:00:00","TemperatureC":15.9}

