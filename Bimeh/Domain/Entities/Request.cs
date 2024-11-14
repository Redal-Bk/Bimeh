using System;
using System.Collections.Generic;

namespace Bimeh.Domain.Entities;

public partial class Request
{
    public int Id { get; set; }

    public string RequestTitle { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public int? UserId { get; set; }

    public int InsuranceCoverageId { get; set; }

    public int? TrackingNumber { get; set; }
    
    public int Price { get; set; }
    public virtual InsuranceCoverage[] InsuranceCoverage { get; set; } = null!;

    public virtual User? User { get; set; }
}
