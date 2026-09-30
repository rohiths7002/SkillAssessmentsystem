using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages.Assessments;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
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