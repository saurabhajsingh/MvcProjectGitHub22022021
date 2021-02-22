using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace KeystoneProject.Models.Master
{
    public class ServicesPackage
    {
        public int ServiceID { get; set; }
        public int WardID { get; set; }
        public string ServiceGroupID { get; set; }
        public string ServiceGroupName { get; set; }
        public string Code { get; set; }
        public string HospitalHSNCode { get; set; }
        public string TPAHSNCode { get; set; }
        public Boolean chkrechange { get; set; }
        public string SPWchargeID { get; set; }
        public string ServicePackageID { get; set; }
           
       public string WardName{ get; set; }	
       
        public string WordName { get; set; }
        public string ServiceName { get; set; }

        public int servicedetailpkgID1 { get; set; }
        public string UnitID { get; set; }
        public string ColumnName { get; set; }

        public string RowsColumn { get; set; }

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

        public int serviceID1 { get; set; }

        public DataSet StoreAllServices { get; set; }

        public DataSet dsServicesCharges { get; set; }

        public DataSet dsServicePackagesCharges { get; set; }

        public DataSet dsServicesTPAWise { get; set; }

        //   public Services_TPAWise[] OrganizationID {get;  set; }

        public DataSet dsWardDetailsTPAWise { get; set; }

        public int ServiceOrTestID { get; set; }

        public string ServiceOrTestName { get; set; }
        public int Quantity { get; set; }

        public int ServiceDetailsPackagelID { get; set; }

        public string UnitName { get; set; }
         


      
    }
    public class ServicesPackage_ServicesCharges
    {
        //public int WardID { get; set; }
        public int ServiceChargesID { get; set; }
        public string[] WardID { get; set; }
        public string[] GeneralCharges1 { get; set; }
        public string[] EmergencyCharges1 { get; set; }
        public string[] HiddenGenCharges { get; set; }
        public string[] GenralCharges { get; set; }

        public string[] ServiceNTestName { get; set; }

        public string[] Quantity { get; set; }
        public string[] ServiceOrTestID { get; set; }
    }

    public class ServicePackage_OPDCharges
    {
        public int ServiceID { get; set; }
        public string[] ServiceOrTestIDOPD { get; set; }
        public string[] QuantityOPD { get; set; }
        public string[] ServiceNTestNameOPD { get; set; }

        public string[] OPDGenralCharges { get; set; }
        public string[] OPDEmergencyCharges { get; set; }

        public string GeneralCharges { get; set; }

        public string ServiceNTestName { get; set; }
        public int ServiceOrTestID { get; set; }
        public int Quantity { get; set; }
        public string EmergencyCharges { get; set; }

        public string ServiceType { get; set; }


    }
    public class ServicesPackage_Services_TPAWise
    {
        public int ServicesTPAWiseID { get; set; }

        // public int OrganizationID { get; set; }

        //   public string[] OrganizationID { get; set; }


    }
}
