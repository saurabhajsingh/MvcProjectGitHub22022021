using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class MISHospitalExpencesReport
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string ServiceGroupID { get; set; }
        public string ServiceGroupName { get; set; }
        public DataSet dsServicesCharges { get; set; }

    }
}