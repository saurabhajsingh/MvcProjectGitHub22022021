using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISServiceWiseCollectionReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string Qty { get; set; }
        public string TotalAmount { get; set; }
        public DateTime FromDate {get;set;}       
        public DateTime ToDate {get;set;}
        public string ServiceGroupName { get; set; }
        public string ServiceName { get; set; }
        public string PatientType { get; set; }
        public DataSet ds { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string TotalAmt1 { get;set;}
       
    }
}
