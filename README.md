# CatchAll10API

A small ASP.NET API that catches all kind of requests (GET, POST, PUT, DELETE, etc.) across any route, and then return success (200); built on .NET 10 platform

## Requirements

- .NET 10 SDK

## Request handling mode

This API can run in two ways: using a controller (`CatchAll10Controller.cs`) or directly in `Program.cs`. Set the `UseController` variable in `appsettings.json` to select the implementation:

- `true` (default): use the MVC catch-all controller in `Controllers/CatchAll10Controller.cs`.
- `false`: use the catch-all minimal API handler in `Program.cs`.

Both modes handle common HTTP methods across routes. Their response shapes differ slighty.

## Run

From the repository directory:

```sh
dotnet run --launch-profile http
```

The HTTP launch profile listens at `http://localhost:5164`. The `https` launch profile also listens at `https://localhost:7200` and requires a trusted development certificate.

- Any URL located under http://localhost:5164/ or https://localhost:7200/ will be handled and will return success, you can customize specific URLs at the "CatchAll()" function, for example:

http://localhost:5164/CatchAll10API/demo/route/?dummyparam=1

https://localhost:7200/CatchAll10API/demo/route/?dummyparam=1

## Examples

```sh
curl -i http://localhost:5164/CatchAll10API
curl -i http://localhost:5164/demo/route?dummyparam=1
curl -i http://localhost:5164/dummyjson/
curl -i -X POST http://localhost:5164/api/CatchAll10API -H "Content-Type: text/plain" --data "hello world"
curl -i -X POST http://localhost:5164/api/CatchAll10API -H "Content-Type: text/json" --data "{id:1}"
curl -i -X PUT "http://localhost:5164/api/whatEver?xx=1" --data "update-me"
curl -i -X DELETE http://localhost:5164/blahblah/CatXXX
```
