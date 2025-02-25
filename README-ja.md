# WrappedSTJson

System.Text.Jsonの一部をラップして使いやすくしたものです。

使い方は WrappedSTJson.Test 以下を参照してください。

## build の方法

```bash
dotnet build WrappedSTJson\
```

## test の方法

```bash
dotnet run --project WrappedSTJson.Test\
```

## local package の使い方

```bash
dotnet pack -c Release WrappedSTJson\
dotnet nuget list source  # localでsourceを追加したことがない場合
dotnet nuget add source <absolute directory path> -n <source name>  # localでsourceを追加したことがない場合
dotnet nuget list source  # localでsourceを追加したことがない場合
dotnet nuget push -s <source name> WrappedSTJson\bin\Release\WrappedSTJson.<version>.nupkg
dotnet add package -s <source name> WrappedSTJson  # パッケージを追加するとき
```
