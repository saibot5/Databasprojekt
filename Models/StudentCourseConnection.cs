using System;
using System.Collections.Generic;

namespace Databasprojekt.Models;

public partial class StudentCourseConnection
{
    public int FkstudentId { get; set; }

    public int FkcourceId { get; set; }

    public int? FkgradeId { get; set; }

    public DateOnly? GradeDate { get; set; }

    public virtual Course Fkcource { get; set; } = null!;

    public virtual Grade? Fkgrade { get; set; }

    public virtual Student Fkstudent { get; set; } = null!;
}
