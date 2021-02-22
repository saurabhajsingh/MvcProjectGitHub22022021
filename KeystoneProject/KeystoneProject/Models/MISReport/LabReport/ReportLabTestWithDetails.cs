using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Buisness_Logic.MISReport.LabReport;
using KeystoneProject.Controllers.MISReport.LabReport;

namespace KeystoneProject.Models.MISReport.LabReport
{
    public class ReportLabTestWithDetails
    {

        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string BillNo { get; set; }
        public string LabNo { get; set; }
        public string PatientType { get; set; }
        public string Patient { get; set; }
        public string ContactNo { get; set; }
        public string DoctorName { get; set; }
        public string Regno { get; set; }
        public string OPDIPDID { get; set; }
        public string TestName { get; set; }
    }
}