using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Pharmacy;
using KeystoneProject.Buisness_Logic.Pharmacy;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace KeystoneProject.Controllers.Pharmacy
{
    public class PatientMedicalBillController : Controller
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
        PatientMedicalBill obj = new Models.Pharmacy.PatientMedicalBill();
        private SqlConnection con;

        private void Connect()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }

        //
        // GET: /PatientMedicalBill/

        public ActionResult PatientMedicalBill()
        {
            return View();
        }

        Decimal QtyUpper = 0;
        Decimal QtyLower = 0;
        public Boolean GetQuantity(decimal Contain, decimal CurrentStock, decimal CurrentStock1)
        {

            decimal AddStock = 0;
            string AddSplit = "";
            int i = 0;
            QtyUpper = 0;
            QtyLower = 0;

            if (Contain > 0 && CurrentStock > 0 && CurrentStock1 > 0)
            {
                AddStock = CurrentStock1 / Contain;
                if (AddStock >= 1)
                {
                    AddSplit = AddStock.ToString();
                    i = AddSplit.IndexOf(".");
                    if (i > 0)
                    {
                        AddSplit = AddSplit.Substring(0, i);
                    }
                }
                else
                { return false; }

                AddStock = Convert.ToDecimal(AddSplit);


                if (AddStock >= 1)
                {
                    QtyUpper = AddStock + CurrentStock;
                    QtyLower = CurrentStock1 - (Contain * AddStock);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetShortDatetForAdtp(DateTime TextDate)
        {
            string ShortDate = "";
            string Month = "";
            string Year = "";

            if (TextDate.Date.Month.ToString().Length == 1)
            {
                ShortDate = "0" + TextDate.Date.Month + "-" + TextDate.Date.Year.ToString().Substring(2, 2);
            }
            else
            {
                ShortDate = TextDate.Date.Month + "" + TextDate.Date.Year.ToString().Substring(2, 2);
            }
            return ShortDate;
        }
        public ActionResult BindCustomer(string Name)
        {
            DataSet ds = BL_obj.BindCustomer(Name);
            List<PatientMedicalBill> obj = new List<Models.Pharmacy.PatientMedicalBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalBill
                {
                    CustomerID = dr["CustomerID"].ToString(),
                    CustomerName = dr["CustomerName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindSalesPerson(string Name)
        {
            DataSet ds = BL_obj.BindSalesPerson(Name);
            List<PatientMedicalBill> obj = new List<Models.Pharmacy.PatientMedicalBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalBill
                {
                    SalesPersonID = dr["SalesPersonID"].ToString(),
                    SalesPersonName = dr["SalesPersonName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindProductName(string Name)
        {
            DataSet ds = BL_obj.BindProductName(Name);
            List<PatientMedicalBill> obj = new List<Models.Pharmacy.PatientMedicalBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalBill
                {
                    ProductID = dr["ProductID"].ToString(),
                    ProductName = dr["ProductName"].ToString(),
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };

            //  return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindDoctor(string Name)
        {
            DataSet ds = BL_obj.BindDoctor(Name);
            List<PatientMedicalBill> obj = new List<Models.Pharmacy.PatientMedicalBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalBill
                {
                    DoctorID = dr["DoctorID"].ToString(),
                    DoctorPrintName = dr["DoctorPrintName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult BindPatient(string Name)
        {
            DataSet ds = BL_obj.BindPatient(Name);
            List<PatientMedicalBill> obj = new List<Models.Pharmacy.PatientMedicalBill>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalBill
                {
                    PatientRegNO = dr["PatientRegNO"].ToString(),
                    PatientName = dr["PatientName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public ActionResult BindBatchNoOfProduct(int ProductID)
        //{
        //    DataSet ds = BL_obj.GetMedicalBillForProductStock(ProductID);
        //    List<PatientMedicalBill> obj1 = new List<Models.Pharmacy.PatientMedicalBill>();

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        Connect();
        //        SqlCommand cmd = new SqlCommand("select  ProductDetailsID ,BatchNo,ExpiryDate,SalesRate,CurrentStock ,CurrentStock1, PurchaseRate,MRP  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and CurrentStock > 0  and Convert(varchar(10),ExpiryDate,120) > (GETDATE()) and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by ExpiryDate asc", con);
        //        con.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        con.Close();
        //    }
        //    else
        //    {
        //        Connect();
        //        SqlCommand cmd = new SqlCommand("select  ProductDetailsID ,BatchNo,ExpiryDate, SalesRate,CurrentStock ,CurrentStock1, PurchaseRate,MRP,SalesRate  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and  Convert(varchar(10),ExpiryDate,120) > (GETDATE()) and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by ExpiryDate asc", con);
        //        con.Open();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        con.Close();
        //    }
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        obj1.Add(new Models.Pharmacy.PatientMedicalBill
        //        {
        //            ProductDetailsID = dr["ProductDetailsID"].ToString(),
        //            batchNumber = dr["BatchNo"].ToString()
        //        });
        //    }
        //    return new JsonResult { Data = obj1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        public void FillData()
        {
            DataSet dsPatient = new DataSet();
            dsPatient.Reset();
            dsPatient = BL_obj.GetPatientMedicalBills(Convert.ToInt32(obj.PatientRegNO));

            if (dsPatient.Tables[0].Rows.Count > 0)
            {
                if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "IPD")
                {
                    obj.PatientRegNO = dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();
                    // ucPatientMedicalBill1.txtPatientIPDNo.Text = dsPatient.Tables[0].Rows[0]["PatientIPDNO"].ToString();
                    obj.PatientName = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
                    obj.DoctorID = dsPatient.Tables[0].Rows[0]["DoctorID"].ToString();
                    //ucPatientMedicalBill1.txtSalesPersonName.Text = dsPatient.Tables[0].Rows[0]["GuardianName"].ToString();
                    //ucPatientMedicalBill1.atxtWardName.Text = dsPatient.Tables[0].Rows[0]["WardID"].ToString();
                    //ucPatientMedicalBill1.atxtRoomNo.Text = dsPatient.Tables[0].Rows[0]["RoomID"].ToString();
                    //ucPatientMedicalBill1.atxtBedNo.Text = dsPatient.Tables[0].Rows[0]["BedID"].ToString();
                    obj.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();

                    // ucPatientMedicalBill1.txtAge.Text = dsPatient.Tables[0].Rows[0]["Age"].ToString();
                    //ucPatientMedicalBill1.cmbAgeType.Text = dsPatient.Tables[0].Rows[0]["AgeType"].ToString();
                    //ucPatientMedicalBill1.txtMobileNo.Text = dsPatient.Tables[0].Rows[0]["MobileNo"].ToString();
                    obj.Address = dsPatient.Tables[0].Rows[0]["Address"].ToString();
                }

                else if (dsPatient.Tables[0].Rows[0]["PatientType"].ToString() == "OPD")
                {
                    obj.PatientRegNO = dsPatient.Tables[0].Rows[0]["PatientRegNO"].ToString();
                    //  ucPatientMedicalBill1.txtPatientIPDNo.Text = dsPatient.Tables[0].Rows[0]["PatientOPDNO"].ToString();
                    obj.PatientName = dsPatient.Tables[0].Rows[0]["PatientName"].ToString();
                    obj.DoctorID = dsPatient.Tables[0].Rows[0]["DoctorID"].ToString();
                    //ucPatientMedicalBill1.txtSalesPersonName.Text = dsPatient.Tables[0].Rows[0]["GuardianName"].ToString();
                    obj.PatientType = dsPatient.Tables[0].Rows[0]["PatientType"].ToString();
                    // ucPatientMedicalBill1.txtAge.Text = dsPatient.Tables[0].Rows[0]["Age"].ToString();
                    // ucPatientMedicalBill1.cmbAgeType.Text = dsPatient.Tables[0].Rows[0]["AgeType"].ToString();
                    // ucPatientMedicalBill1.txtMobileNo.Text = dsPatient.Tables[0].Rows[0]["MobileNo"].ToString();
                    obj.Address = dsPatient.Tables[0].Rows[0]["Address"].ToString();
                }
                if (dsPatient.Tables[1].Rows.Count > 0)
                {
                    if (dsPatient.Tables[1].Rows[0]["PreBalance"].ToString() != null && dsPatient.Tables[1].Rows[0]["PreBalance"].ToString() != "")
                    {
                        string PreBalance = dsPatient.Tables[1].Rows[0]["PreBalance"].ToString();
                        string CurrentPreBalance = PreBalance;
                        //ucPatientMedicalBill1.txtPreBalance.Text = dsPatient.Tables[1].Rows[0]["PreBalance"].ToString();
                        if (Convert.ToDecimal(PreBalance) < 0)
                        //if (Convert.ToDecimal(ucPatientMedicalBill1.txtPreBalance.Text) < 0)
                        {
                            //ucPatientMedicalBill1.lblPreBalance.Enabled = true;
                            ////PreBalance = PreBalance.Replace("-", "");
                            //ucPatientMedicalBill1.txtPreBalance.Text = PreBalance.Replace("-", "");
                            //ucPatientMedicalBill1.lblPreBalance.Text = "Deposit";
                            //ucPatientMedicalBill1.lblPreBalance.ForeColor = System.Drawing.Color.Blue;
                            //ucPatientMedicalBill1.btnRefound.ButtonText = "Refound Amount";
                            //ucPatientMedicalBill1.btnRefound.Visible = true;
                        }
                        else
                        {
                            //ucPatientMedicalBill1.lblPreBalance.Enabled = true;
                            //ucPatientMedicalBill1.txtPreBalance.Text = dsPatient.Tables[1].Rows[0]["PreBalance"].ToString();
                            //ucPatientMedicalBill1.lblPreBalance.Text = "Pre. Balance";
                            //ucPatientMedicalBill1.lblPreBalance.ForeColor = System.Drawing.Color.Red;

                        }

                    }
                    else
                    {
                        //  ucPatientMedicalBill1.txtPreBalance.Text = "0.00";
                    }
                }
                //  ucPatientMedicalBill1.txtServiceTax.Text = dsPatient.Tables[2].Rows[0]["ServiceCharge"].ToString();
                //   BindOLDBillNo();

            }
        }

        public void BindOLDBillNo(int PatientRegNO)
        {
            List<PatientMedicalBill> objList = new List<Models.Pharmacy.PatientMedicalBill>();
            DataSet dsPatientBillNo = new DataSet();
            // DataSet dsOldBillReprint = new DataSet();

            dsPatientBillNo.Reset();
            string Type = "";

            Type = "" + obj.PatientType + "MedicalBill";
            if (obj.CustomerID != null && obj.CustomerPrintName.Length > 0)
            {
                if (obj.PatientRegNO == null || obj.PatientRegNO == "")
                {
                    PatientRegNO = Convert.ToInt32(obj.CustomerID);
                    Type = "GeneralMedicalBill";
                }
                dsPatientBillNo = BL_obj.GetPatientOLDBillsNO(PatientRegNO, Type);

                DataSet dsOldBillReprint = new DataSet();
                dsOldBillReprint.Tables.Add(dsPatientBillNo.Tables[0].Copy());
                if (dsPatientBillNo.Tables[0].Rows.Count > 0)
                {
                    ////if (this.Mode.ToString() == "Edit")
                    ////{
                    ////    ucPatientMedicalBill1.cmbOldBillsNo.Enabled = true;
                    ////}
                    foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                    {
                        objList.Add(new Models.Pharmacy.PatientMedicalBill
                        {
                            BillNo = dr["BillNo"].ToString(),
                            BillNoDate = dr["BillNo&Date"].ToString(),

                        });
                    }

                    //  ucPatientMedicalBill1.dgvOldRePrint.DataSource = dsOldBillReprint.Tables[0];

                }
                else
                {
                    foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                    {
                        objList.Add(new Models.Pharmacy.PatientMedicalBill
                        {
                            BillNo = dr["BillNo"].ToString(),
                            BillNoDate = dr["BillNo&Date"].ToString(),

                        });
                    }

                    //  ucPatientMedicalBill1.dgvOldRePrint.DataSource = dsPatientBillNo.Tables[0];
                }
            }
            else
            {


                dsPatientBillNo = BL_obj.GetPatientOLDBillsNO(PatientRegNO, Type);

                DataSet dsOldBillReprint = new DataSet();
                dsOldBillReprint.Tables.Add(dsPatientBillNo.Tables[0].Copy());

                if (dsPatientBillNo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                    {
                        objList.Add(new Models.Pharmacy.PatientMedicalBill
                        {
                            BillNo = dr["BillNo"].ToString(),
                            BillNoDate = dr["BillNo&Date"].ToString(),

                        });
                    }


                    //  ucPatientMedicalBill1.dgvOldRePrint.DataSource = dsOldBillReprint.Tables[0];

                }
                else
                {
                    foreach (DataRow dr in dsPatientBillNo.Tables[0].Rows)
                    {
                        objList.Add(new Models.Pharmacy.PatientMedicalBill
                        {
                            BillNo = dr["BillNo"].ToString(),
                            BillNoDate = dr["BillNo&Date"].ToString(),

                        });
                    }

                    //   ucPatientMedicalBill1.dgvOldRePrint.DataSource = dsPatientBillNo.Tables[0];
                }
            }
        }
        public ActionResult FillDataByProductID(int ProductDetailsID)
        {
            obj.Gst = 0;
            obj.DiscountValue = "";
            DataSet dsProductDetail = new DataSet();
            dsProductDetail = BL_obj.GetServicesDetailsforMedicalBills(ProductDetailsID);
            if (dsProductDetail.Tables[0].Rows.Count > 0)
            {
                if (dsProductDetail.Tables[0].Rows[0]["Contain"].ToString().Trim() != "")
                {
                    obj.Contain = Convert.ToDecimal((dsProductDetail.Tables[0].Rows[0]["Contain"].ToString().Trim()));
                }
                DataSet ds = new DataSet();

                ds = BL_obj.GetGSTRatio();
                if (dsProductDetail.Tables[0].Rows[0]["GST"].ToString() != "")
                {

                    obj.Gst = Convert.ToDecimal(dsProductDetail.Tables[0].Rows[0]["GST"].ToString());
                }

                long Gstper = (int)obj.Gst;

                string valueGst = Gstper + "%".ToString();
                DataView dv1 = new DataView(ds.Tables[0], " GSTTAX = '" + valueGst + "' ", "", DataViewRowState.CurrentRows);
                obj.DiscountValue = "0";
                if (dv1.ToTable().Rows.Count > 0)
                {
                    obj.DiscountValue = dv1.ToTable().Rows[0]["GstDiscValue"].ToString();
                }

                obj.netRate = dsProductDetail.Tables[0].Rows[0]["SalesRate"].ToString();
                obj.QtyU = dsProductDetail.Tables[0].Rows[0]["Packing"].ToString();
                obj.QtyL = dsProductDetail.Tables[0].Rows[0]["ProductLowerUnitName"].ToString();

                obj.mrp = dsProductDetail.Tables[0].Rows[0]["MRP"].ToString();
                if (Convert.ToDateTime(dsProductDetail.Tables[0].Rows[0]["ExpiryDate"].ToString()) < DateTime.Now)
                {
                    obj.expiryDate = dsProductDetail.Tables[0].Rows[0]["ExpiryDate"].ToString();
                    // ucPatientMedicalBill1.lblExpirydate.ForeColor = Color.Red;
                }
                else
                {
                    obj.expiryDate = dsProductDetail.Tables[0].Rows[0]["ExpiryDate"].ToString();
                    // ucPatientMedicalBill1.lblExpirydate.ForeColor = Color.Black;
                }
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult AjaxMethod_PatName(string patnm)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
            KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
            List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

            DataSet ds = BL_obj.BindRegno(patnm);
            DataTable dt = new DataTable();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = BL_obj.Bind_patDetail(Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNO"]));

                foreach (DataRow dr in dt.Rows)
                {
                    searchList.Add(
                        new PatientMedicalBill
                        {
                            PatientName = Convert.ToString(dr["PatientName"]),
                            PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                            CustomerName = Convert.ToString(dr["PatientName"]),
                            PatientType = Convert.ToString(dr["PatientType"]),
                            Address = Convert.ToString(dr["Address"]),
                            DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                        });
                }
            }
            return new JsonResult
            {
                Data = searchList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public ActionResult AjaxMethod_OldBill(string patnm)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
            KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
            List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

            DataTable dt = new DataTable();
            DataSet ds = BL_obj.BindRegno(patnm);
            if (ds.Tables[0].Rows.Count > 0)
                dt = BL_obj.GetOldBill(Convert.ToInt16(ds.Tables[0].Rows[0]["PatientRegNO"].ToString()));
            
            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalBill
                    {
                        BillNo = Convert.ToString(dr["BillNo&Date"]),
                    });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult AjaxMethod_regno(int regno)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
            KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
            List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

            DataTable dt = new DataTable();
            dt = BL_obj.Bind_patDetail(regno);

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalBill
                    {
                        PatientName = Convert.ToString(dr["PatientName"]),
                        PatientRegNO = Convert.ToString(dr["PatientRegNO"]),
                        CustomerName = Convert.ToString(dr["PatientName"]),
                        PatientType = Convert.ToString(dr["PatientType"]),
                        Address = Convert.ToString(dr["Address"]),
                        DoctorPrintName = Convert.ToString(dr["DoctorPrintName"]),
                    });
            }
            return new JsonResult
            {
                Data = searchList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public ActionResult AjaxMethod_OldBill_regno(int regno)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
            KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
            List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

            DataTable dt = new DataTable();
            dt = BL_obj.GetOldBill(regno);

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalBill
                    {
                        BillNo = Convert.ToString(dr["BillNo&Date"]),
                    });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult AjaxMethod_CustName(string custNM)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
            KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
            List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

            DataSet ds = BL_obj.BindCustID(custNM);
            DataTable dt = new DataTable();

            dt = BL_obj.Bind_CustDetail(Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerID"]));

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalBill
                    {
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        Address = Convert.ToString(dr["Address"]),
                    });
            }
            return new JsonResult
            {
                Data = searchList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        //public ActionResult AjaxMethod_CustOldBill(string custNM)
        //{
        //    KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
        //    KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
        //    List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

        //    DataTable dt = new DataTable();
        //    DataSet ds = BL_obj.BindCustID(custNM);
        //    dt = BL_obj.GetOldBill(Convert.ToInt16(ds.Tables[0].Rows[0]["CustomerID"].ToString()));

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        searchList.Add(
        //            new PatientMedicalBill
        //            {
        //                BillNo = Convert.ToString(dr["BillNo"]),
        //            });
        //    }

        //    return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        //}

        public ActionResult AjaxMethod_batchno(string prodnm)
        {
            KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill BL_obj = new BL_PatientMedicalBill();
            KeystoneProject.Models.Pharmacy.PatientMedicalBill obj = new PatientMedicalBill();
            List<PatientMedicalBill> searchList = new List<PatientMedicalBill>();

            DataTable dt = new DataTable();
            DataSet ds2 = new DataSet();

            DataSet ds1 = BL_obj.BindProductName(prodnm);

            int ProductID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ProductID"].ToString());

            DataSet ds = BL_obj.GetMedicalBillForProductStock(ProductID);

            List<PatientMedicalBill> obj1 = new List<Models.Pharmacy.PatientMedicalBill>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Connect();
                SqlCommand cmd = new SqlCommand("select  ProductDetailsID ,BatchNo,ExpiryDate,SalesRate,CurrentStock ,CurrentStock1, PurchaseRate,MRP  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and CurrentStock > 0  and Convert(varchar(10),ExpiryDate,120) > (GETDATE()) and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by ExpiryDate asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds2);
                con.Close();

            }
            else
            {
                Connect();
                SqlCommand cmd = new SqlCommand("select  ProductDetailsID ,BatchNo,ExpiryDate,SalesRate,CurrentStock ,CurrentStock1, PurchaseRate,MRP  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and Convert(varchar(10),ExpiryDate,120) > (GETDATE()) and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by ExpiryDate asc", con);
                // SqlCommand cmd = new SqlCommand("select ProductDetailID,ProductPurchaseDetails.ProductID,BatchNo,Expiry from ProductPurchaseDetails left join Product  on Product.ProductID = ProductPurchaseDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and  Convert(varchar(10),Expiry,120) > (GETDATE()) and ProductPurchaseDetails.ProductID =" + ProductID + "  and ProductPurchaseDetails.RowStatus = 0 order by Expiry asc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds2);
                con.Close();
            }
            string color = "";
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                string date = Convert.ToDateTime(dr["ExpiryDate"]).AddDays(-90).ToString();
                if (Convert.ToDateTime(DateTime.Now) > Convert.ToDateTime(date))
                {
                    color = "#90EE90";
                }
                else
                {
                    color = "#D3D3D3";
                }
                obj1.Add(new Models.Pharmacy.PatientMedicalBill
                {
                    ProductDetailsID = dr["ProductDetailsID"].ToString(),
                    batchNumber = dr["BatchNo"].ToString(),
                    expiryDate = dr["ExpiryDate"].ToString(),
                    ExparyDateColor = color,
                });
            }
            return new JsonResult { Data = obj1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult PatientMedicalBill(PatientMedicalBill obj, FormCollection fc)

        {
            try
            {
                //obj.MedicalBillID = Convert.ToInt32(fc["MedicalBillID"]);
                //obj.PatientRegNO = fc["PatientRegNO"].ToString();
                //obj.PatientName = fc["PatientName"].ToString();
                //obj.PatientType = fc["PatientType"].ToString();
                //obj.DoctorPrintName = fc["DoctorPrintName"].ToString();
                //obj.DoctorID = fc["DoctorID"].ToString();
                //obj.Address = fc["address"].ToString();
                obj.BillNo = fc["oldBillNumber"].ToString();
                obj.BillNoDate = fc["billDate"].ToString();
                //obj.message = fc["message"].ToString();
                //obj.CustomerName = fc["CustomerName"].ToString();
                //obj.CustomerID = fc["CustomerID"].ToString();
                //obj.SalesPersonName = fc["SalesPersonName"].ToString();
                //obj.SalesPersonID = fc["SalesPersonID"].ToString();

                //obj.grossTotal = fc["grossTotal"].ToString();
                //obj.totalAmount = fc["totalAmount"].ToString();
                //obj.discountAmount = fc["discountAmount"].ToString();
                //obj.discountPercentage = fc["discountPercentage"].ToString();
                //obj.vatAmount = fc["vatAmount"].ToString();
                obj.otheLess = fc["otheLess"].ToString();
                //obj.salesReturn = fc["salesReturn"].ToString();
                //obj.netAmount = fc["netAmount"].ToString();
                //obj.paymentType = fc["paymentType"].ToString();
                //obj.cashRecipt = fc["cashRecipt"].ToString();
                //obj.scanBarcode = fc["scanBarcode"].ToString();
                //obj.barcodeNumber = fc["barcodeNumber"].ToString();

                //obj.Number = fc["Number"].ToString();
                //obj.Name = fc["Name"].ToString();
                //obj.paymentDate = fc["paymentDate"].ToString();
                //obj.Remarks = fc["Remarks"].ToString();

                //obj.ProductName = fc["ProductName"].ToString();
                //obj.ProductID = fc["ProductID"].ToString();
                //obj.PatientBillsDetailsMedicalID = Convert.ToInt32(fc["PatientBillsDetailsMedicalID"]);
                //obj.batchNumber = fc["batchNumber"].ToString();
                //obj.ProductDetailsID = fc["ProductDetailsID"].ToString();
                //obj.expiryDate = fc["expiryDate"].ToString();
                //obj.mrp = fc["mrp"].ToString();
                //obj.netRate = fc["netRate"].ToString();
                //obj.Prod_discountPercentage = fc["Prod_discountPercentage"].ToString();
                //obj.QtyU = fc["QtyU"].ToString();
                //obj.QtyL = fc["QtyL"].ToString();
                //obj.freeQuantity = fc["freeQuantity"].ToString();

                obj.tblProductName = fc["tblProductName"].ToString();
                obj.tblProductID = fc["tblProductID"].ToString();
                obj.tblbatchNumber = fc["tblbatchNumber"].ToString();
                obj.tblmrp = fc["tblmrp"].ToString();

                obj.tblexpiryDate = fc["tblexpiryDate"].ToString();
                obj.tblnetRate = fc["tblnetRate"].ToString();
                obj.tblQtyU = fc["tblQtyU"].ToString();
                obj.tblQtyL = fc["tblQtyL"].ToString();
                obj.tblGST = fc["tblGST"].ToString();
                obj.tbldiscountPercentage = fc["tbldiscountPercentage"].ToString();
                obj.tbltotalAmount = fc["tbltotalAmount"].ToString();
                obj.tblDiscountValue = fc["tblDiscountValue"].ToString();
                obj.tblfreeQuantity = fc["tblfreeQuantity"].ToString();

                int saleID = obj.MedicalBillID;

                if (BL_obj.Save(obj))
                {
                    if (saleID == 0)
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Medical Bill Saved Successfully !";

                        Session["medID"] = obj.MedicalBillID;
                        return RedirectToAction("medicalBill", "PharmacyReport");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Medical Bill Updated Successfully !";

                        Session["medID"] = obj.MedicalBillID;
                        return RedirectToAction("medicalBill", "PharmacyReport");
                    }

                }
                else
                {
                    ModelState.Clear();
                    TempData["msg"] = "Medical Bill Not Saved !";
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("PatientMedicalBill", "PatientMedicalBill");
            }
            return RedirectToAction("PatientMedicalBill", "PatientMedicalBill");
        }

        public ActionResult FillOldBillDetails(int BillNo)
        {
            List<PatientMedicalBill> obj1 = new List<Models.Pharmacy.PatientMedicalBill>();
            DataSet Med_Bill = new DataSet();
            DataSet Med_Bill_Detail = new DataSet();

            try
            {

                Med_Bill = BL_obj.GetPatientBillsMedical(BillNo);
                Med_Bill_Detail = BL_obj.GetPatientBillsDetailsMedical(BillNo);

                foreach (DataRow dr in Med_Bill_Detail.Tables[0].Rows)
                {
                    obj1.Add(
                        new Models.Pharmacy.PatientMedicalBill
                        {
                            PatientBillsDetailsMedicalID = Convert.ToInt32(dr["PatientBillsDetailsMedicalID"]),
                            MedicalBillID = Convert.ToInt32(dr["MedicalBillID"]),
                            BillNo = dr["BillNo"].ToString(),
                            tblProductID = dr["ProductID"].ToString(),
                            tblProductName = dr["ProductName"].ToString(),
                            //unit = dr["ProductUnitID"].ToString(),
                            ProductDetailsID = dr["ProductDetailID"].ToString(),
                            tblbatchNumber = dr["BatchNo"].ToString(),
                            tblexpiryDate = dr["ExpiryDate"].ToString(),
                            tblmrp = dr["MRP"].ToString(),
                            tblnetRate = dr["SalesRate"].ToString(),
                            tblQtyU = dr["Quantity"].ToString(),
                            tblQtyL = dr["QuantityL"].ToString(),
                            tblfreeQuantity = dr["FreeQuantity"].ToString(),
                            //billtype = dr["BillType"].ToString(),
                            tbldiscountPercentage = dr["DiscountPer"].ToString(),
                            tbltotalAmount = dr["TotalAmount"].ToString(),
                        });
                }

                if (Med_Bill.Tables[0].Rows.Count > 0)
                {
                    obj.MedicalBillID = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["MedicalBillID"]);
                    obj.BillNo = Med_Bill.Tables[0].Rows[0]["BillNo"].ToString();
                    obj.BillNoDate = Med_Bill.Tables[0].Rows[0]["BillDate"].ToString();
                    obj.PatientRegNO = Med_Bill.Tables[0].Rows[0]["PatientRegNo"].ToString();
                    obj.PatientName = Med_Bill.Tables[0].Rows[0]["PatientName"].ToString();
                    obj.Address = Med_Bill.Tables[0].Rows[0]["Address"].ToString();
                    obj.message = Med_Bill.Tables[0].Rows[0]["Message"].ToString();
                    //obj.path = Med_Bill.Tables[0].Rows[0]["Path"].ToString();
                    obj.DoctorID = Med_Bill.Tables[0].Rows[0]["DoctorID"].ToString();
                    obj.DoctorPrintName = Med_Bill.Tables[0].Rows[0]["DoctorName"].ToString();
                    //obj.billtype = Med_Bill.Tables[0].Rows[0]["BillType"].ToString();
                    obj.SalesPersonID = Med_Bill.Tables[0].Rows[0]["SalesPersonID"].ToString();
                    obj.SalesPersonName = Med_Bill.Tables[0].Rows[0]["SalesPersonName"].ToString();
                    obj.paymentType = Med_Bill.Tables[0].Rows[0]["PaymentType"].ToString();
                    obj.Number = Med_Bill.Tables[0].Rows[0]["Number"].ToString();
                    obj.Name = Med_Bill.Tables[0].Rows[0]["Name"].ToString();
                    obj.paymentDate = Med_Bill.Tables[0].Rows[0]["ChequeDate"].ToString();

                    //obj.other = Med_Bill.Tables[0].Rows[0]["Other"].ToString();
                    obj.Remarks = Med_Bill.Tables[0].Rows[0]["Remarks"].ToString();
                    obj.grossTotal = Med_Bill.Tables[0].Rows[0]["GrossAmount"].ToString();
                    obj.discountAmount = Med_Bill.Tables[0].Rows[0]["DiscountAmount"].ToString();
                    obj.vatAmount = Med_Bill.Tables[0].Rows[0]["TaxAmount"].ToString();


                    obj.salesReturn = Med_Bill.Tables[0].Rows[0]["SalesReturn"].ToString();
                    obj.totalAmount = Med_Bill.Tables[0].Rows[0]["TotalAmount"].ToString();
                    obj.discountPercentage = Med_Bill.Tables[0].Rows[0]["DiscountPercent"].ToString();
                    obj.otheLess = Med_Bill.Tables[0].Rows[0]["OtherLess"].ToString();
                    obj.netAmount = Med_Bill.Tables[0].Rows[0]["NetAmount"].ToString();
                    obj.cashRecipt = Med_Bill.Tables[0].Rows[0]["CashReceived"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { obj1 = obj1, objmodel = obj }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete_medBill(int MedicalBillID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalBill objdb = new BL_PatientMedicalBill();
                PatientMedicalBill objSG = new Models.Pharmacy.PatientMedicalBill();

                int DependaincyName = objdb.DeleteBill(MedicalBillID);

                if (DependaincyName > 0)
                {
                    _Del = "Medical Bill Deleted Successfully !";
                }
                else
                {
                    _Del = "Medical Bill can not be Deleted !";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Bind_NewTable()
        {            
            return new JsonResult { Data = BL_obj.Bind_NewTable(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Rebind_Data(int medID)
        {
            return new JsonResult { Data = BL_obj.Rebind_Data(medID), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Bind_oldPrint(string patnm)
        {
            DataSet ds = BL_obj.BindRegno(patnm);
           
            return new JsonResult { Data = BL_obj.Bind_oldPrint(Convert.ToInt16(ds.Tables[0].Rows[0]["PatientRegNO"].ToString())), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult Bind_oldPrint_regno(int regno)
        {            
            return new JsonResult { Data = BL_obj.Bind_oldPrint(Convert.ToInt16(regno)), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult OldBillPrint_click(int BillNo)
        {
            Session["medID"] = BillNo;

            return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}