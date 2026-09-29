using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentSystem.Models
{
    public class Assessment
    {
        public int AssessmentId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationMinutes { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Question> Questions { get; set; }
            = new List<Question>();
    }
}