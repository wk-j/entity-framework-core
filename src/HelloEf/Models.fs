namespace HelloEf.Models

open System
open Microsoft.EntityFrameworkCore
open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type Student =
    { [<Key>]
      Id: int
      Name: string
      Gpa: float }

type MyContext(options: DbContextOptions) =
    inherit DbContext(options)

    [<DefaultValue>]
    val mutable private students: DbSet<Student>

    member this.Students
        with get() = this.students
        and  set v = this.students <- v