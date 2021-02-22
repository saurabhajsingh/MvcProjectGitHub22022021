using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientPrivilegeCard
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string PrefixName { get; set; }
        public string PrefixID { get; set; }
        public int AccountsID { get; set; }
        public int VoucharID { get; set; }
        public int VoucharEntryID {get;set;}
        public int VoucharEntryDetailID{get;set;}
        public int ScheduleID { get; set; }
        public string PFPatientName { get; set; }
        public int PatientRegNo { get; set; }
        public int CardName1 { get; set; }
        public int PriceName1 { get; set; }
        public int PrivilegeCardID { get; set; }
        public int PatientPrivilegeCardID { get; set; }
        public string PrivilegePriceDetailID { get; set; }
        public string FinancialYear { get; set; }
        public string PatientName { get; set; }
        public string PFirstName { get; set; }
        public string PMiddleName { get; set; }
        public string PLastName { get; set; }
         public string Age { get; set; }
         public int AgeYear { get; set; }
         public int AgeMonth { get; set; }
         public int AgeDay { get; set; }
        public  string DateOfBirth {get ; set;}
        public string BP { get; set; }
        public string PFGuardianName { get; set; }

        public string GuardianName { get; set; }
        public string Gender { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string AmountName { get; set; }
        public string chk { get; set; }
        public string PrintRegNO { get; set; }


        public string CardName { get; set; }
        public int CardID { get; set; }
        public string PriceName { get; set; }
        public decimal PriceAmt { get; set; }
        public decimal PaidAmt { get; set; }
        public Nullable<DateTime> ValidDate { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> PrivilegeDate { get; set; }
        public int FinancialYearID { get; set; }
        public string mode { get; set; }
        public int CreationID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }



    }
}
