using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Oib { get; set; }
        public string Mbo { get; set; }
        public string FirstAndLastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Diagnosis { get; set; }

    }
}
