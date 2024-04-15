using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LedighetApplication.Data;
using LedighetApplication.Models;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LedighetApplication.Areas.Identity.Data;

namespace LedighetApplication.Controllers
{
    [Authorize]
    public class LeaveApplicationsController : Controller
    {
        private readonly UserManager<EmployeeUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeaveApplicationsController> _logger;

        public LeaveApplicationsController(ApplicationDbContext context, UserManager<EmployeeUser> userManager, ILogger<LeaveApplicationsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); // Get the current logged-in user's ID
            var applicationDbContext = _context.LeaveApplications
                                               .Include(l => l.Employee)
                                               .Include(l => l.LeaveType)
                                               .Where(l => l.EmployeeUserId == userId); // Filter to show only this user

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.LeaveApplicationId == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        private void PopulateDropDownLists()
        {
            ViewBag.LeaveTypeId = new SelectList(_context.LeaveTypes, "LeaveTypeId", "Name");
        }

        // GET: LeaveApplications/Create
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,Description,LeaveTypeId")] LeaveApplication leaveApplication)
        {
            leaveApplication.EmployeeUserId = _userManager.GetUserId(User); // Assign user ID

            if (string.IsNullOrEmpty(leaveApplication.EmployeeUserId))
            {
                ModelState.AddModelError("", "Cannot identify the user making this request.");
            }
            else
            {
                ModelState.Remove("EmployeeUserId");
            }

            if (ModelState.IsValid)
            {
                leaveApplication.CreatedAt = DateTime.UtcNow; 
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDownLists();
            return View(leaveApplication);
        }

        private void DebugModelStateErrors()
        {
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    Debug.WriteLine($"ModelState error in {entry.Key}:");
                    foreach (var error in entry.Value.Errors)
                    {
                        Debug.WriteLine($" - {error.ErrorMessage}");
                    }
                }
            }
        }
        private void LogModelStateErrors()
        {
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    _logger.LogError($"ModelState error in {entry.Key}:");
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogError($" - {error.ErrorMessage}");
                    }
                }
            }
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["EmployeeUserId"] = new SelectList(_context.Users, "Id", "Id", leaveApplication.EmployeeUserId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "LeaveTypeId", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveApplicationId,StartDate,EndDate,Description,EmployeeUserId,LeaveTypeId,CreatedAt")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.LeaveApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.LeaveApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeUserId"] = new SelectList(_context.Users, "Id", "Id", leaveApplication.EmployeeUserId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "LeaveTypeId", "LeaveTypeId", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.LeaveApplicationId == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.LeaveApplicationId == id);
        }
    }
}
