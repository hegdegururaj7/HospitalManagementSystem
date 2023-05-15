namespace HMS.Service.Domain.Abstractions.Models
{
    public class PatientDetailsRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int DaysInHospital { get; set; }
        public bool HasInsurance { get; set; }
        public bool IsCritical { get; set; }


    }
}
