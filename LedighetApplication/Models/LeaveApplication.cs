using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LedighetApplication.Areas.Identity.Data;

namespace LedighetApplication.Models
{
    public class LeaveApplication
    {
        [Key]
        public int LeaveApplicationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        // Foreign key to EmployeeUser
        public string EmployeeUserId { get; set; }
        [ForeignKey("EmployeeUserId")]
        public virtual EmployeeUser? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType? LeaveType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
