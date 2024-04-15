using LedighetApplication.Areas.Identity.Data;
using LedighetApplication.Models;
using System.Collections.Generic;

namespace LedighetApplication.ViewModels
{
    public class UserApplicationsViewModel
    {
        public EmployeeUser User { get; set; }
        public IEnumerable<LeaveApplication> Applications { get; set; }
    }
}
