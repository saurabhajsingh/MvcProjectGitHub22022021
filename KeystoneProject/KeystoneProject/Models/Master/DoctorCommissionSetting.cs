using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace KeystoneProject.Models.Master
{
    public class DoctorCommissionSetting
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>


        public string DoctorID
        {
            get;
            set;
        }
        public string DoctorName
        {
            get;
            set;
        }
        public string Type
        { get; set; }

        public string ServiceGroupIDLab
        {
            get;
            set;
        }
        public string ServiceGroupNameLab
        {
            get;
            set;
        }
        public string ServiceGroupID
        {
            get;
            set;
        }
        public string ServiceGroupName
        {
            get;
            set;
        }
        public string ServiceGroupIDIPD
        {
            get;
            set;
        }
        public string ServiceGroupNameIPD
        {
            get;
            set;
        }
        public string ServiceName
        {
            get;
            set;
        }
        public int ServiceID
        {
            get;
            set;
        }

        public string ServiceNameIPD
        {
            get;
            set;
        }
        public int ServiceIDIPD
        {
            get;
            set;
        }

        public string ServiceNameLab
        {
            get;
            set;
        }
        public int ServiceIDLab
        {
            get;
            set;
        }
        public string Department
        {
            get;
            set;
        }
        public int DoctorCommissionSettingID
        {
            get;
            set;
        }
        public string CheckRow { get; set; }
        public string doc { get; set; }
        public string Mode { get; set; }
        public string CommissionTypeOPD
        {
            get;
            set;
        }
        public decimal FixedOPD
        {
            get;
            set;
        }
        public string CommissionTypeIPD
        {
            get;
            set;
        }
        public decimal FixedIPD
        {
            get;
            set;
        }
        public string CommissionTypeLab
        {
            get;
            set;
        }
        public string DoctorType
        {
            get;
            set;
        }
        public decimal FixedLab
        {
            get;
            set;
        }
        public DataSet dsDoctorCommissionAllShow
        {
            get;
            set;
        }

        public string IEOPD
        {
            get;
            set;
        }
        public string IEIPD
        {
            get;
            set;
        }

        public string IELAB
        {
            get;
            set;
        }
        public string CommissionOPD
        {
            get;
            set;
        }
        public string CommissionIPD
        {
            get;
            set;
        }
        public string CommissionLAB
        {
            get;
            set;
        }


        public string FixedOPDRsType
        {
            get;
            set;
        }
        public string FixedIPDType
        {
            get;
            set;
        }
        public string FixedLabType
        {
            get;
            set;
        }

       

        public DoctorCommissiondgv[] filldgv
        {
            get;
            set;
        }

        public OPDDGV1[] OPDdgv
        {
            get;
            set;
        }
        public IPDDGV1[] IPDdgv
        {
            get;
            set;
        }
        public LAB1[] LABdgv
        {
            get;
            set;
        }
    }
}

public class DoctorCommissiondgv
{
    public string FixedOPDRsType
    {
        get;
        set;
    }
    public string FixedIPDType
    {
        get;
        set;
    }
    public string FixedLabType
    {
        get;
        set;
    }
    public decimal FixedIPDdgv
    {
        get;
        set;
    }
    public decimal FixedLabdgv
    {
        get;
        set;
    }
    public decimal FixedOPDdgv
    {
        get;
        set;
    }
    public string IEOPDdgv
    {
        get;
        set;
    }
    public string IEIPDdgv
    {
        get;
        set;
    }
    public string DoctorName
    {
        get;
        set;
    }
    public int DoctorID
    {
        get;
        set;
    }
    public string IELABdgv
    {
        get;
        set;
    }
    public int DoctorCommissionSettingIDdv
    {
        get;
        set;
    }
    public string ServiceNameIPDdgv
    {
        get;
        set;
    }
    public string ServiceNameLABdgv
    {
        get;
        set;
    }

    public string ServiceNamedgv
    {
        get;
        set;
    }


    public string ServiceIDLabdgv
    {
        get;
        set;
    }
    public string IELABdgv1
    {
        get;
        set;
    }


    public string IEIPDdgv1
    {
        get;
        set;
    }
    public string ServiceIDdgv
    {
        get;
        set;
    }
    public string ModeDgvOPD
    {
        get;
        set;
    }
    public string IEOPDdgv1
    {
        get;
        set;
    }

    public string ServiceIDIPDdgv
    {
        get;
        set;
    }

    public string CommissionLABdgv
    {
        get;
        set;
    }
    public string CommissionOPDdgv
    {
        get;
        set;
    }
    public string CommissionIPDdgv
    {
        get;
        set;
    }
    //------------


    public string CommissionOPDRsTypedgv
    {
        get;
        set;
    }
    public string CommissionIPDTypedgv
    {
        get;
        set;
    }
    public string CommissionLABTypedgv
    {
        get;
        set;
    }
}
public class OPDDGV
{
    public string[] ServiceIDdgv
    {
        get;
        set;
    }
    public string[] ModeDgvOPD
    {
        get;
        set;
    }
    public string[] ServiceNamedgv
    {
        get;
        set;
    }
    public string[] ServiceGroupIDdgv
    {
        get;
        set;
    }
    public string[] ServiceGroupNamedgv
    {
        get;
        set;
    }
    public string[] CommissionOPDRsTypedgv
    {
        get;
        set;
    }
    public string[] CommissionOPDdgv
    {
        get;
        set;
    }

    public string[] IEOPDdgv
    {
        get;
        set;
    }
}
public class IPDDGV
{
    public string[] ServiceNameIPDdgv
    {
        get;
        set;
    }
    public string[] ServiceIDIPDdgv
    {
        get;
        set;
    }
    public string[] ServiceGroupNameIPDdgv
    {
        get;
        set;
    }
    public string[] ServiceGroupIDIPDdgv
    {
        get;
        set;
    }

    public string[] CommissionIPDTypedgv
    {
        get;
        set;
    }
    public string[] CommissionIPDdgv
    {
        get;
        set;
    }
    public string[] IEIPDdgv
    {
        get;
        set;
    }
}
public class LAB
{
    public string[] CommissionLABTypedgv
    {
        get;
        set;
    }
    public string[] CommissionLABdgv
    {
        get;
        set;
    }
    public string[] IELABdgv
    {
        get;
        set;
    }

    public string[] ServiceNameLABdgv
    {
        get;
        set;
    }
    public string[] ServiceIDLabdgv
    {
        get;
        set;
    }
    public string[] ServiceGroupNameLABdgv
    {
        get;
        set;
    }
    public string[] ServiceGroupIDLabdgv
    {
        get;
        set;
    }
}


//----------------------------------------------------------------------------------

public class OPDDGV1
{
    public string ServiceIDdgv
    {
        get;
        set;
    }

    public string ModeDgvOPD
    { get; set; }
    public string ServiceNamedgv
    {
        get;
        set;
    }
    public string ServiceGroupIDdgv
    {
        get;
        set;
    }
    public string ServiceGroupNamedgv
    {
        get;
        set;
    }
    public string CommissionOPDRsTypedgv
    {
        get;
        set;
    }
    public string CommissionOPDdgv
    {
        get;
        set;
    }

    public string IEOPDdgv
    {
        get;
        set;
    }
}
public class IPDDGV1
{
    public string ServiceNameIPDdgv
    {
        get;
        set;
    }
    public string ServiceIDIPDdgv
    {
        get;
        set;
    }
    public string ServiceGroupNameIPDdgv
    {
        get;
        set;
    }
    public string ServiceGroupIDIPDdgv
    {
        get;
        set;
    }
    public string CommissionIPDTypedgv
    {
        get;
        set;
    }
    public string CommissionIPDdgv
    {
        get;
        set;
    }
    public string IEIPDdgv
    {
        get;
        set;
    }
}
public class LAB1
{
    public string CommissionLABTypedgv
    {
        get;
        set;
    }
    public string CommissionLABdgv
    {
        get;
        set;
    }
    public string IELABdgv
    {
        get;
        set;
    }

    public string ServiceNameLABdgv
    {
        get;
        set;
    }
    public string ServiceIDLabdgv
    {
        get;
        set;
    }
    public string ServiceGroupNameLABdgv
    {
        get;
        set;
    }
    public string ServiceGroupIDLabdgv
    {
        get;
        set;
    }
}
