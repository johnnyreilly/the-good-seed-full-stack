# Seed Tools

## Pre-Requisites

- Install [node](https://nodejs.org/en/).
- Install [seq](https://getseq.net/)

## Developing seed-tools:

To get the code clone the repo.

The docs are going to assume you've cloned it at this location: `c/work/` and that you're using bash for your shell.

To get the ASP.Net Core server installed:

```sh
cd /c/work/seed-tools/src
dotnet restore
```

To get the database into a running state:

```sh
cd /c/work/seed-tools/src/Seed.Data
dotnet ef database update
```

This should create these 2 files in the root of the project:

- `seed-tools.mdf`
- `seed-tools_log.ldf`

To debug the ASP.Net Core server you should open up VS Code at this location: `c:\work\seed-tools\src` and hit F5.  

To run the ASP.Net Core server you should:


```sh
cd /c/work/seed-tools/src/Seed.Web
dotnet run
```

This will serve at http://localhost:5000 - you can check status here: http://localhost:5000/api/status

To get React client installed:

```sh
cd /c/work/seed-tools/src/Seed.App
yarn install
```

To run React client:

```sh
cd /c/work/seed-tools/src/Seed.App
yarn start
```

This will serve at http://localhost:8080 and will talk to http://localhost:5000 for data (so you need both running).