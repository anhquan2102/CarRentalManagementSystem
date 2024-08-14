using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Car
{
    public string LicensePlates { get; set; } = null!;

    public string CarName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateOnly? DateCar { get; set; }

    public string Color { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public decimal Price { get; set; }

    public int NumberOfSeats { get; set; }

    public int? CarStatusId { get; set; }

    public string Fuel { get; set; } = null!;

    public decimal RentalPrice { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CarRental> CarRentals { get; set; } = new List<CarRental>();

    public virtual CarStatus? CarStatus { get; set; }
}
