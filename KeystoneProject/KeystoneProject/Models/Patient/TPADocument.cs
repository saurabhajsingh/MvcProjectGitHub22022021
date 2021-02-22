using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Patient
{
    public class TPADocument
    {
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public int UserId { get; set; }
        public int registrationNumber { get; set; }
        public string patientName { get; set; }
        public int oldDocument { get; set; }        
        public int DocumentID { get; set; }
        public string narration { get; set; }
    }
}