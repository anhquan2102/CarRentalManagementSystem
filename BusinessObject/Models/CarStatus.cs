using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class CarStatus
{
    public int CarStatusId { get; set; }

    public string CarStatusName { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
