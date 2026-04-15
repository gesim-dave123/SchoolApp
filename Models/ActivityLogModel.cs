using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class ActivityLogModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Details { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PerformedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
