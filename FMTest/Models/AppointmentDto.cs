using FMTest.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Models
{
    public class AppointmentDto
    {
        [RdbmsName("PatientName")]
        public string Patient{ set; get; }
        public int PatientId { set; get; }

        [RdbmsName("DoctorName")]
        public string Doctor { set; get; }

        [RdbmsName("Comment")]
        public string Comment { set; get; }

        [RdbmsName("CreationTime")]
        public virtual DateTime CreationTime { set; get; }

        public virtual string Created
        {
            get
            {
                return CreationTime.ToString("dd MMM yyyy");
            }
        }
    }
}