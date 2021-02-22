using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class MedicalAccount
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string AccountName { get; set; }
        public string PrintName { get; set; }
        public string ScheduleName { get; set; }
        public string AccountType { get; set; }
        public string CreditDays { get; set; }
        public string CrLimit { get; set; }
        public string OPBalance { get; set; }
        public string OBType { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string PinCode { get; set; }
        public string country { get; set; }
        public string EmailID { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FAX { get; set; }
        public string Remark { get; set; }
        public string TinNo { get; set; }
        public string TinDate { get; set; }
        public string Pan { get; set; }
        public string CSTNO { get; set; }

        public int DrAmount { get; set; }
        public int CrAmount { get; set; }
        public string Locality { get; set; }

        public int AccountID { get; set; }
        public string ScheduleID { get; set; }
        public string CityID { get; set; }
        public string StateID { get; set; }
        public string CountryID { get; set; }
        

    }
}