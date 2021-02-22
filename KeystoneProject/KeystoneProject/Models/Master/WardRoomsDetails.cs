using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace KeystoneProject.Models.Master
{

    public class WardRoomsDetails
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int WardID { get; set; }
        public string RoomID { get; set; }
        public string BedID { get; set; }
        public string RoomName { get; set; }
        public string WardName { get; set; }
        public string ReferenceCode = "1";//{ get; set; }
        public string GeneralCharges = "0.00";//{ get; set; }
        public string EmergencyCharges = "0.00";//{ get; set; }      
        public string Mode { get; set; }
        public string colorname { get; set; }
        public int ServiceID { get; set; }
        public string ServiceChargesID { get; set; }

        public DataSet GetAllRoomsDetails { get; set; }
        public DataSet GetBedDetails { get; set; }
        
        public string BedNo { get; set; }
        public bool BedStatus { get; set; }
        public string BedStatus1 { get; set; }
        public string[] Bedcount { get; set; }
        public string TotalBed { get; set; }
       

    }
}
