using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Patient
{
    public class PatientSearch
    {

        public int PatientIPDNO { get; set; }
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int PatientRegNO { get; set; }
        public string OPDIPDID { get; set; }
        public string PrintOPDIPDNO { get; set; }
        public int PatientOPDNO { get; set; }
        public int PrintOPDNO { get; set; }
        public string PatientType { get; set; }
        //public DateTime AddmissionDate { get; set; }
        public string RegDate { get; set; }
        public int FinancialYearID { get; set; }
        public string financialYear { get; set; }
        public string PatientName { get; set; }
        public string PLastName { get; set; }
        public string PMiddleName { get; set; }
        public string PFirstName { get; set; }
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string GuardianName { get; set; }
        public string Weight { get; set; }
        public string PhoneNo { get; set; }
        public int MobileNo { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string DoctorID { get; set; }
        public string DoctorPrintName { get; set; }
        public string ConsDoctor { get; set; }
        public string RefferedDoctorPrintName { get; set; }
        public string ReffDoctor { get; set; }
        public string TPAName { get; set; }
        public int WardID { get; set; }
        public string WardName { get; set; }
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public int BedID { get; set; }
        public string BedNO { get; set; }
        public string PatientStatus { get; set; }
        public string Dischargedate { get; set; }
        public DataSet StoreAllData { get; set; }
      
    }
}