using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MISReport.MIS_PatientReports
{
    public class MISWardWiseDetails
    {
        public string HospitalID { get; set; }
        public string LocationID { get; set; }

        public string RegNo { get; set; }
        public string IPDNo { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string AddmissionDate { get; set; }
        public string DischargeDate { get; set; }
        public string Days { get; set; }
        public string WardName { get; set; }
        public string ConsultantDr { get; set; }
        public string RefferedDr { get; set; }
        public string PatientStatus { get; set; }
    }
}