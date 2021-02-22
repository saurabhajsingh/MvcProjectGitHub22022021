using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class ServicesTPAWise
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public int ServicesTPAWiseID { get; set; }
        public DataSet ds { get; set; }
        public string OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string ServiceID { get; set; }

        public string tpa_id { get; set; }
        public string ServiceIDHidden { get; set; }
        public string unithidden { get; set; }
        public string ServiceName { get; set; }

        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public string Mode { get; set; }

        public Decimal GeneralCharges { get; set; }
        public Decimal EmergencyCharges { get; set; }

        public string ServiceType { get; set; }

        public string RecommendedByDoctor { get; set; }
        public string HideInBilling { get; set; }
        
        ///////////////////////////////////////////////////////////////////////////////////////////////
        
        public int ServicesDetailsTPAWiseID { get; set; }
        
        public int WardID { get; set; }
        public string WardName { get; set; }

        public DataSet StoreAllTPAWiseService { get; set; }


        public DataSet dsServiceTPAWiseCharges { get; set; }




    }


   public class ArrayWordDetailTpa
   {
       public Decimal[] GeneralCharges1 { get; set; }
       public Decimal[] EmergencyCharges1 { get; set; }

       public int[] ServicesDetailsTPAWiseID { get; set; }
       public int[] WardID1 { get; set; }
       public string[] WardName { get; set; }
   }
}
