using System;
using System.Collections.Generic;

namespace Bimeh.Domain.Entities;

public partial class InsuranceCoverage
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int MinPrice { get; set; }

    public int MaxPrice { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<SumInsuranceItem> SumInsuranceItems { get; set; } = new List<SumInsuranceItem>();
}
