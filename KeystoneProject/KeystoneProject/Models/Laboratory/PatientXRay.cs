using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Laboratory
{
    public class PatientXRay
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string PatientType { get; set; }
        public string LabNo { get; set; }
        public string LabDate { get; set; }
        public string TestID { get; set; }
        public string Testname { get; set; }
        public string Footer { get; set; }
        public int PatientLabParameterID { get; set; }
        public string Description { get; set; }
    }
}