# PracticalNCode

ASP.NET Core MVC sample using SQL Server and Dapper.

- `src/PracticalNCode.Web`: MVC presentation and composition root.
- `src/PracticalNCode.Application`: use case and service/repository contracts, service implementation, and list DTO.
- `src/PracticalNCode.Domain`: core entities.
- `src/PracticalNCode.Infrastructure`: Dapper and SQL Server implementations.
- `database`: manually executed schema and seed SQL.

Run `database\Test.AiModels.sql` in SSMS, confirm `Server=Mayur` in the Web project's appsettings, then run `dotnet restore PracticalNCode.sln` and `dotnet run --project src/PracticalNCode.Web`.
