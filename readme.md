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
        "Key": "key"
      }
    ]
  }
```