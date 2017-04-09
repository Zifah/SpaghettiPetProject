using FMTest.Business;
using FMTest.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace FMTest.Controllers
{
    public class PatientsController : ApiController
    {
        public PatientSystem _patientSystem = new PatientSystem();
        // GET: api/Customer
        public IList<Patient> Get()
        {
            var result = _patientSystem.GetAllPatients();
            return result;
        }

        public int Post(Patient patient)
        {
            return _patientSystem.RegisterPatient(patient);
        }
    }
}
