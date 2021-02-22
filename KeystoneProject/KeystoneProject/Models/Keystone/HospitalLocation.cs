using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Keystone
{
    
    public class HospitalLocation
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }
        public DataSet StoreAllDoctor { get; set; }
        public string LocationName { get; set; }
        public string file1 { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
        public HttpPostedFileWrapper ImageFile1 { get; set; }
        public HttpPostedFileWrapper ImageFile2 { get; set; }
        public HttpPostedFileWrapper ImageFile3 { get; set; }
        public HttpPostedFileWrapper ImageFile4 { get; set; }
        public HttpPostedFileWrapper ImageFile5 { get; set; }
        public HttpPostedFileWrapper ImageFile6 { get; set; }
        public HttpPostedFileWrapper ImageFile7 { get; set; }
        public HttpPostedFileWrapper ImageFile8 { get; set; }
        public HttpPostedFileWrapper ImageFile9 { get; set; }
        public HttpPostedFileWrapper ImageFile10 { get; set; }
        public HttpPostedFileWrapper ImageFile11 { get; set; }
        public string HospitalName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CityID { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string StateID { get; set; }
        public string Country { get; set; }
        public string CountryID { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber1 { get; set; }
        public string Fax { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string URL { get; set; }

        public string RegistrationNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public string PANNo { get; set; }
        public string TANo { get; set; }
        public string TDSCircle { get; set; }
        public string TDSProfessional { get; set; }
        public string TDSContrator { get; set; }
        public string RegistrationCharge { get; set; }
        public string RegistrationRenwalCharges { get; set; }
        public string ServiceCharge { get; set; }

        public Byte[] Report1 { get; set; }
        public Byte[] Report2 { get; set; }
        public string ReportHeader { get; set; }
       
        public string BillHeader { get; set; }
        public string BillHeader1 { get; set; }
        public string Logo { get; set; }

        public string BillsHeaderImg { get; set; }
        public string LabReportsHeaderImg {get;set;}
         public string ReportsHeaderImg {get;set;}
         public string ReportsSignatureImg {get;set;}
        public string  ReportSignature1Img{get;set;}
        public string  CardBackendImg{get;set;}
        public string CardFrontendImg { get; set; }
        public string  PriCardBackendImg{get;set;}
         public string PriCardFrontendImg {get;set;}
         public string OtherHeaderImg { get; set; }
        public string PharmacyImg { get; set; }
        public Byte[] LabsReportHeader { get; set; }
        public Byte[] HospitalReportHeader { get; set; }
        
       
        public string OtherHeader { get; set; }
        public string GroupName { get; set; }
        public string ManagingBody { get; set; }
        public string Adminstrator { get; set; }
        public string EmailPassword { get; set; }
        public string ReferenceCode { get; set; }
        public string ReportSignature { get; set; }
        public string ReportSignature1 { get; set; }
        public string ReportSignature12 { get; set; }
        public string OtherSignature { get; set; }
        public string LabReportHeader { get; set; }
        public string AdmissionCardBack { get; set; }
        public string AdmissionCardFront { get; set; }
        public string PrivilegeCardBack { get; set; }
        public string PrivilegeCardFront { get; set; }
        public string Pharmacy { get; set; }
        public string Logo1 { get; set; }
      //  public string OtherSignature1 { get; set; }
       // public string ReportSignatur { get; set; }
      //  public string LabReportHeader1 { get; set; }
       // public string ReportHeader1 { get; set; }
      //  public string AdmissionCardFront1 { get; set; }
      //  public string AdmissionCardBack1 { get; set; }
    //    public string PrivilegeCardFront1 { get; set; }
      //  public string PrivilegeCardBack1 { get; set; }

      
        
    }
}