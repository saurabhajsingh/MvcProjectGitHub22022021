using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeystoneProject.Models.PharmacyMaster
{
    public class TaxInformationCompany
    {
        public int HospitalID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string taxInformationType { get; set; }
        public string taxType { get; set; }
        public string taxInforamtionTypeTaxRate { get; set; }
        public string interState { get; set; }
        public string formType { get; set; }
        public string interStateTaxRate { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string postingAccount { get; set; }
        public string taxInformationDetailTaxRate { get; set; }
        public string taxInformationDetailTaxAccount { get; set; }
        public string surcharge { get; set; }
        public string surchargeAccount { get; set; }
        public string rateInclusiveInTax { get; set; }
        public string taxInclusiveInRates { get; set; }
        public string taxInclusiveInRatesForLTE { get; set; }
        public string addCharge { get; set; }
        public string discount { get; set; }
        public string abatementRate { get; set; }
        public string chargePromotionalRate { get; set; }
        public string addICOnFreeGoods { get; set; }
        public string calculateTaxBeforeDiscount { get; set; }
        public string chargeTaxOnFreeGoods { get; set; }
        public string taxOfFreeGoodsOnGrossValue { get; set; }
        public string taxCalculateBasedOnMRP { get; set; }
        public string abatementRebateForLTE { get; set; }
        public int taxInformationTypeID { get; set; }
        public string postingAccountID { get; set; }
        public string taxInformationDetailTaxAccountID { get; set; }
        public string surchargeAccountID { get; set; }


    }
}