using System;
using System.Collections.Generic;

namespace HumanResourcesDBFirst.Models;

public partial class Leave
{
    public int LeaveId { get; set; }

    public int? EmployeeId { get; set; }

    public int? LeaveTypeId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual LeaveType? LeaveType { get; set; }
}
