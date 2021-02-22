using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Patient
{
    public class BirthCertificate
    {
        public string printReg { get; set; }
        public int CertificateNo { get; set; }
        public string ReferenceCode { get; set; }
        public int PatientRegNo { get; set; }
        public string PatientName { get; set; }
        public string Cast { get; set; }
        public string Date { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string MAge { get; set; }
        public string MNationality { get; set; }
        public string MReligion { get; set; }
        public string MOccuption { get; set; }
        public string MQualification { get; set; }
        public string FAge { get; set; }
        public string FNationality { get; set; }
        public string FReligion { get; set; }
        public string FOccuption { get; set; }
        public string FQualification { get; set; }
        public string DOB1 { get; set; }
        public string DOB2 { get; set; }
        public string DOB3 { get; set; }
        public string DOB4 { get; set; }
        public string TOB1 { get; set; }
        public string TOB2 { get; set; }
        public string TOB3 { get; set; }
        public string TOB4 { get; set; }
        public string Weight1 { get; set; }
        public string Weight2 { get; set; }
        public string Weight3 { get; set; }
        public string Weight4 { get; set; }
        public string Sex1 { get; set; }
        public string Sex2 { get; set; }
        public string Sex3 { get; set; }
        public string Sex4 { get; set; }
        public string BirthPlace { get; set; }
        public string DeliveryType { get; set; }
        public string HODone { get; set; }
        public string Remarks { get; set; }
        public string NoOfChild { get; set; }
        public string Mode { get; set; }
    }
}