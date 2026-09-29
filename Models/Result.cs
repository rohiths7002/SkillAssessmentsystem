using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillAssessmentSystem.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }

        public int StudentId { get; set; }

        public int AssessmentId { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.Now;

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [ForeignKey("AssessmentId")]
        public Assessment? Assessment { get; set; }
    }
}