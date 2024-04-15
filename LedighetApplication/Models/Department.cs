using LedighetApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LedighetApplication.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
		public bool IsDefault { get; set; } 

		// Navigation 
		public virtual ICollection<EmployeeUser>? Employees { get; set; }
    }
}
