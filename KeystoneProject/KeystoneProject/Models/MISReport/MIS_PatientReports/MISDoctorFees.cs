using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class MISDoctorFees
    {
        public string opdtype { get; set; }
       
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string TotalAmount1 { get; set; }
        
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
        
        public string PatientRegNO {get;set;}
         public string PatientName  {get;set;}
        public string NetAmount{get;set;}
       public string Fees { get; set; }
       public string OPDCommission { get; set; }
       public string IPDCommission { get; set; }
       public string LABCommission { get; set; }
       public string TotalCommission { get; set; }
   }
}
