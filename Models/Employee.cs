using System;
using System.Collections.Generic;

namespace HumanResourcesDBFirst.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? HireDate { get; set; }

    public decimal? Salary { get; set; }

    public int? DepartmentId { get; set; }

    public int? PositionId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();

    public virtual Position? Position { get; set; }
}
