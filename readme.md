# Shuttle.Packager

Provides a Web API and a Vue front-end for managing NuGet packages.

The structure for the settings is as follows:

```json
  "Packager": {
    "BaseFolder": "<search for .csproj file from here, recursively>",
    "VisualStudioPath": "<path to visual studio C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Common7\\IDE\\devenv.exe>",
    "PackageSources": [
      {
        "Name": "my-packages",
        "Key": "key",
        "Url": "https://my-feed.example.com/v3/index.json"
      },
      {
        "Name": "local",
        "Url": "D:\\packages\\local-feed"
      }
    ]
  }
```

`Url` identifies where a package source's packages actually live and is used to look up the latest published
version of a project (via the "get package version" action) — it accepts either a NuGet V3 service index URL
(e.g. `https://api.nuget.org/v3/index.json`) or a local folder path used as a NuGet feed (e.g. one added with
`dotnet nuget add source <folder>` for locally-built development packages). When the version check is run
without selecting a package source, it defaults to nuget.org. `Key` is only required for sources you push to.