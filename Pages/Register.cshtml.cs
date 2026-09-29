using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check password confirmation
            if (Password != ConfirmPassword)
            {
                Message = "Passwords do not match.";
                return Page();
            }

            // Check whether email already exists
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == Email);

            if (existingStudent != null)
            {
                Message = "Email already registered.";
                return Page();
            }

            // Create student
            var student = new Student
            {
                FullName = FullName,
                Email = Email,
                CreatedAt = DateTime.Now
            };

            // Hash password
            var passwordHasher = new PasswordHasher<Student>();
            student.PasswordHash =
                passwordHasher.HashPassword(student, Password);

            // Save to database
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            Message = "Registration successful! You can now login.";

            return Page();
        }
    }
}