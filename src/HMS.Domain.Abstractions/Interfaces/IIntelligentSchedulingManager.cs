using HMS.Service.Domain.Abstractions.Entities;
using HMS.Service.Domain.Abstractions.Models;

namespace HMS.Service.Domain.Abstractions.Interfaces
{
    public interface IIntelligentSchedulingManager
    {
        public PatientViewResult RequestAppointment(PatientAppointmentRequest patientAppointmentRequest);

    }
}
