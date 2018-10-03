using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class PatientSystem
    {
        private DataSystem _dataSystem = new DataSystem();

        public List<Patient> GetAllPatients()
        {
            var patients = _dataSystem.GetAll<Patient>("Patients").ToList();
            return patients;
        }

        public int RegisterPatient(Patient newPatient)
        {
            return _dataSystem.SavePatient(newPatient);
        }
    }
}