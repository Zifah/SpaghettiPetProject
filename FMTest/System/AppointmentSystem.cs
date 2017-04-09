using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class AppointmentSystem
    {
        private DataSystem _dataSystem = new DataSystem();

        public List<AppointmentDto> GetAllAppointments()
        {
            var patients = _dataSystem.RunSelect<AppointmentDto>("select A.DoctorName, A.Comment, A.CreationTime, P.Name PatientName from Appointments A join Patients P on P.Id = A.Patient");
            return patients.ToList();
        }

        public int SaveAppointment(AppointmentDto appointment)
        {
            return _dataSystem.SaveAppointment(appointment);
        }
    }
}