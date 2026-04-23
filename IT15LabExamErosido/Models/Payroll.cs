using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT15LabExamErosido.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }
        
        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required]
        [Display(Name = "Days Worked")]
        [Range(0, 31, ErrorMessage = "Days worked must be between 0 and 31")]
        public int DaysWorked { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Deduction must be positive")]
        public decimal Deduction { get; set; }
        
        // Computed fields - not stored in database
        [NotMapped]
        public decimal GrossPay => DaysWorked * (Employee?.DailyRate ?? 0);
        
        [NotMapped]
        public decimal NetPay => GrossPay - Deduction;
        
        // Navigation property
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}