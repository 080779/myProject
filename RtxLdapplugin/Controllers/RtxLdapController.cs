using CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RtxLdapplugin.Controllers
{
    public class RtxLdapController : Controller
    {
        public ActionResult AddUser(string userName)
        {
            RtxUserManager rum = new RtxUserManager();
            if (rum.AddRtxUser(userName, 1))
            {
                return Json(new AjaxResult { Status = "ok", Msg = "添加成功", Data = userName });
            }
            return Json(new AjaxResult { Status = "error", Msg = "添加失败" });
        }

        public ActionResult AddDept(string deptName)
        {
            RtxUserManager rum = new RtxUserManager();
            if (rum.AddRtxUser(deptName, 1))
            {
                return Json(new AjaxResult { Status = "ok", Msg = "添加成功", Data = deptName });
            }
            return Json(new AjaxResult { Status = "error", Msg = "添加失败" });
        }

        public ActionResult Del(string userName)
        {
            RtxUserManager rum = new RtxUserManager();
            if (rum.RemoveUser(userName))
            {
                return Json(new AjaxResult { Status = "ok", Msg = "删除成功", Data = userName });
            }
            return Json(new AjaxResult { Status = "error", Msg = "删除失败" });
        }

        public ActionResult DelDept(string deptName)
        {
            RtxDeptManager rdm = new RtxDeptManager();
            if(rdm.DelDept(deptName))
            {
                return Json(new AjaxResult { Status = "ok", Msg = "删除成功", Data = deptName });
            }
            return Json(new AjaxResult { Status = "error", Msg = "删除失败" });
        }
    }
}