using LedighetApplication.Areas.Identity.Data;

namespace LedighetApplication.ViewModels
{
    public class LeaveApplicationDaysModel
    {
        public string LeaveTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Computed property to calculate total days including the end date
        public int TotalDays => (EndDate - StartDate).Days + 1;
    }
}
