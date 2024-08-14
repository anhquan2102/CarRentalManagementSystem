using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int? Point { get; set; }

    public int? RankLevel { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();

    public virtual RankLevelCustomer? RankLevelNavigation { get; set; }
}
