using HMS.Service.Domain.Abstractions.Interfaces;
using HMS.Service.Domain.Abstractions.Models;
using System.Net;

namespace HMS.Service.Domain.Infrastructure
{
    public class CostEstimatorManager : ICostEstimatorManager
    {
        public PatientViewResult GetCostEstimator(PatientDetailsRequest patientDetailsRequest)
        {
            if (patientDetailsRequest.DaysInHospital <= 0 || patientDetailsRequest.Age <= 0)
            {
                return new PatientViewResult()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = $"Age Or Days in Hospital should be a positive number"
                };
            }
            var basicCostPerDay = patientDetailsRequest.DaysInHospital * 500;
            decimal ageFactor;
          
            if (patientDetailsRequest.Age < 18)
                ageFactor = 0.8M;
            else
                ageFactor = 1M;
            var insuranceFactor = patientDetailsRequest.HasInsurance ? 0.8M : 1M;
            var criticalFactor = patientDetailsRequest.IsCritical ? 2 : 1;
            var totalCost = basicCostPerDay * ageFactor * insuranceFactor * criticalFactor;
            return new PatientViewResult()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = $"Total Cost Predicted For Diagnosis of Patient: {patientDetailsRequest.FirstName} {patientDetailsRequest.LastName} is Rs.{totalCost}"
            };
        }
    }
}
