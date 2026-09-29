using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class AssessmentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AssessmentsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Assessment> Assessments { get; set; } = new();

        public async Task OnGetAsync()
        {
            Assessments = await _context.Assessments
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}