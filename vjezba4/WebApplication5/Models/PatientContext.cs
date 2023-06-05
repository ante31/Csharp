using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication5.Models
{
    public class PatientContext : DbContext
    {
        public PatientContext(DbContextOptions<PatientContext> options) : base(options) 
        { 
        
        }
        public DbSet<Patient> Patients { get; set; }
    }
}
