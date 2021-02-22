using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class MedicalSchedule
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }

        public int UserID { get; set; }

        public string scheduleName { get; set; }

        public string MasterScheduleName { get; set; }
        public string Nature { get; set; }

        public string BPT { get; set; }

        public string generalLedgerPosting { get; set; }

        public string showDetailsInReports { get; set; }

        public int ScheduleID { get; set; }

        public string MasterScheduleID { get; set; }
       
    }
}