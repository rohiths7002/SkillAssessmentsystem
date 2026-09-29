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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Check whether student is logged in
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            // Load assessment with questions
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
            // Get logged-in student's ID
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToPage("/Login");
            }

            // Load assessment with questions
            Assessment = await _context.Assessments
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.AssessmentId == id);

            if (Assessment == null)
            {
                return NotFound();
            }

            // Calculate score
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

            // Check pass/fail status
            Passed = Score >= (Assessment.Questions.Count / 2.0);

            // Save result
            var result = new Result
            {
                StudentId = studentId.Value,
                AssessmentId = Assessment.AssessmentId,
                Score = Score,
                TotalQuestions = Assessment.Questions.Count,
                CompletedAt = DateTime.Now
            };

            _context.Results.Add(result);

            // Create certificate only when passed
            if (Passed)
            {
                var certificate = new Certificate
                {
                    CertificateNumber =
                        "CERT-" + DateTime.Now.ToString("yyyyMMddHHmmss"),

                    StudentId = studentId.Value,

                    AssessmentId = Assessment.AssessmentId,

                    Score = Score,

                    TotalQuestions = Assessment.Questions.Count,

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