using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Report
{
    public class MISLabWiseCollectionReport
    {

        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string SrNo { get; set; }
        public string ServiceGroupName { get; set; }

        public int TestID { get; set; }
        public string TestName { get; set; }
        public Decimal TotalAmount { get; set; }
        public int PatientRegNo { get; set; }
        public string PatientName { get; set; }
        
       


    }
}