using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class PatientIPDWardTransfer
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public string ExitTime { get; set; }
        public string time { get; set; }

        public string Message { get; set; }
        public int HospitalID { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string OrganizationID { get; set; }
        public string GuardianName { get; set; }
        public int LocationID { get; set; }
        public int PatientRegNO { get; set; }
        public string PatientName { get; set; }
        public int PatientIPDNO { get; set; }
        public string WardID { get; set; }
        public string WardName { get; set; }
        public string WardNameFill { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string RoomNameFill { get; set; }
        public string BedID { get; set; }
        public string BedNo { get; set; }

        public string BedNoFill { get; set; }
        public string BedName { get; set; }
        public string OldWardID { get; set; }
        public string OldRoomID { get; set; }
        public string OldBedID { get; set; }
        public string BedCharges { get; set; }
        public string IsCurrentBed { get; set; }
        public string EnterDateTime { get; set; }
        public string EnterDateTimeFill { get; set; }
        public int CreationID { get; set; }

        public string FromWordID { get; set; }
        public string FromRoomID { get; set; }
        public string FromBedID { get; set; }
        public string FromDate { get; set; }
        public string FromTime { get; set; }


        public string ToWord { get; set; }
        public string ToRoom
        {
            get;
            set;
        }
        public string ToBedNo
        {
            get;
            set;
        }
        public string TransferDate
        {
            get;
            set;
        }
        public string TransferTime
        {
            get;
            set;
        }

        public string FinancialYearID
        {
            get;
            set;
        }

        public string FinancialYear
        {
            get;
            set;
        }

    }
}
