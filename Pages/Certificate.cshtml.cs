using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class CertificateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CertificateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Certificate? Certificate { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            Certificate = await _context.Certificates
                .Include(c => c.Assessment)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(c =>
                    c.CertificateId == id &&
                    c.StudentId == studentId.Value);

            if (Certificate == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}