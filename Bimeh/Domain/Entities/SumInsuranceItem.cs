using System;
using System.Collections.Generic;

namespace Bimeh.Domain.Entities;

public partial class SumInsuranceItem
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? InsuranceId { get; set; }

    public virtual InsuranceCoverage? Insurance { get; set; }

    public virtual User? User { get; set; }
}
