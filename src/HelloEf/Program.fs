open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.EntityFrameworkCore
open HelloEf.Models
open System.Net.Http.Headers
open Microsoft.Extensions.Logging.Console
open Microsoft.Extensions.Logging

let createContext connectionString =
    let collection = ServiceCollection().AddDbContext<MyContext>(fun options ->
        let provider = new ConsoleLoggerProvider((fun _ level -> level <> LogLevel.Debug), true)
        let factory = new LoggerFactory([provider])
        options.UseLoggerFactory(factory)  |> ignore
        options.UseNpgsql (connectionString: string) |> ignore
    )
    let provider = collection.BuildServiceProvider()
    provider.GetService<MyContext>()

let insertStudents (context: MyContext) students =
    context.Students.AddRange students |> ignore
    context.SaveChanges()

let findStudentsByGpa (context: MyContext) gpa =
    query {
        for student in context.Students do
        where (student.Gpa >= gpa)
        select student
    }

[<EntryPoint>]
let main argv =
    let connectionString = "Host=localhost;User Id=postgres;Password=1234;Database=MyDB"
    use context = createContext connectionString

    context.Database.EnsureDeleted() |> ignore
    context.Database.EnsureCreated() |> ignore

    let students = [|
        { Id = 0; Name = "A"; Gpa = 3.5 }
        { Id = 0; Name = "B"; Gpa = 2.0 }
        { Id = 0; Name = "C"; Gpa = 3.8 }
    |]
    insertStudents context students |> ignore

    let clevers = findStudentsByGpa context 3.5
    for student in clevers do
        printfn "Name = %s, Gpa = %.2f" student.Name student.Gpa

    0