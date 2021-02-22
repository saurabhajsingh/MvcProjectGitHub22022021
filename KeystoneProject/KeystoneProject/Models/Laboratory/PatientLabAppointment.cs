using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Laboratory
{
    public class PatientLabAppointment 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string PrefixID { get; set; }
        public string PrefixName { get; set; }
        public string OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string CollectionCentreID { get; set; }
        public string CollectionCentreName { get; set; }
        public string PatientLabAppointmentID { get; set; }
        public int AppointmentID { get; set; }
        public string AgeType { get;set;}
        public string PatientLabAppointmentDetailID { get; set; }
        public string PatientName { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string GuardianName { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentDate { get; set; }
        public string ConsultantDrID { get; set; }
        public string ConsultantDr { get; set; }
        public string RefferedDrID { get; set; }
        public string RefferedDr { get; set; }
        public string SampleCollectionBy { get; set; }
        public string TestID { get; set; }
        public string TestName { get; set; }
        public string TotalAmount { get; set; }
        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }
        public int ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string Rate { get; set; }
        public string Quantity { get; set; }
        public string TestType { get; set; }
        public string CreationID { get; set; }
        public string Mode { get; set; }
    }
}
