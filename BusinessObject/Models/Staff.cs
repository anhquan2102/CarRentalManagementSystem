using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Email { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public decimal? Salary { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();
}
