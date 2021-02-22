using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class OutSourceLab
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string OutSourceLabID { get; set; }
        public string OutSourceLabtestdetailsID { get; set; }
        public int TestID { get; set; }
        public string TestName { get; set; }
        public string Test { get; set; }
        public string GroupID { get; set; }
        public int LabID { get; set; }
        public string Rate { get; set; }
        public string Mode { get; set; }
        public int CreationID { get; set; }
        public string ReferenceCode { get; set; }
        public string LabName { get; set; }
        public int TestGroupID { get; set; }
        public string TestGroupName { get; set; }
        public string ManagingBody { get; set; }
        public string Percentage { get; set; }
        public string Adminstrator { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string EmailID { get; set; }
        public string URL { get; set; }
        public string FaxNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string PinCode { get; set; }
    }
}