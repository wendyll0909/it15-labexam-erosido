using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IT15LabExamErosido.Data;
using IT15LabExamErosido.Models;

namespace IT15LabExamErosido.Controllers
{
    public class PayrollsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public PayrollsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: Payrolls
        public async Task<IActionResult> Index()
        {
            var payrolls = _context.Payrolls.Include(p => p.Employee);
            return View(await payrolls.ToListAsync());
        }
        
        // GET: Payrolls/EmployeePayrolls/5
        public async Task<IActionResult> EmployeePayrolls(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            var payrolls = await _context.Payrolls
                .Where(p => p.EmployeeId == id)
                .Include(p => p.Employee)
                .ToListAsync();
                
            ViewBag.EmployeeName = $"{employee.FirstName} {employee.LastName}";
            ViewBag.EmployeeId = id;
            
            return View(payrolls);
        }
        
        // GET: Payrolls/Create
        public IActionResult Create(int? employeeId)
        {
            ViewBag.Employees = _context.Employees.ToList();
            if (employeeId.HasValue)
            {
                var payroll = new Payroll { EmployeeId = employeeId.Value, Date = DateTime.Today };
                return View(payroll);
            }
            return View(new Payroll { Date = DateTime.Today });
        }
        
        // POST: Payrolls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Date,DaysWorked,Deduction")] Payroll payroll)
        {
            // Validate DaysWorked is not negative
            if (payroll.DaysWorked < 0)
            {
                ModelState.AddModelError("DaysWorked", "Days worked cannot be negative");
            }
            
            // Validate Deduction is not negative
            if (payroll.Deduction < 0)
            {
                ModelState.AddModelError("Deduction", "Deduction cannot be negative");
            }
            
            // Get employee for validation
            var employee = await _context.Employees.FindAsync(payroll.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError("EmployeeId", "Please select a valid employee");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(payroll);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Payroll record added! Gross Pay: {payroll.GrossPay:C}, Net Pay: {payroll.NetPay:C}";
                return RedirectToAction(nameof(EmployeePayrolls), new { id = payroll.EmployeeId });
            }
            
            ViewBag.Employees = _context.Employees.ToList();
            return View(payroll);
        }
        
        // GET: Payrolls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll == null)
            {
                return NotFound();
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(payroll);
        }
        
        // POST: Payrolls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayrollId,EmployeeId,Date,DaysWorked,Deduction")] Payroll payroll)
        {
            if (id != payroll.PayrollId)
            {
                return NotFound();
            }
            
            // Validate DaysWorked is not negative
            if (payroll.DaysWorked < 0)
            {
                ModelState.AddModelError("DaysWorked", "Days worked cannot be negative");
            }
            
            // Validate Deduction is not negative
            if (payroll.Deduction < 0)
            {
                ModelState.AddModelError("Deduction", "Deduction cannot be negative");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payroll);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Payroll updated! New Gross Pay: {payroll.GrossPay:C}, Net Pay: {payroll.NetPay:C}";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollExists(payroll.PayrollId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(EmployeePayrolls), new { id = payroll.EmployeeId });
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(payroll);
        }
        
        // GET: Payrolls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var payroll = await _context.Payrolls
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.PayrollId == id);
            if (payroll == null)
            {
                return NotFound();
            }
            
            return View(payroll);
        }
        
        // POST: Payrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            int employeeId = payroll.EmployeeId;
            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Payroll record deleted successfully!";
            }
            return RedirectToAction(nameof(EmployeePayrolls), new { id = employeeId });
        }
        
        private bool PayrollExists(int id)
        {
            return _context.Payrolls.Any(e => e.PayrollId == id);
        }
    }
}