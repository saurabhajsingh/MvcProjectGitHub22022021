using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Laboratory
{
    public class PatientLabReportPrint
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; } 
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string TestID { get; set; } 
        public string TestName { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; } 
        public string LabType { get; set; }
        public string LabDate { get; set; }
        public string LabNo { get; set; } 
        public string TestStatus { get; set; } 
        public string TestType { get; set; }
        public string EmailID { get; set; }
        public string EmailPassword { get; set; }

        public string chbchkGrp { get; set; }

        public CrystalDecisions.Shared.ExportOptions ExportOptions { get; set; }

        internal void Export()
        {
            throw new NotImplementedException();
        }
    }
}