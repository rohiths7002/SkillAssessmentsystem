using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentSystem.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int AssessmentId { get; set; }

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string OptionA { get; set; } = string.Empty;

        [Required]
        public string OptionB { get; set; } = string.Empty;

        [Required]
        public string OptionC { get; set; } = string.Empty;

        [Required]
        public string OptionD { get; set; } = string.Empty;

        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;

        public Assessment? Assessment { get; set; }
    }
}