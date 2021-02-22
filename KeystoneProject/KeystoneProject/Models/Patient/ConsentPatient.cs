using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class ConsentPatient
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string FinancialYear { get; set; }
        public int FinancialYearID { get; set; }
        public string PatientRegNo { get; set; }
        public string PrefixName { get; set; }
        public string PatientName { get; set; }
        public string GuardianName { get; set; }
        public string PatientType { get; set; }
        public string AgeType { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string OPDIPDID { get; set; }
        public string ConsultantDrID { get; set; }
        public string ReferredByDoctorID { get; set; }
        public string DoctorPrintName { get; set; }
        public string ReffDoctorName { get; set; }
        public string ConsentID { get; set; }
        public string ConsentName { get; set; }
        public string Path { get; set; }
        public string ConsentDetailID { get; set; }
        public string Footer { get; set; }
        public string PatientRegistrationDate { get; set; }
        public string Description { get; set; }
        public string billDate { get; set; }
        public string TPA_ID { get; set; }
        public string DepartmentID { get; set; }
        public string Weight { get; set; }
        public string BloodPressure { get; set; }
        public string PrintOPDNo { get; set; }
        public string opdIpdNumberID { get; set; }
        public string PatientType1 { get; set; }
        public string ConsentNameform { get; set; }
        public string oldBill { get; set; }
        public string billTime { get; set; }
        public string PrintRegNO { get; set; }
        public string ReferredDrID { get; set; }


    }
}