using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using System.Web.Mvc;
using KeystoneProject.Models.Master;
using KeystoneProject.Buisness_Logic.Hospital;
using KeystoneProject.Models.Keystone;
using System.Data;
using System.IO;

namespace KeystoneProject.Controllers.Hospital
{
    public class StateController : Controller
    {
        //
        // GET: /State/

        BL_State obj_State = new BL_State();
        public ActionResult State()
        {
            return View();
        }
        [HttpPost]
      
        public JsonResult ShowState()
        {
            BL_State db = new BL_State();

            return new JsonResult {Data = db.SelectAllData(), JsonRequestBehavior= JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult State(State obj)
        {
             try
            {
                if (obj_State.CheckState(obj.StateID, obj.StateName))
                {
                    if (obj_State.Save(obj))
                    {
                        if (obj.StateID > 0)
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "State Updated Successfully !";
                            return RedirectToAction("State", "State");

                        }
                        else
                        {
                            ModelState.Clear();
                            TempData["Msg"] = "State Saved Successfully !";
                            return RedirectToAction("State", "State");

                        }

                    }
                }
                else
                {
                    TempData["Msg"] = "State Already Exists !";
                    return RedirectToAction("State", "State");
                }

            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;
                return RedirectToAction("State", "State");
            }
             return RedirectToAction("State", "State");
            //try
            //{
            //    if (obj_State.CheckState(obj.StateID, obj.StateName))
            //    {  
            //    string A = Request.Form["StateName"];
            //    TempData["Msg"] = "";
            //    if (Request.Form["StateName"] != null)
            //    {
                    
            //        if (obj.StateID > 0)
            //        {
                       
            //            if (obj_State.Save(obj))
            //            {
            //                obj.StateName = "";
            //                TempData["Msg"] = "State Update Successfully";
            //                ModelState.Clear();
            //                RedirectToAction("State", "State");
            //            }


            //            RedirectToAction("State", "State");
            //        }

            //        else
            //        {
            //            try
            //            {
                            
            //                if (obj_State.Save(obj))
            //                {
            //                    if (obj.StateID > 0)
            //                    {
            //                        obj.StateName = "";
            //                        TempData["Msg"] = "State Update Successfully";
            //                        ModelState.Clear();
            //                        RedirectToAction("State", "State");
            //                    }
            //                    TempData["Msg"] = "State Save Successfully";
            //                    RedirectToAction("State", "State");
            //                }
                
            //            }

            //            catch (Exception)
            //            {

            //            }
            //        }
                    
            //    }
            //    }
            //    else
            //    {
            //        //TempData["Msg"] = "Error";
            //        TempData["Msg"] = "State Already Exist's";
            //        return RedirectToAction("State", "State");

            //    }
            //}
                
        
        }

        public JsonResult GetCountryRecord(string prefix)
        {
            DataSet dsCountry = new DataSet();
            dsCountry = obj_State.GetCountry(prefix);
            List<State> search = new List<State>();
            foreach (DataRow dr in dsCountry.Tables[0].Rows)
            {
                search.Add(new State
                {
                    CountryID = Convert.ToInt32(dr["CountryID"]),
                    CountryName = dr["CountryName"].ToString(),
                  
                });
            }
            return new JsonResult { Data = search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DatatableBind(string prefix)
        {
            State Obj_Model = new Models.Keystone.State();
            Obj_Model.StoreAllState = obj_State.SelectAllData();
            List<State> serch = new List<State>();
            foreach (DataRow dr in Obj_Model.StoreAllState.Tables[0].Rows)
            {
                serch.Add(new State
                {
                    StateID = Convert.ToInt32(dr["StateID"]),
                    StateName = dr["StateName"].ToString(),
                    ReferenceCode = dr["ReferenceCode"].ToString(),
                    CountryName = dr["CountryName"].ToString(),
                    CountryID = Convert.ToInt32(dr["CountryID"]),
                   
                });
            }
            return new JsonResult { Data = serch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Fill(int StateID)
        {
            DataSet ds = new DataSet();
            List<State> Search = new List<State>();
            ds = obj_State.GetState(StateID);
            State AddState = new State();
            AddState.StateID = Convert.ToInt32(ds.Tables[0].Rows[0]["StateID"].ToString());
            AddState.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
            AddState.ReferenceCode = ds.Tables[0].Rows[0]["ReferenceCode"].ToString();


            if (ds.Tables[0].Rows[0]["CountryID"].ToString() == "0" || ds.Tables[0].Rows[0]["CountryName"].ToString() == "")
            {
                AddState.CountryName = "";
            }
            else
            {
                int counID = Convert.ToInt32(ds.Tables[0].Rows[0]["CountryID"].ToString());
                AddState.CountryID = counID;
                AddState.CountryName = ds.Tables[0].Rows[0]["CountryName"].ToString();
            }

            AddState.Mode = "Edit";
            Search.Add(AddState);

            return new JsonResult { Data = Search, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteState(int StateID)
        {
            string data = "";
            try
            {
                KeystoneProject.Buisness_Logic.Hospital.BL_State bl_state = new Buisness_Logic.Hospital.BL_State();
                int a = bl_state.DeleteState(StateID);
                if (a == 1)
                {
                    data = "State Deleted Successfully !";
                }
            }
            catch (Exception ex)
            {
                data = ex.Message;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        
        }
	}
}