using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class Reason 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int ReasonID { get; set; }

        [Required(ErrorMessage = "Reason is Required")]
        public string ReasonName { get; set; }

        public DataSet StoreAllReason { get; set; }
        public string Mode { get; set; }  
    }
}
