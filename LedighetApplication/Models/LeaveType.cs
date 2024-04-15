using System.ComponentModel.DataAnnotations;

namespace LedighetApplication.Models
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }
        public string Name { get; set; }  

        // Navigation 
        public virtual ICollection<LeaveApplication>? LeaveApplications { get; set; }
    }
}
