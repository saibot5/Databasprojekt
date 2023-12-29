using System;
using System.Collections.Generic;

namespace Databasprojekt.Models;

public partial class Student
{
    public int StudendId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Pnumber { get; set; }

    public int? FkclassId { get; set; }

    public virtual Class? Fkclass { get; set; }
}
