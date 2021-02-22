using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.MasterLaboratory
{
    public class PatientCWProfile

    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }

        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string ProfileID { get; set; }
        public string PatientCWProfileID { get; set; }
        public string ProfileName { get; set; }
        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }
        public string Mode { get; set; }
        
    }
}