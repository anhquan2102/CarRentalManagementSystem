using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class HistoryCarRental
{
    public int HistoryCarRentalId { get; set; }

    public int? RentalId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime ActualReturnTime { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual CarRental? Rental { get; set; }
}
