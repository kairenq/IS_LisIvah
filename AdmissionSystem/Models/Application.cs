using System;

namespace AdmissionSystem.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SpecialtyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double ExamScore { get; set; }
        public string Status { get; set; } // "На рассмотрении", "Одобрено", "Отклонено"
        public DateTime SubmissionDate { get; set; }
        public string Notes { get; set; }

        public Application()
        {
            SubmissionDate = DateTime.Now;
            Status = "На рассмотрении";
        }
    }
}
