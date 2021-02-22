using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace KeystoneProject.Models.Master
{
    public class MedicineLibrary
    {
        public int LocationID { get; set; }
        public int HospitalID { get; set; }

        public int MedicineLibraryID { get; set; }
        public string Type { get; set; }

        public string txtdeteils { get; set; }
        public string Medicines { get; set; }

 
        public string Unit { get; set; }
        public string Time { get; set; }
        public string Instruction { get; set; }
        public string Frequency1 { get; set; }
        public string Frequency2 { get; set; }
        public string Frequency3 { get; set; }
        public string Frequency4 { get; set; }

        public string Strength { get; set; }


        public string Days { get; set; }
      
        public string Quantity { get; set; }
        public string CompositionName { get; set; }
        public string Packages { get; set; }
        public DataSet StoreAllMedicines{ get; set; }
        public string Mode { get; set; }

        
        
        public int UnitID { get; set; }

        public string UnitName { get; set; }

        public int DrugID { get; set; }

        public string DrugName { get; set; }


        public string MedicineTypeName { get; set; }
        public int MedicineTypeID { get; set; }

        public string Frequency { get; set; }
        
    }
}