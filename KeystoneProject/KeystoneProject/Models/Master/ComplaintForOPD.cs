using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
namespace KeystoneProject.Models.Master
{
    public class ComplaintForOPD 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int ComplaintID { get; set; }

        [Required(ErrorMessage = "Complaint is Required")]
        public string ComplaintName { get; set; }

        public DataSet StoreAllComplaint { get; set; }
        public string Mode { get; set; }  
    }
}
