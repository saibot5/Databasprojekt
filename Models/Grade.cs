using System;
using System.Collections.Generic;

namespace Databasprojekt.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public string GradeName { get; set; } = null!;
}
