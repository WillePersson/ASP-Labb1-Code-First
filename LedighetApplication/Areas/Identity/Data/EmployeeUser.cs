using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LedighetApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace LedighetApplication.Areas.Identity.Data;

public class EmployeeUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? DepartmentId { get; set; } 

    [ForeignKey("DepartmentId")]
    public virtual Department Department { get; set; }

   
    public virtual ICollection<LeaveApplication>? LeaveApplications { get; set; }
}

