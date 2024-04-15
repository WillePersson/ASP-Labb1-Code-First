using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LedighetApplication.Data;
using LedighetApplication.Areas.Identity.Data;
using LedighetApplication.Models;
using LedighetApplication.ViewModels;
using System.Globalization;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<EmployeeUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context, UserManager<EmployeeUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Dashboard view showing options
    public IActionResult Index()
    {
        var users = _userManager.Users
                                .OrderBy(u => u.FirstName)
                                .ThenBy(u => u.LastName)
                                .Select(u => new SelectListItem
                                {
                                    Value = u.Id,
                                    Text = u.FirstName + " " + u.LastName
                                }).ToList();

        ViewBag.Users = new SelectList(users, "Value", "Text");
        return View();
    }

    // POST: Display leave applications for a specific user
    [HttpPost]
    public async Task<IActionResult> DisplayUserApplications(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var applications = await _context.LeaveApplications
                                         .Include(l => l.LeaveType)
                                         .Where(l => l.EmployeeUserId == userId)
                                         .ToListAsync();

        var viewModel = new UserApplicationsViewModel
        {
            User = user,
            Applications = applications
        };

        return View(viewModel);
    }

    public async Task<IActionResult> SearchByMonth(int year, int month)
    {
        var applications = await _context.LeaveApplications
            .Include(l => l.LeaveType)
            .Include(l => l.Employee)  // Make sure to include the Employee
            .Where(l => l.CreatedAt.Year == year && l.CreatedAt.Month == month)
            .Select(a => new LeaveApplicationDaysModel
            {
                LeaveTypeName = a.LeaveType.Name,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                FirstName = a.Employee.FirstName,  // Assuming Employee is a navigation property
                LastName = a.Employee.LastName
            }).ToListAsync();

        return View("ApplicationsByMonth", applications);
    }
}
