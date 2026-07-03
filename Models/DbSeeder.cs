using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResourcesDBFirst.Models
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Seed Roles
            string adminRole = "Admin";
            string employeeRole = "Employee";
            
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            
            if (!await roleManager.RoleExistsAsync(employeeRole))
                await roleManager.CreateAsync(new IdentityRole(employeeRole));

            // 2. Seed Admin User
            string adminEmail = "admin@hr.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            // 3. Seed generic Employee User
            string emp1Email = "employee@hr.com";
            var emp1User = await userManager.FindByEmailAsync(emp1Email);
            if (emp1User == null)
            {
                emp1User = new IdentityUser { UserName = emp1Email, Email = emp1Email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(emp1User, "Employee123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(emp1User, employeeRole);
                }
            }

            // Seed generic Employee record in database
            var emp1Record = context.Employees.FirstOrDefault(e => e.Email == emp1Email);
            if (emp1Record == null)
            {
                var firstDept = context.Departments.FirstOrDefault();
                var firstPos = context.Positions.FirstOrDefault();
                emp1Record = new Employee
                {
                    FirstName = "Test",
                    LastName = "Çalışan",
                    Email = emp1Email,
                    Phone = "555-0001",
                    HireDate = DateOnly.FromDateTime(DateTime.Now),
                    Salary = 45000,
                    DepartmentId = firstDept?.DepartmentId,
                    PositionId = firstPos?.PositionId,
                    IsDeleted = false
                };
                context.Employees.Add(emp1Record);
            }

            // 4. Seed Ahmet Yılmaz Employee User
            string emp2Email = "ahmet.yilmaz@hr.com";
            var emp2User = await userManager.FindByEmailAsync(emp2Email);
            if (emp2User == null)
            {
                emp2User = new IdentityUser { UserName = emp2Email, Email = emp2Email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(emp2User, "Ahmet123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(emp2User, employeeRole);
                }
            }

            // Seed Ahmet Yılmaz Employee record in database
            var emp2Record = context.Employees.FirstOrDefault(e => e.Email == emp2Email);
            if (emp2Record == null)
            {
                var firstDept = context.Departments.FirstOrDefault();
                var firstPos = context.Positions.FirstOrDefault();
                emp2Record = new Employee
                {
                    FirstName = "Ahmet",
                    LastName = "Yılmaz",
                    Email = emp2Email,
                    Phone = "555-0002",
                    HireDate = DateOnly.FromDateTime(DateTime.Now),
                    Salary = 55000,
                    DepartmentId = firstDept?.DepartmentId,
                    PositionId = firstPos?.PositionId,
                    IsDeleted = false
                };
                context.Employees.Add(emp2Record);
            }

            await context.SaveChangesAsync();
        }
    }
}
