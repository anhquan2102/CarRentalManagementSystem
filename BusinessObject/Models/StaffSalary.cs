using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class StaffSalary
{
    public int? StaffId { get; set; }

    public string StaffName { get; set; } = null!;

    public decimal Salary { get; set; }

    public int RentalCount { get; set; }

    public decimal Commission { get; set; }

    public decimal? TotalSalary { get; set; }

    public virtual Staff? Staff { get; set; }
}
