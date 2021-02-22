using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace KeystoneProject.Models.Master
{
    public class PrivilegeCard
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int PrivilegeCardID { get; set; }

        public int PrivilegePriceDetailID { get; set; }

        public string[] PriceName { get; set; }

        public decimal[] Price { get; set; }


        public int PrivilegeServiceDetailID { get; set; }

        public int PrivilegeTestDetailID { get; set; }

     

        public int ServiceID { get; set; }

        public string ServiceName { get; set; }

        public int TestID { get; set; }

        public string TestName { get; set; }


        public string CardName { get; set; }

        public string Path { get; set; }

        public string Remark { get; set; }

        public int ServiceGroupID { get; set; }

        public string ServiceGroupName { get; set; }

        public decimal DiscountPer { get; set; }

        public DataSet StoreAllUnit { get; set; }
        public string Mode { get; set; }

        public decimal DiscountService { get; set; }

        public decimal DiscountTest { get; set; }


        public priviledgecardarray1[] filldgv
        {
            get;
            set;
        }

        public priviledgecardServiceGroup[] ServiceGroupArray
        {
            get;
            set;
        }

        public priviledgecardServiceName[] ServiceNameArray
        {
            get;
            set;
        }

        public priviledgecardTestName[] TestNameArray
        {
            get;
            set;
        }

        public priviledgecardPriceDetail[] cardPriceDetailArray
        {
            get;
            set;
        }
    }

   public class priviledgecardarray
    {
       public int[] PrivilegeServiceGroupID { get; set; }
        public int[] ServiceID1 { get; set; }

        public string[] ServiceName1 { get; set; }
        public int[] ServiceGroupID{ get; set; }

        public string[] ServiceGroupName { get; set; }

        public int[] TestID1 { get; set; }

        public string[] TestName1 { get; set; }

        public decimal[] DiscountPer { get; set; }

        public decimal[] DiscountService { get; set; }
        public decimal[] DiscountTest { get; set; }

        public string[] PriceName { get; set; }
        public decimal[] Price { get; set; }
        public int[] PrivilegeServiceDetailID { get; set; }
        public int[] PrivilegeTestDetailID { get; set; }
        public int[] PrivilegePriceDetailID { get; set; }
        public int PrivilegeCardID { get; set; }
    }


   public class priviledgecardarray1
   {

       public int PrivilegeCardID { get; set; }
       public int PrivilegeServiceGroupID { get; set; }
       public int ServiceID1 { get; set; }

       public string ServiceName1 { get; set; }
       public int ServiceGroupID { get; set; }

       public string ServiceGroupName { get; set; }

     

     

       public decimal DiscountService { get; set; }
      

     


       public string CardName { get; set; }

       public string Path { get; set; }

       public string Remark { get; set; }


       public int PrivilegeServiceDetailID { get; set; }

       public int PrivilegeTestDetailID { get; set; }

      
   }

   public class priviledgecardServiceGroup
   {
       public int PrivilegeServiceGroupID { get; set; }
       public int ServiceGroupID { get; set; }
       public decimal DiscountPer { get; set; }
       public string ServiceGroupName { get; set; }
   }


   public class priviledgecardServiceName
   {
       public int PrivilegeServiceDetailID { get; set; }

       public int PrivilegeCardID { get; set; }
       public int ServiceID1 { get; set; }
       public decimal DiscountService { get; set; }
       public string ServiceName1 { get; set; }
   }

   public class priviledgecardTestName
   {
       public int PrivilegeTestDetailID { get; set; }
       public int PrivilegeCardID { get; set; }
       public int TestID1 { get; set; }
       public decimal DiscountTest { get; set; }
       public string TestName1 { get; set; }
   }

   public class priviledgecardPriceDetail
   {
       public int PrivilegeCardID { get; set; }
       public string PriceName { get; set; }
       public int PrivilegePriceDetailID { get; set; }
       public decimal Price { get; set; }

   }
}