using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

public class Entity {
    [Key]
    public int Id { set; get; }
}
public class Student : Entity {
    public virtual string Name { get; set; }
    public virtual decimal TotalDebt { get; set; }

    public virtual IList<Enrollment> Enrollments { get; set; }
    public virtual IList<SportsActivity> SportsActivities { get; set; }
}

public class Enrollment : Entity {
    public virtual Student Student { get; set; }
    public virtual Course Course { get; set; }
    public virtual Grade Grade { get; set; }
}

public enum Grade {
    A = 1, B = 2, C = 3, D = 4, F = 5
}

public class Course : Entity {
    public virtual string Name { get; set; }
    public virtual int Credits { get; set; }
}

public class SportsActivity : Entity {
    public virtual Sports Sports { get; set; }
    public virtual DateTime PlayingSince { get; set; }
}

public class Sports : Entity {
    public virtual string Name { get; set; }
}