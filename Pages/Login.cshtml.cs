using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == Email);

            if (student == null)
            {
                Message = "Invalid email or password.";
                return Page();
            }

            var passwordHasher = new PasswordHasher<Student>();

            var result = passwordHasher.VerifyHashedPassword(
                student,
                student.PasswordHash,
                Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                Message = "Invalid email or password.";
                return Page();
            }

            // Store logged-in student's information in Session
            HttpContext.Session.SetInt32(
                "StudentId",
                student.StudentId
            );

            HttpContext.Session.SetString(
                "StudentName",
                student.FullName
            );

            return RedirectToPage("/Dashboard");
        }
    }
}