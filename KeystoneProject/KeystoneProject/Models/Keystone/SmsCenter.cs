using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Keystone
{
    public class SmsCenter
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public string Select { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Designation { get; set; }
        public string PatientType { get; set; }
        public string message { get; set; }
        public string custonnumber { get; set; }
        public string smstype { get; set; }
        public string checkall { get; set; }



    }
}