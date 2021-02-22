using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.PatientReport
{
    public class MISGovernmentRecordReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string DoctorID { get; set; }
        public string DoctorPrintName { get; set; }
        public string Problem { get; set; }
        public string ProblemID { get; set; }
        public string PatientName {get;set;}
    public string PatientRegNo {get;set;}
    public string GuardianName {get;set;}
    public string OPDIPDID {get;set;}
    public string Gender {get;set;}
    public string Age {get;set;}
    public string PatientType {get;set;}
    public string MaritalStatus {get;set;}
    public string ReligionName {get;set;}
    public string IndicationOfTermination { get; set; }
    public string Duration { get; set; }
    public string Remark { get; set; }
    public string OpinionBy { get; set; }
    public string PerformedBy { get; set; }
    public string AddmissionDate { get; set; }
    public string DischargeDate { get; set; }
    public string Address { get; set; }
    //public string PerformedBy { get; set; }
    }
}