using LedighetApplication.Areas.Identity.Data;
using LedighetApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LedighetApplication.Data;

public class ApplicationDbContext : IdentityDbContext<EmployeeUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Department> Departments { get; set; }
	public DbSet<LeaveApplication> LeaveApplications { get; set; }
	public DbSet<LeaveType> LeaveTypes { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		// adding standard inputs for Department
		builder.Entity<Department>().HasData(
			new Department { DepartmentId = 1, Name = "HR" },
			new Department { DepartmentId = 2, Name = "IT" },
			new Department { DepartmentId = 3, Name = "Finance" },
			 new Department { DepartmentId = 4, Name = "Not Chosen", IsDefault = true }
		);

		// adding standard inputs for LeaveType
		builder.Entity<LeaveType>().HasData(
			new LeaveType { LeaveTypeId = 1, Name = "Annual" },
			new LeaveType { LeaveTypeId = 2, Name = "Sick" },
			new LeaveType { LeaveTypeId = 3, Name = "Maternity" },
			new LeaveType { LeaveTypeId = 4, Name = "Paternity" },
			new LeaveType { LeaveTypeId = 5, Name = "HateWork" },
			new LeaveType { LeaveTypeId = 6, Name = "Other" }
		);
	}
}
