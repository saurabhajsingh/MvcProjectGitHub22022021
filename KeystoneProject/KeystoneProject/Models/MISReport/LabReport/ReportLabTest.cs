using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeystoneProject.Controllers.MISReport.LabReport;

namespace KeystoneProject.Models.MISReport.LabReport
{
    public class ReportLabTest
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string Patient { get; set; }
        public string PatientType { get; set; }
        public string BillNo { get; set; }
        public string LabNo { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string ContactNo { get; set; }
        public string Address { get; set; }

        public string DoctorName { get; set; }
    }
}