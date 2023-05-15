using HMS.Service.Domain.Abstractions.Models;

namespace HMS.Service.Domain.Abstractions.Interfaces
{
    public interface ICostEstimatorManager
    {

        public PatientViewResult GetCostEstimator(PatientDetailsRequest patientDetailsRequest);
    }
}
