using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Models
{
    public class TransactionModel
    {
        [Key]
        public int TransactionId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 999999, ErrorMessage = "Amount must be greater than 0.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; } = "General";
    }
}