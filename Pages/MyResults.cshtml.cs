using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class MyResultsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MyResultsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Result> Results { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Check whether student is logged in
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            // Get results only for the logged-in student
            Results = await _context.Results
                .Include(r => r.Assessment)
                .Where(r => r.StudentId == studentId.Value)
                .OrderByDescending(r => r.CompletedAt)
                .ToListAsync();

            return Page();
        }
    }
}