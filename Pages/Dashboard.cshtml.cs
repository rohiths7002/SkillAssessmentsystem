using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string StudentName { get; set; } = "Student";

        public int TotalAssessments { get; set; }

        public int TotalResults { get; set; }

        public int TotalCertificates { get; set; }

        public List<Result> RecentResults { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");

            var studentName = HttpContext.Session.GetString("StudentName");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            if (!string.IsNullOrEmpty(studentName))
            {
                StudentName = studentName;
            }

            // Total assessments available
            TotalAssessments = await _context.Assessments.CountAsync();

            // Total results completed by current student
            TotalResults = await _context.Results
                .CountAsync(r => r.StudentId == studentId.Value);

            // Total certificates earned by current student
            TotalCertificates = await _context.Certificates
                .CountAsync(c => c.StudentId == studentId.Value);

            // Recent assessment results
            RecentResults = await _context.Results
                .Include(r => r.Assessment)
                .Where(r => r.StudentId == studentId.Value)
                .OrderByDescending(r => r.CompletedAt)
                .Take(5)
                .ToListAsync();

            return Page();
        }
    }
}