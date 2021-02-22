using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.FinancialAccount;
using KeystoneProject.Buisness_Logic.FinancialAccount;
using System.Data.SqlClient;
using System.Data;


namespace KeystoneProject.Controllers.FinancialAccount
{
	public class VoucherEntryController : Controller
	{
		//
		// GET: /VoucherEntry/

		BL_VoucherEntry objbl = new BL_VoucherEntry();
		VoucherEntry objmodel = new VoucherEntry();
		DataSet dsBindCheque = new DataSet();
	   
		int HospitalID;
		int LocationID;
		int CreationID;

		public void HospitlLocationID()
		{
			HospitalID = Convert.ToInt32(Session["HospitalID"]);
			LocationID = Convert.ToInt32(Session["LocationID"]);
			CreationID = Convert.ToInt32(Session["CreationID"]);
		}

		[HttpGet]
		public ActionResult VoucherEntry()
		{
		   
			return View();
		}
		

		[HttpPost]
		public ActionResult VoucherEntry(VoucherEntry objmodels)
		{

			objmodels.AccountsID = Request.Form["account_nameId"];
			objmodels.VoucharID = Request.Form["voucher_nameId"];
			objmodels.VoucharName = Request.Form["voucher_name"];
			objmodels.RefVoucharNo = Request.Form["RefVoucharNo"];
            objmodels.CurrentDate = Convert.ToDateTime(Request.Form["current_date"]).ToString("yyyy-MM-dd");
            objmodels.VoucharDate = Convert.ToDateTime(Request.Form["voucher_date"]).ToString("yyyy-MM-dd");
			objmodels.VoucharAccountName = Request.Form["account_name"];
			objmodels.VoucharDrAmount = Request.Form["dr_amount"];
			objmodels.VoucharCrAmount = Request.Form["cr_amount"];

			objmodels.AccountName = Request.Form["AccountName"];
			objmodels.AccountsIDTable = Request.Form["account_name_otherId"];
			objmodels.DrAmount = Request.Form["Dramt"];
			objmodels.CrAmount = Request.Form["Cramt"];
			objmodels.ChequeNo = Request.Form["Cheq"];
			objmodels.BankName = Request.Form["Bank"];
			objmodels.Date = Request.Form["date"];
			objmodels.Narration = Request.Form["Narr"];
			//objmodels.ChequeClearDate = Request.Form["chq_date"];
			objmodels.InvNo = Request.Form["inv_no"];
            try
            {

                if (objbl.Save(objmodels))
                {
                    objmodels.dsbank = objbl.GetAllBank();
                    ModelState.Clear();
                    TempData["msg"] = "Voucher Entry Save Successfully"; 
                }
                else
                {
                    TempData["msg"] = "Voucher Entry Not Save";
                }
            }
            catch (Exception)
            {
                
                throw;
            }
			
			return View();
		}
        public ActionResult PrintVoucherEntry(int VoucharEntryID)
        {
            objbl.RptVoucherEntry(VoucharEntryID);
           
           // return RedirectToAction("RptVoucherEntryPrint", "VoucherEntry");
            return new JsonResult { Data = "Print", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult RptVoucherEntryPrint()
        {
            
            return View();
        }

		public JsonResult BindVoucherName(string prefix)
		{
			DataSet ds = objbl.BindVoucherName(prefix);
			List<VoucherEntry> Serchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				Serchlist.Add(new VoucherEntry
				{
					VoucherTypeName = dr["VoucherTypeName"].ToString(),
					VoucherTypeID = dr["VoucherTypeID"].ToString(),
					//AccountName = dr["AccountName"].ToString(),
					Narration = dr["Narration"].ToString()
				});
			}
			return new JsonResult { Data = Serchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
		public JsonResult GetVoucherType(string VoucherTypeID)
		{
			DataSet ds = objbl.GetVoucherType(VoucherTypeID);
			List<VoucherEntry> searchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				searchlist.Add(new VoucherEntry
				{
					VoucherTypeID = dr["VoucherTypeID"].ToString(),
					VoucherTypeName = dr["VoucherTypeName"].ToString(),
					AccountsID = dr["AccountsID"].ToString(),
					DebitMasterAccount = dr["DebitMasterAccount"].ToString(),
					EditMasterAccount = dr["EditMasterAccount"].ToString()
				});
			}
			return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

	  
		public JsonResult getAccountsName(string prefix)
		{
			HospitlLocationID();
			objbl.Connect();
			DataSet ds = new DataSet();
			SqlCommand cmd = new SqlCommand("select AccountsID, AccountName from Accounts where HospitalID='" + HospitalID + "' and LocationID='" + LocationID + "' and  AccountsID =" + prefix + " and RowStatus=0", objbl.con);
			cmd.Parameters.AddWithValue("@prefix", prefix);
			objbl.con.Open();
			SqlDataAdapter ad = new SqlDataAdapter(cmd);
			ad.Fill(ds);
			objbl.con.Close();

			List<VoucherEntry> searchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				searchlist.Add(new VoucherEntry
				{
					AccountName = dr["AccountName"].ToString(),
					AccountsID = dr["AccountsID"].ToString()
				});
			}

			return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
		public JsonResult BindAccountName(string prefix)
		{
			DataSet ds = objbl.BindAccountName(prefix);
			List<VoucherEntry> Serchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				Serchlist.Add(new VoucherEntry
				{
					AccountName = dr["AccountName"].ToString(),
					AccountsID = dr["AccountsID"].ToString()

				});
			}
			return new JsonResult { Data = Serchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
		public JsonResult GetAllBank()
		{
			DataSet ds = objbl.GetAllBank();
			List<VoucherEntry> searchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				searchlist.Add(new VoucherEntry
				{
					BankId = dr["BankID"].ToString(),
					BankName = dr["BankName"].ToString()
				});
			}
			return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
		public JsonResult GetChequeBookByBankID(string BankId)
		{
			DataSet ds = objbl.GetChequeBookByBankID(BankId);
			List<VoucherEntry> searchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				searchlist.Add(new VoucherEntry
				{
					ChequeBookID = dr["ChequeBookID"].ToString(),
					BookName = dr["BookName"].ToString()
				});
			}
			return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
		public JsonResult ChequeNo(string ChequeBookID)
		{
			DataSet ds = objbl.GetChequeLayoutForCheckPrint(ChequeBookID);
			int StartNo = 0;
			int EndNo = 0;
			StartNo = Convert.ToInt32(ds.Tables[1].Rows[0]["ChequeSNo"].ToString());
			EndNo = Convert.ToInt32(ds.Tables[1].Rows[0]["ChequeENo"].ToString());
			int Total = EndNo - StartNo;
			dsBindCheque.Reset();
			dsBindCheque.Tables.Add(new DataTable());
			dsBindCheque.Tables[0].Columns.Add("ID");
			dsBindCheque.Tables[0].Columns.Add("ChequeNo");

			int SchequeNo = 0;

			for (int i = 0; Total >= i; i++)
			{
				DataRow dr = dsBindCheque.Tables[0].NewRow();
				DataView dv1 = new DataView(ds.Tables[2], " ChequeNo = '" + StartNo + "' ", "", DataViewRowState.CurrentRows);
				if (dv1.Count > 0)
				{
					if (Convert.ToInt32(dv1[0]["ChequeNo"].ToString()) == StartNo && Convert.ToString(dv1[0]["ChequeStatus"].ToString()) == "Active")
					{
						dr["ChequeNo"] = StartNo.ToString() + " --- P";
						dr["ID"] = StartNo;
					}
					else if (Convert.ToInt32(dv1[0]["ChequeNo"].ToString()) == StartNo && Convert.ToString(dv1[0]["ChequeStatus"].ToString()) == "Cancel")
					{
						dr["ChequeNo"] = StartNo.ToString() + " --- C";
						dr["ID"] = StartNo;
					}
					else
					{
						dr["ChequeNo"] = StartNo;
						dr["ID"] = StartNo;
					}
				}
				else
				{
					if (SchequeNo == 0)
					{
						SchequeNo = StartNo;
					}
					dr["ChequeNo"] = StartNo;
					dr["ID"] = StartNo;

				}
				dsBindCheque.Tables[0].Rows.Add(dr);

				StartNo++;
			}
			List<VoucherEntry> searchlist = new List<VoucherEntry>();
			foreach (DataRow dr in dsBindCheque.Tables[0].Rows)
			{
				searchlist.Add(new VoucherEntry
				{
					Id = dr["ID"].ToString(),
					ChequeNo1 = dr["ChequeNo"].ToString(),
					SchequeNo = (SchequeNo).ToString()
				});
			}
			return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		public void BindChequeNo(DataSet ds)
		{
			int StartNo = 0;
			int EndNo = 0;
			StartNo = Convert.ToInt32(ds.Tables[1].Rows[0]["ChequeSNo"].ToString());
			EndNo = Convert.ToInt32(ds.Tables[1].Rows[0]["ChequeENo"].ToString());
			int Total = EndNo - StartNo;
			dsBindCheque.Reset();
			dsBindCheque.Tables.Add(new DataTable());
			dsBindCheque.Tables[0].Columns.Add("ID");
			dsBindCheque.Tables[0].Columns.Add("ChequeNo");

			int SchequeNo = 0;

			for (int i = 0; Total >= i; i++)
			{
				DataRow dr = dsBindCheque.Tables[0].NewRow();
				DataView dv1 = new DataView(ds.Tables[2], " ChequeNo = '" + StartNo + "' ", "", DataViewRowState.CurrentRows);
				if (dv1.Count > 0)
				{
					if (Convert.ToInt32(dv1[0]["ChequeNo"].ToString()) == StartNo && Convert.ToString(dv1[0]["ChequeStatus"].ToString()) == "Active")
					{
						dr["ChequeNo"] = StartNo.ToString() + " --- P";
						dr["ID"] = StartNo;
					}
					else if (Convert.ToInt32(dv1[0]["ChequeNo"].ToString()) == StartNo && Convert.ToString(dv1[0]["ChequeStatus"].ToString()) == "Cancel")
					{
						dr["ChequeNo"] = StartNo.ToString() + " --- C";
						dr["ID"] = StartNo;
					}
					else
					{
						dr["ChequeNo"] = StartNo;
						dr["ID"] = StartNo;
					}
				}
				else
				{
					if (SchequeNo == 0)
					{
						SchequeNo = StartNo;
					}
					dr["ChequeNo"] = StartNo;
					dr["ID"] = StartNo;

				}
				dsBindCheque.Tables[0].Rows.Add(dr);

				StartNo++;
			}
		}
		public String AutoGanratReferecnCode()
		{
			string Reference = "";
			Reference += "AL";
			Reference += (DateTime.Now.Year.ToString()).Substring(2, 2);
			Reference += DateTime.Now.Month.ToString();
			Reference += DateTime.Now.Day.ToString();
			Reference += DateTime.Now.Hour.ToString();
			Reference += DateTime.Now.Minute.ToString();
			Reference += DateTime.Now.Second.ToString();
			Reference += DateTime.Now.Millisecond.ToString();
			return Reference;
		}

		public ActionResult ShowAllVoucherEntry()
		{
			BL_VoucherEntry db = new BL_VoucherEntry();

			return new JsonResult { Data = db.SelectAllVoucherEntry(), JsonRequestBehavior = JsonRequestBehavior.AllowGet};
		}

		public ActionResult FillVoucherEntry(string VoucharEntryID)
		{
			DataSet ds = objbl.GetVoucharEntryDetail(VoucharEntryID);
			List<VoucherEntry> searchlist = new List<VoucherEntry>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				searchlist.Add(new VoucherEntry
				{
					VoucharEntryDetailID = dr["VoucharEntryDetailID"].ToString(),
					VoucharEntryID = dr["VoucharEntryID"].ToString(),
					VoucharName = dr["VoucharName"].ToString(),
					AccountsIDTable = dr["AccountsID"].ToString(),
					AccountName = dr["AccountName"].ToString(),
					DrAmount = dr["DrAmount"].ToString(),
					CrAmount = dr["CrAmount"].ToString(),
					ChequeNo = dr["ChequeNo"].ToString(),
					Name = dr["Name"].ToString(),
					Date = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd"),
					Narration = dr["Narration"].ToString(),
					AccountsID = dr["AccountsID"].ToString(),
					
				});
			}
			return new JsonResult { Data = searchlist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };           
		}

		public ActionResult DeleteVoucher(int VoucharEntryID)
		{

			string del = "";
			VoucharEntryID = Convert.ToInt32(Request.Form["VoucharEntryID"]);
			BL_VoucherEntry questionhead1 = new BL_VoucherEntry();

			if (questionhead1.DeleteVoucherEntry(VoucharEntryID))
			{
				del = "Delete";
			}

			return Json(del, JsonRequestBehavior.AllowGet);
		} 


	}
}