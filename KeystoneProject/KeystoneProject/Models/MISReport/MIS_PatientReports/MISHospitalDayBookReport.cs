using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class MISHospitalDayBookReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public DateTime FromDate { get; set; }
    }
}