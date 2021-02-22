using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.LabReport
{
    public class ReportLabTestByCategory
    {

        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string ParentCategoryName { get; set; }
        public string TestName { get; set; }

        public string TestDate { get; set; }
        public string PatientName { get; set; }

        public string PatientType { get; set; }
        public string Gender { get; set; }

        public string Count { get; set; }
    }
}