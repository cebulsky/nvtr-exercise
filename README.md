# Pulling Jokes

This application is pulling jokes from https://api.chucknorris.io/ JSON api and storing it in sqlite database
Application do not store duplicated jokes
Application filters out jokes longer than configured max length (see appsettings.json file)

Entry point of application is Time Trigerred AzureFunction

# Required configuration:

cron expression of funtion trigger can be changed and needs to be set up, to do that please ensure that running locally or on Azure you do have settings With CronExpression value;
Sample local.settings.json:

```json
{
  "IsEncrypted": false,
  "Values": {
    "CronExpression": "0/10 * * * * *",
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```

# Additional configuration:

you can control endpoint api (base URL), sqlite file path, max jokes length to filter and amount of jokes to pull on every function run

appsettings.json
```json
{

  "DatabaseSettings": {
    "ConnectionString": "Data Source=.\\jokesDatabase.sqlite"
  },
  "JokesProviderSettings": {
    "EndpointUrl": "https://api.chucknorris.io/"
  },
  "JokesFilterSettings": {
    "MaxJokeLength": 200
  },
  "ApplicationSettings": {
    "JokesToPullAmount": 5
  }
}
```
