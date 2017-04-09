using FMTest.Business;
using FMTest.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace FMTest.Controllers
{
    public class AppointmentsController : ApiController
    {
        public AppointmentSystem _appointmentSystem = new AppointmentSystem();
        // GET: api/Customer
        public IList<AppointmentDto> Get()
        {
            var result = _appointmentSystem.GetAllAppointments();
            return result;
        }

        public int Post(AppointmentDto appointment)
        {
            return _appointmentSystem.SaveAppointment(appointment);
        }
    }
}
