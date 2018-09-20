#! "netcoreapp2.1"

#r "nuget:Dapper,1.50.5"
#r "nuget:Npgsql,4.0.3"

using System;
using Dapper;
using Npgsql;

var str = "Host=localhost;User Id=postgres;Password=1234;Database=DynamicModel";
var connection = new NpgsqlConnection(str);
connection.Open();
var rs = connection.Query<dynamic>(@"select * from public.""A""");


foreach (var item in rs) {
    Console.WriteLine(item.LastName.ToString());
    Console.WriteLine(item.Name.ToString());
}