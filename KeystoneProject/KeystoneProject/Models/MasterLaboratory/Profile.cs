using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class Profile
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }
        public string ProfileID { get; set; }
        public string ProfileTestID { get; set; }
        public string PatientCWProfileID { get; set; }
        public string TestID { get; set; }
        public string TestName { get; set; }
        public string Name { get; set; }
        public string PrintAs { get; set; }
        public string Client { get; set; }
        public string GeneralCharges { get; set; }
        public string EmergencyCharges { get; set; }
        public string Commission { get; set; }
        public string HMSCode { get; set; }
        public string CommissionRs { get; set; }
        public string MyCost { get; set; }
        public string Discount { get; set; }
        public string ForGender { get; set; }
        public string Panel { get; set; }
        public int CreationID { get; set; }
        public string Mode { get; set; }
        public object ReferenceCode { get; set; }
        public DataSet dsgrid { get; set; }


        
    }
}
