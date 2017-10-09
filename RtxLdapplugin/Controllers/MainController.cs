using CommonHelper;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace RtxLdapplugin.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string path = Server.MapPath("~/ADConfig.xml");
            RTX rtx = new RTX();
            rtx.FilePath = path;
            string reg= rtx.RegApp();
            string start= rtx.StartApp();
            return View((object)(path+":"+reg+":"+start));
        }
        public ActionResult Delete(string userName)
        {
            RtxUserManager rum = new RtxUserManager();
            if (rum.RemoveUser(userName))
            {
                return Json(new AjaxResult { Status = "ok", Msg = "删除成功"});
            }
            else
            {
                return Json(new AjaxResult { Status = "error", Msg = "删除失败"});
            }            
        }
        public ActionResult DelDept(string deptName)
        {
            RtxDeptManager rdm = new RtxDeptManager();
            if (rdm.DelDept(deptName))
            {
                return Json(new AjaxResult { Status = "ok", Msg = "删除成功"});
            }
            else
            {
                return Json(new AjaxResult { Status = "error", Msg = "删除失败" });
            }
        }

        public ActionResult EditRtx(string deptName)
        {
            RtxDeptManager dept = new RtxDeptManager();
            dept.RemoveDept(deptName);
            return Json(new AjaxResult { Status = "ok", Msg = "success" });
        }

        public ActionResult Edit(string comName)
        {
            string filePath = Server.MapPath("~/ADConfig.xml");
            AdOperate ado = new AdOperate(filePath);
            ado.SetADConfig(comName, filePath);
            return Content("ok");
        }

        [HttpPost]
        public ActionResult SyncRtx(string comName)
        {
            string filePath= Server.MapPath("~/ADConfig.xml");
            AdOperate ado = new AdOperate(filePath);
            //ado.GetADConfig(filePath,comName);
            DirectoryEntry entry = ado.GetEntry();
            RtxDeptManager dept = new RtxDeptManager();
            RtxUserManager user = new RtxUserManager();
            string filter = "objectclass=organizationalUnit";
            ado.OUEntrySyncRtx(entry, filter, dept);
            filter = "(&(objectCategory=person)(objectClass=user))";
            ado.UserEntrySyncRtx(entry, filter, user, dept);
            return Json(new AjaxResult { Status = "ok", Msg = "success"});
        }
    }
}