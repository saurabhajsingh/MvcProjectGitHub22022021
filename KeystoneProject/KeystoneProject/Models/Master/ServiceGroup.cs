using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class ServiceGroup 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int ServiceGroupID { get; set; }

        public string ServiceGroupName { get; set; }

        public string ServiceType { get; set; }

        public int ServicesOrder { get; set; }

        public string SubDirectry { get; set; }
        public string HideInBilling { get; set; }
        public string HSNCode { get; set; }
        public DataSet StoreAllServiceGroup { get; set; }
        //public int intCreationID { get; set; }
        //public string strMode { get; set; }
    }
}
