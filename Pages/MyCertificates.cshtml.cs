using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class MyCertificatesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MyCertificatesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Certificate> Certificates { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            Certificates = await _context.Certificates
                .Include(c => c.Assessment)
                .Where(c => c.StudentId == studentId.Value)
                .OrderByDescending(c => c.IssuedDate)
                .ToListAsync();

            return Page();
        }
    }
}