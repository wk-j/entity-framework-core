open System
open Microsoft.Extensions.DependencyInjection
open HelloEf.Models
open Microsoft.EntityFrameworkCore

let createContext connectionString =
    let collection = ServiceCollection().AddDbContext<MyContext>(fun options ->
        options.UseNpgsql (connectionString: string) |> ignore
    )
    let provider = collection.BuildServiceProvider()
    provider.GetService<MyContext>()

let insertStudent (context: MyContext) student =
    context.Students.Add student |> ignore
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
    context.Database.EnsureCreated() |> ignore

    insertStudent context { Id = 0; Name = "A"; Gpa = 3.5 } |> ignore
    insertStudent context { Id = 0; Name = "B"; Gpa = 2.0 } |> ignore
    insertStudent context { Id = 0; Name = "C"; Gpa = 3.9 } |> ignore

    let clevers = findStudentsByGpa context 3.5

    for student in clevers do
        printfn "%A" student
    0