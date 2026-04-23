using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT15LabExamErosido.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        public string Position { get; set; } = string.Empty;
        
        [Required]
        public string Department { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Daily Rate")]
        [Range(0, double.MaxValue, ErrorMessage = "Daily rate must be positive")]
        public decimal DailyRate { get; set; }
        
        // Navigation property for payroll transactions
        public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
    }
}