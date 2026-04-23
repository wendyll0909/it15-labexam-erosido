using Microsoft.EntityFrameworkCore;
using IT15LabExamErosido.Models;

namespace IT15LabExamErosido.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Employee-Payroll relationship
            modelBuilder.Entity<Payroll>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Payrolls)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Fix decimal precision for DailyRate (MySQL compatibility)
            modelBuilder.Entity<Employee>()
                .Property(e => e.DailyRate)
                .HasColumnType("decimal(18,2)");
            
            // Fix decimal precision for Deduction (MySQL compatibility)
            modelBuilder.Entity<Payroll>()
                .Property(p => p.Deduction)
                .HasColumnType("decimal(18,2)");
            
            // Configure Employee ID auto-generation
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();
            
            // Configure Payroll ID auto-generation
            modelBuilder.Entity<Payroll>()
                .Property(p => p.PayrollId)
                .ValueGeneratedOnAdd();
        }
    }
}