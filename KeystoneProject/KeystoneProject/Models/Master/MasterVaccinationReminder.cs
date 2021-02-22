using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.Master
{
    public class MasterVaccinationReminder 
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationId { get; set; }
        public int HospitalId { get; set; }

        public int MasterVaccinationReminderID { get; set; }

        public string MasterVaccinationReminderName { get; set; }

        public string Advice { get; set; }

        public string ReferenceCode { get; set; }
        public int MasterVaccinationReminderDetailID { get; set; }
        public string VaccinesName { get; set; }

        public Nullable<DateTime> DueDate { get; set; }

        public Nullable<DateTime> GivenDate { get; set; }

        public Nullable<DateTime> HealthCheckupDate { get; set; }
        public int MasterVaccinationReminderDetaiWithTestlID { get; set; }

        public string TestName { get; set; }

        public int TestID { get; set; }



    }
}
