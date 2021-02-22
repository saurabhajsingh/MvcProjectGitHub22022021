using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISDoctorWiseCollectionReport 
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string TotalAmount1 { get; set; }
        public DataSet dsDoctor { get; set; }
        public DataSet dsDoctorWise { get; set; }
        public DataSet dsDoctorWiseDetail { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DoctorType { get; set; }
        public string PatientType { get; set; }
        public string Username { get; set; }
        public string ConsultantDoctor { get; set; }
        public string ReferredDoctor { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string BillNo { get; set; }
        public string TotalAmount { get; set; }
    
    }
}
