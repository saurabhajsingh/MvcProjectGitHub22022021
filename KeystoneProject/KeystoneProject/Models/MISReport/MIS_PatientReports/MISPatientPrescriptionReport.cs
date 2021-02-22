using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISPatientPrescriptionReport
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int PatientPrescriptionID { get; set; }
        public int PatientRegNO { get; set; }
        public string PatientName { get; set; }
        public string DoctorPrintName { get; set; }
        public string ChiefComplaint { get; set; }
        public string Days { get; set; }
        public string DiagnosisName { get; set; }
        public string InvestigationName { get; set; }
        public string DrugName { get; set; }

        }
}