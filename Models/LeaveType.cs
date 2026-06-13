using System;
using System.Collections.Generic;

namespace HumanResourcesDBFirst.Models;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string? LeaveTypeName { get; set; }

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
}
