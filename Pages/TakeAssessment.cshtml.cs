using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class TakeAssessmentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TakeAssessmentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Assessment? Assessment { get; set; }

        public int Score { get; set; }

        public bool Submitted { get; set; }

        public bool Passed { get; set; }

        public int TotalQuestions => Assessment?.Questions.Count ?? 0;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            Assessment = await _context.Assessments
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.AssessmentId == id);

            if (Assessment == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            Assessment = await _context.Assessments
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.AssessmentId == id);

            if (Assessment == null)
            {
                return NotFound();
            }

            Score = 0;

            foreach (var question in Assessment.Questions)
            {
                string fieldName = $"question_{question.QuestionId}";
                string? selectedAnswer = Request.Form[fieldName];

                if (!string.IsNullOrEmpty(selectedAnswer) &&
                    selectedAnswer == question.CorrectAnswer)
                {
                    Score++;
                }
            }

            int totalQuestions = Assessment.Questions.Count;

            // 50% or above = Pass
            Passed = totalQuestions > 0 &&
                     Score >= Math.Ceiling(totalQuestions / 2.0);

            // Save result
            var result = new Result
            {
                StudentId = studentId.Value,
                AssessmentId = Assessment.AssessmentId,
                Score = Score,
                TotalQuestions = totalQuestions,
                CompletedAt = DateTime.Now
            };

            _context.Results.Add(result);

            // Generate certificate when passed
            if (Passed)
            {
                string certificateNumber =
                    $"CERT-{DateTime.Now:yyyyMMddHHmmss}-{studentId.Value}";

                var certificate = new Certificate
                {
                    CertificateNumber = certificateNumber,
                    StudentId = studentId.Value,
                    AssessmentId = Assessment.AssessmentId,
                    Score = Score,
                    TotalQuestions = totalQuestions,
                    IssuedDate = DateTime.Now
                };

                _context.Certificates.Add(certificate);
            }

            await _context.SaveChangesAsync();

            Submitted = true;

            return Page();
        }
    }
}
