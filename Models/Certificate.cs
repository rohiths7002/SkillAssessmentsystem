using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillAssessmentSystem.Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        public string CertificateNumber { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public int AssessmentId { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [ForeignKey("AssessmentId")]
        public Assessment? Assessment { get; set; }
    }
}