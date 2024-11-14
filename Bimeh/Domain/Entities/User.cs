using System;
using System.Collections.Generic;

namespace Bimeh.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Token { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<SumInsuranceItem> SumInsuranceItems { get; set; } = new List<SumInsuranceItem>();
}
