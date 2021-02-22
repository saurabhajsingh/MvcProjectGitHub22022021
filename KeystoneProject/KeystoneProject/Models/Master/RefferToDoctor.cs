using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class RefferToDoctor
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int RefferDoctorID { get; set; }
        public string InstituteName { get; set; }
        public string PrintName { get; set; }
        public string ReferenceCode { get; set; }
        public string PermanentAddress { get; set; }
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public DataSet StoreAllInstitute { get; set; }
        public string Mode { get; set; }

    }
}
