using HMS.Service.Domain.Abstractions.Interfaces;
using HMS.Service.Domain.Abstractions.Models;
using System.Net;

namespace HMS.Service.Domain.Infrastructure
{
    public class IntelligentSchedulingManager : IIntelligentSchedulingManager
    {
        public readonly IHMSRepository _hMSRepository;
        public IntelligentSchedulingManager(IHMSRepository hMSRepository)
        {
            _hMSRepository = hMSRepository;
        }

        public PatientViewResult RequestAppointment(PatientAppointmentRequest patientAppointmentRequest)
        {
            var patientData = _hMSRepository.GetByPatientName(patientAppointmentRequest.Name);
            if (patientData == null || patientData.BedId != Guid.Empty || patientData.DoctorId != Guid.Empty)
            {
                return new PatientViewResult()
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    Message = patientData == null ? $"Patient Data is not found" : (patientData.BedId != Guid.Empty  && patientData.DoctorId != Guid.Empty)
                    ? $"Patient is already assigned with a bed and doctor" : $"Patient is already assigned with a faculty"
                };
            }
            var doctorList = _hMSRepository.GetAvailableDoctors();

            if (!doctorList.Any(x => x.IsAvailable))
            {
                return new PatientViewResult()
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    Message = $"No available doctors"
                };

            }

            var doctor = doctorList.Where(x => x.Qualification.ToLower().Contains(patientAppointmentRequest.MedicalCondition.ToLower()) && x.IsAvailable).FirstOrDefault();
            if (doctor is null)
            {
                return new PatientViewResult()
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    Message = $"No available doctors matching the qualification"
                };

            }

            var bedList = _hMSRepository.GetAvailableBeds();
            if (!bedList.Any(x => x.IsOccupied))
            {
                return new PatientViewResult()
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    Message = $"No available beds"
                };

            }
            patientData.DoctorId = doctor.Id;
            patientData.BedId = bedList.Where(x => x.IsOccupied).First().Id;
            patientData.UpdatedBy = "TestUser";
            patientData.UpdatedAt = DateTime.UtcNow;

            var response = _hMSRepository.UpdatePatientDetails(patientData);
            if (response)
            {
                return new PatientViewResult()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = $"Scheduling patient name : {patientData.PatientName} with assigned doctor: {doctor.DoctorName} and assigned bed number: {bedList.Where(x => x.IsOccupied).First().BedNumber}"
                };
            }
            return new PatientViewResult()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = $"Error occured while updating the data"
            };
        }
    }
}