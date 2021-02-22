using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientPrescriptionMedicine
    {
        public string PatientPrescriptionNewID { get; set; }

        public string medicine { get; set; }

        public string Strength { get; set; }

        public string Frequency { get; set; }

        public string Instruction { get; set; }

        public string Days { get; set; }

    }
}