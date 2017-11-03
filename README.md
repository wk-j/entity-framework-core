## Entity Framework Core

```
dotnet add StringInterpolation/StringInterpolation.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

// Not test
dotnet add StringInterpolation/StringInterpolation.csproj package Microsoft.EntityFrameworkCore.SqlServer

// Not support EF Core 2.0
dotnet add StringInterpolation/StringInterpolation.csproj package MySql.Data.EntityFrameworkCore -v 8.0.9-dmr  
```

### MySQL

```
brew install mysql
brew tap homebrew/services
brew services start mysql
mysql -V

mysql_secure_installation
```