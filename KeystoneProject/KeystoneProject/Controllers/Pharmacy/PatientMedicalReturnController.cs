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
    public class PatientMedicalReturnController : Controller
    {
        int HospitalID = 0;
        int LocationID = 0;
        int UserID = 0;

        BL_PatientMedicalReturn BL_obj = new BL_PatientMedicalReturn();
        PatientMedicalReturn obj = new Models.Pharmacy.PatientMedicalReturn();
        private SqlConnection con;

        private void Connect()
        {
            HospitalID = Convert.ToInt32(Session["HospitalID"]);
            LocationID = Convert.ToInt32(Session["LocationID"]);
            UserID = Convert.ToInt32(Session["UserID"]);
            string constring = ConfigurationManager.ConnectionStrings["Mycon"].ToString();
            con = new SqlConnection(constring);
        }
        public ActionResult PatientMedicalReturn()
        {
            return View();
        }

        public ActionResult BindProductName(string Name)
        {
            DataSet ds = BL_obj.BindProductName(Name);
            List<PatientMedicalReturn> obj = new List<Models.Pharmacy.PatientMedicalReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalReturn
                {
                    productID = Convert.ToInt32(dr["ProductID"]),
                    productName = dr["ProductName"].ToString(),
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = 86753090 };

            //  return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindCustomer(string Name)
        {
            DataSet ds = BL_obj.BindCustomer(Name);
            List<PatientMedicalReturn> obj = new List<Models.Pharmacy.PatientMedicalReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalReturn
                {
                    customerID = Convert.ToInt32(dr["CustomerID"]),
                    customerName = dr["CustomerName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindSalesPerson(string Name)
        {
            DataSet ds = BL_obj.BindSalesPerson(Name);
            List<PatientMedicalReturn> obj = new List<Models.Pharmacy.PatientMedicalReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalReturn
                {
                    salesPersonID = Convert.ToInt32(dr["SalesPersonID"]),
                    salesPerson = dr["SalesPersonName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindDoctor(string Name)
        {
            DataSet ds = BL_obj.BindDoctor(Name);
            List<PatientMedicalReturn> obj = new List<Models.Pharmacy.PatientMedicalReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalReturn
                {
                    doctorID = Convert.ToInt32(dr["DoctorID"]),
                    doctorName = dr["DoctorPrintName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult BindPatient(string Name)
        {
            DataSet ds = BL_obj.BindPatient(Name);
            List<PatientMedicalReturn> obj = new List<Models.Pharmacy.PatientMedicalReturn>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.Add(new Models.Pharmacy.PatientMedicalReturn
                {
                    patientRegNo = Convert.ToInt32(dr["PatientRegNO"]),
                    patientName = dr["PatientName"].ToString()
                });
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult AjaxMethod_PatName(string patnm)
        {
            try
            {

                List<PatientMedicalReturn> searchList = new List<PatientMedicalReturn>();

                DataSet ds = BL_obj.BindRegno(patnm);
                DataTable dt = new DataTable();

                dt = BL_obj.Bind_patDetail(Convert.ToInt32(ds.Tables[0].Rows[0]["PatientRegNO"]));

                foreach (DataRow dr in dt.Rows)
                {
                    searchList.Add(
                        new PatientMedicalReturn
                        {
                            patientName = Convert.ToString(dr["PatientName"]),
                            patientRegNo = Convert.ToInt32(dr["PatientRegNO"]),
                            customerName = Convert.ToString(dr["PatientName"]),
                            PatientType = Convert.ToString(dr["PatientType"]),
                            address = Convert.ToString(dr["Address"]),
                            doctorName = Convert.ToString(dr["DoctorPrintName"]),
                        });
                }

                return new JsonResult
                {
                    Data = searchList,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public ActionResult AjaxMethod_OldBill(string patnm)
        {
            try
            {
                List<PatientMedicalReturn> searchList = new List<PatientMedicalReturn>();

                DataTable dt = new DataTable();
                DataSet ds = BL_obj.BindRegno(patnm);
                dt = BL_obj.GetOldBill(Convert.ToInt16(ds.Tables[0].Rows[0]["PatientRegNO"].ToString()));

                foreach (DataRow dr in dt.Rows)
                {
                    searchList.Add(
                        new PatientMedicalReturn
                        {
                            oldBillNumber = Convert.ToString(dr["BillNo&Date"]),
                        });
                }

                return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public ActionResult AjaxMethod_regno(int regno)
        {
            List<PatientMedicalReturn> searchList = new List<PatientMedicalReturn>();

            DataTable dt = new DataTable();
            dt = BL_obj.Bind_patDetail(regno);

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalReturn
                    {
                        patientName = Convert.ToString(dr["PatientName"]),
                        patientRegNo = Convert.ToInt32(dr["PatientRegNO"]),
                        customerName = Convert.ToString(dr["PatientName"]),
                        PatientType = Convert.ToString(dr["PatientType"]),
                        address = Convert.ToString(dr["Address"]),
                        doctorName = Convert.ToString(dr["DoctorPrintName"]),
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
            List<PatientMedicalReturn> searchList = new List<PatientMedicalReturn>();

            DataTable dt = new DataTable();
            dt = BL_obj.GetOldBill(regno);

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalReturn
                    {
                        oldBillNumber = Convert.ToString(dr["BillNo&Date"]),
                    });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult AjaxMethod_CustName(string custNM)
        {
            List<PatientMedicalReturn> searchList = new List<PatientMedicalReturn>();

            DataSet ds = BL_obj.BindCustID(custNM);
            DataTable dt = new DataTable();

            dt = BL_obj.Bind_CustDetail(Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerID"]));

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalReturn
                    {
                        customerName = Convert.ToString(dr["CustomerName"]),
                        address = Convert.ToString(dr["Address"]),
                    });
            }
            return new JsonResult
            {
                Data = searchList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public ActionResult AjaxMethod_CustOldBill(string custNM)
        {
            List<PatientMedicalReturn> searchList = new List<PatientMedicalReturn>();

            DataTable dt = new DataTable();
            DataSet ds = BL_obj.BindCustID(custNM);
            dt = BL_obj.GetOldBill(Convert.ToInt16(ds.Tables[0].Rows[0]["CustomerID"].ToString()));

            foreach (DataRow dr in dt.Rows)
            {
                searchList.Add(
                    new PatientMedicalReturn
                    {
                        oldBillNumber = Convert.ToString(dr["BillNo"]),
                    });
            }

            return new JsonResult { Data = searchList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult AjaxMethod_batchno(string prodnm)
        {
            DataTable dt = new DataTable();
            DataSet ds1 = BL_obj.BindProductName(prodnm);

            int ProductID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ProductID"].ToString());

            DataSet ds = BL_obj.GetMedicalBillForProductStock(ProductID);
            DataSet ds2 = new DataSet();
            List<PatientMedicalReturn> obj1 = new List<Models.Pharmacy.PatientMedicalReturn>();

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
                SqlCommand cmd = new SqlCommand("select  ProductDetailsID ,BatchNo,ExpiryDate,SalesRate,CurrentStock ,CurrentStock1, PurchaseRate,MRP,SalesRate  from ProductDetails left join Product  on  Product .ProductID = ProductDetails.ProductID where Product.HospitalID = " + HospitalID + " and Product.LocationID = " + LocationID + " and  Convert(varchar(10),ExpiryDate,120) > (GETDATE()) and ProductDetails.ProductID =" + ProductID + "  and ProductDetails.RowStatus = 0 order by ExpiryDate asc", con);
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
                obj1.Add(new Models.Pharmacy.PatientMedicalReturn
                {
                    ProductDetailsID = dr["ProductDetailsID"].ToString(),
                    batchNumber = dr["BatchNo"].ToString(),
                    expiry = dr["ExpiryDate"].ToString(),
                    //ExparyDateColor = color,
                });
            }
            return new JsonResult { Data = obj1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult FillDataByProductID(int ProductDetailsID)
        {
            //obj.Gst = 0;
            //obj.DiscountValue = "";
            DataSet dsProductDetail = new DataSet();
            dsProductDetail = BL_obj.GetServicesDetailsforMedicalBills(ProductDetailsID);
            if (dsProductDetail.Tables[0].Rows.Count > 0)
            {
                //if (dsProductDetail.Tables[0].Rows[0]["Contain"].ToString().Trim() != "")
                //{
                //    obj.Contain = Convert.ToDecimal((dsProductDetail.Tables[0].Rows[0]["Contain"].ToString().Trim()));
                //}
                DataSet ds = new DataSet();

                //ds = BL_obj.GetGSTRatio();
                //if (dsProductDetail.Tables[0].Rows[0]["GST"].ToString() != "")
                //{

                //    obj.Gst = Convert.ToDecimal(dsProductDetail.Tables[0].Rows[0]["GST"].ToString());
                //}

                //long Gstper = (int)obj.Gst;

                //string valueGst = Gstper + "%".ToString();
                //DataView dv1 = new DataView(ds.Tables[0], " GSTTAX = '" + valueGst + "' ", "", DataViewRowState.CurrentRows);
                ///obj.DiscountValue = "0";
                //if (dv1.ToTable().Rows.Count > 0)
                //{
                //    obj.DiscountValue = dv1.ToTable().Rows[0]["GstDiscValue"].ToString();
                //}

                obj.netRate = Convert.ToInt32(dsProductDetail.Tables[0].Rows[0]["SalesRate"]);
                obj.QtyU = dsProductDetail.Tables[0].Rows[0]["Packing"].ToString();
                obj.QtyL = dsProductDetail.Tables[0].Rows[0]["ProductLowerUnitName"].ToString();
                obj.barCode = dsProductDetail.Tables[0].Rows[0]["BarCode"].ToString();

                obj.mrp = Convert.ToInt32(dsProductDetail.Tables[0].Rows[0]["MRP"]);
                if (Convert.ToDateTime(dsProductDetail.Tables[0].Rows[0]["ExpiryDate"].ToString()) < DateTime.Now)
                {
                    obj.expiry = dsProductDetail.Tables[0].Rows[0]["ExpiryDate"].ToString();
                    // ucPatientMedicalBill1.lblExpirydate.ForeColor = Color.Red;
                }
                else
                {
                    obj.expiry = dsProductDetail.Tables[0].Rows[0]["ExpiryDate"].ToString();
                    // ucPatientMedicalBill1.lblExpirydate.ForeColor = Color.Black;
                }
            }
            return new JsonResult { Data = obj, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult PatientMedicalReturn(PatientMedicalReturn obj, FormCollection fc)
        {
            try
            {
                obj.postingAccount = fc["postingAccount"].ToString();

                obj.tblProductName = fc["tblProductName"].ToString();
                obj.tblProductID = fc["tblProductID"].ToString();
                obj.tblbatchNumber = fc["tblbatchNumber"].ToString();
                obj.tblmrp = fc["tblmrp"].ToString();
                obj.tblexpiryDate = fc["tblexpiryDate"].ToString();
                obj.tblnetRate = fc["tblnetRate"].ToString();
                obj.tblQtyU = fc["tblQtyU"].ToString();
                obj.tblQtyL = fc["tblQtyL"].ToString();
                obj.tbldiscountPercentage = fc["tbldiscountPercentage"].ToString();
                obj.tbltotalAmount = fc["tbltotalAmount"].ToString();
                obj.tblfreeQuantity = fc["tblfreeQuantity"].ToString();

                int saleID = obj.medicalReturnID;

                if (BL_obj.Save(obj))
                {
                    if (saleID == 0)
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Medical Bill Return Saved Successfully !";

                        Session["medID"] = obj.medicalReturnID;
                        return RedirectToAction("medicalBillReturn", "PharmacyReport");
                    }
                    else
                    {
                        ModelState.Clear();
                        TempData["msg"] = "Medical Bill Return Updated Successfully !";

                        Session["medID"] = obj.medicalReturnID;
                        return RedirectToAction("medicalBillReturn", "PharmacyReport");
                    }

                }
                else
                {
                    ModelState.Clear();
                    TempData["msg"] = "Medical Bill Return Not Saved !";
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("PatientMedicalReturn", "PatientMedicalReturn");
            }
            return RedirectToAction("PatientMedicalReturn", "PatientMedicalReturn");
        }

        public ActionResult FillOldBillDetails(int BillNo)
        {
            List<PatientMedicalReturn> obj1 = new List<Models.Pharmacy.PatientMedicalReturn>();
           
            DataSet Med_Bill = new DataSet();
            DataSet Med_Bill_Detail = new DataSet();

            try
            {

                Med_Bill = BL_obj.GetPatientBillsMedical(BillNo);
                Med_Bill_Detail = BL_obj.GetPatientBillsDetailsMedical(BillNo);

                foreach (DataRow dr in Med_Bill_Detail.Tables[0].Rows)
                {
                    obj1.Add(
                        new Models.Pharmacy.PatientMedicalReturn
                        {
                            medicalReturnDetailID = Convert.ToInt32(dr["PatientBillsDetailsMedicalID"]),
                            medicalReturnID = Convert.ToInt32(dr["MedicalBillID"]),
                            //oldBillNumber = dr["BillNo"].ToString(),
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
                    obj.medicalReturnID = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["MedicalBillID"]);
                    obj.oldBillNumber = Med_Bill.Tables[0].Rows[0]["BillNo"].ToString();
                    obj.referenceDate = Med_Bill.Tables[0].Rows[0]["BillDate"].ToString();
                    obj.patientRegNo = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["PatientRegNo"]);
                    obj.patientName = Med_Bill.Tables[0].Rows[0]["PatientName"].ToString();
                    obj.address = Med_Bill.Tables[0].Rows[0]["Address"].ToString();
                    //obj.message = Med_Bill.Tables[0].Rows[0]["Message"].ToString();
                    //obj.path = Med_Bill.Tables[0].Rows[0]["Path"].ToString();
                    obj.doctorID = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["DoctorID"]);
                    obj.doctorName = Med_Bill.Tables[0].Rows[0]["DoctorName"].ToString();
                    //obj.billtype = Med_Bill.Tables[0].Rows[0]["BillType"].ToString();
                    obj.salesPersonID = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["SalesPersonID"]);
                    obj.salesPerson = Med_Bill.Tables[0].Rows[0]["SalesPersonName"].ToString();
                    //obj.paymentType = Med_Bill.Tables[0].Rows[0]["PaymentType"].ToString();
                    //obj.Number = Med_Bill.Tables[0].Rows[0]["Number"].ToString();
                    //obj.Name = Med_Bill.Tables[0].Rows[0]["Name"].ToString();
                    //obj.paymentDate = Med_Bill.Tables[0].Rows[0]["ChequeDate"].ToString();

                    //obj.other = Med_Bill.Tables[0].Rows[0]["Other"].ToString();
                    obj.remark = Med_Bill.Tables[0].Rows[0]["Remarks"].ToString();
                    obj.grossTotal = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["GrossAmount"]);
                    obj.discountAmount = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["DiscountAmount"]);
                    obj.vatAmount = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["TaxAmount"]);


                    // obj.salesReturn = Med_Bill.Tables[0].Rows[0]["SalesReturn"].ToString();
                    obj.totalAmount = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["TotalAmount"]);
                    obj.discountPercentage = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["DiscountPercent"]);
                    obj.otherLess = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["OtherLess"]);
                    obj.netAmount = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["NetAmount"]);

                    obj.postingAccount = Med_Bill.Tables[0].Rows[0]["PostingAccount"].ToString();
                    obj.referenceNumber = Convert.ToInt32(Med_Bill.Tables[0].Rows[0]["RefNo"]);
                    obj.referenceDate = Med_Bill.Tables[0].Rows[0]["RefDate"].ToString();

                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { obj1 = obj1, objmodel = obj }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete_medBill(int medicalReturnID)
        {
            string _Del = null;
            try
            {
                KeystoneProject.Buisness_Logic.Pharmacy.BL_PatientMedicalReturn objdb = new BL_PatientMedicalReturn();
                PatientMedicalReturn objSG = new Models.Pharmacy.PatientMedicalReturn();

                int DependaincyName = objdb.DeleteBill(medicalReturnID);

                if (DependaincyName > 0)
                {
                    _Del = "Medical Return Bill Deleted Successfully !";
                }
                else
                {
                    _Del = "Medical Return Bill can not be Deleted !";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult { Data = _Del, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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