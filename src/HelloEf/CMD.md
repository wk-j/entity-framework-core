## Command

```bash
dotnet new console --language F# --output src/HelloEf
dotnet add src/HelloEf package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add src/HelloEf package Microsoft.EntityFrameworkCore
dotnet add src/HelloEf package Microsoft.Extensions.DependencyInjection
```