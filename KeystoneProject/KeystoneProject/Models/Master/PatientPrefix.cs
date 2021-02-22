using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class PatientPrefix
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int PrefixID { get; set; }

        [Required(ErrorMessage = "Prefix is Required")]
        public string PrefixName { get; set; }
        public string Gender { get; set; }
        public DataSet StoreAllPrefix { get; set; }
        public string Mode { get; set; }  
    }
}