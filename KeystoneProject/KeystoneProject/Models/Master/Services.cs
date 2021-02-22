using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class Services
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>@
        /// 
        /// 
        public string CheckOutTime { get; set; }

        public string CheckOneDayTime { get; set; }
        public string CheckedOneDay { get; set; }
        public string CheckedOutTime { get; set; }
        public bool BillChargesOne { get; set; }
        public bool BillCharges { get; set; }
        public int ServiceID { get; set; }
        public int WardID { get; set; }
        public string ServiceGroupID { get; set; }
        public string ServiceGroupName { get; set; }
        public string HSNCode { get; set; }
        public string HospitalHSNCode { get; set; }
        public string TPAHSNCode { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationID { get; set; }
        public decimal CutForTPA { get; set; }


        public string WordName { get; set; }
        public string ServiceName { get; set; }

        public string UnitID { get; set; }

        public decimal GeneralCharges { get; set; }

        public decimal EmergencyCharges { get; set; }

        public string ServiceType { get; set; }

        public decimal ReffCommission { get; set; }

        public string ReffCommissionRs { get; set; }

        public decimal Commission { get; set; }

        public string CommissionRs { get; set; }

        public string RecommendedByDoctor { get; set; }

        public string HideInBilling { get; set; }

        public string Mode { get; set; }

        public DataSet StoreAllServices { get; set; }

        public DataSet dsServicesCharges { get; set; }

        public DataSet dsServicesTPAWise { get; set; }

        //   public Services_TPAWise[] OrganizationID {get;  set; }

        public DataSet dsWardDetailsTPAWise { get; set; }

        public string UnitName { get; set; }
    }

    public class ServicesCharges
    {
        //public int WardID { get; set; }
        public int ServiceChargesID { get; set; }
        public string[] WardID { get; set; }
        public string[] GeneralCharges1 { get; set; }
        public string[] EmergencyCharges1 { get; set; }
    }

    public class Services_TPAWise
    {
        public int ServicesTPAWiseID { get; set; }


        // public int OrganizationID { get; set; }

        //   public string[] OrganizationID { get; set; }


    }

}

