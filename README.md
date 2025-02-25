# WrappedSTJson

It wraps parts of System.Text.Json to make it easier to use.

See WrappedSTJson.Test below for usage.

## How to build

```bash
dotnet build WrappedSTJson\
```

## How to test

```bash
dotnet run --project WrappedSTJson.Test\
```

## How to use local package

```bash
dotnet pack -c Release WrappedSTJson\
dotnet nuget list source  # If you have never added a source locally
dotnet nuget add source <absolute directory path> -n <source name>  # If you have never added a source locally
dotnet nuget list source  # If you have never added a source locally
dotnet nuget push -s <source name> WrappedSTJson\bin\Release\WrappedSTJson.<version>.nupkg
dotnet add package -s <source name> WrappedSTJson  # when adding to a package
```
