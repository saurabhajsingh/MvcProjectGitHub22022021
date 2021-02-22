using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Keystone
{
    public class HospitalCreation 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string GroupName { get; set; }
        public string ReferenceCode { get; set; }
        public HttpPostedFileWrapper Logo { get; set; } 
        //public string Logo   { get; set; }
        public string ManagingBody { get; set; }
        public string Adminstrator { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public string EmailID { get; set; }
        public string StateID { get; set; }
        public string StateName { get; set; }
        public string CountryID { get; set; }
        public string CountryName { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string PhoneNo1 { get; set; }
        public string MobileNo { get; set; }
        public string URL { get; set; }
        public string Mode { get; set; }
        public DataSet StoreAllHospital { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }      
        public Byte[] Report1 { get; set; }
        public Byte[] Report2 { get; set; }
        public Byte[] ReportHeader { get; set; }

        public Byte[] BillHeader { get; set; }
        public Byte[] LabsReportHeader { get; set; }
        public Byte[] HospitalReportHeader { get; set; }
    }
}
