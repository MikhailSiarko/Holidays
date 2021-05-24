# Holidays

## Simple service providing public holidays for several countries (based on Enrico Service 2.0).

### API

* */api/countries* - list of available countries
* */api/holidays/{year}/{country}* - holidays list grouped by a month for a given country and year
* */api/holidays/{year}/{country}/max-free-days-in-row* - the maximum number of free (free day + holiday) days in a row for a given country and year
* */api/day/{day}-{month}-{year}/{country}/status* - specific day status (workday, free day, holiday)

### Development

#### Requirements
* .NET 5
* Docker
* Your favourite IDE or text editor

#### Starting the application

* Run *build.sh* on MacOS or Linux
* Run *build.ps1* on Windows
