using System;
using System.Collections.Generic;

namespace HumanResourcesDBFirst.Models
{
    public class DepartmentStatViewModel
    {
        public string DepartmentName { get; set; } = null!;
        public int EmployeeCount { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal TotalSalary { get; set; }
    }

    public class EmployeeDetailReportViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateOnly? HireDate { get; set; }
    }

    public class ReportsViewModel
    {
        public List<DepartmentStatViewModel> DepartmentStats { get; set; } = new();
        public List<EmployeeDetailReportViewModel> EmployeeDetails { get; set; } = new();
    }
}
