using CommonHelper;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RtxLdapplugin.Controllers
{
    public class DomainRtxController : Controller
    {
        public ActionResult AddUser(DomainUser user)
        {
            string filePath = Server.MapPath("~/ADConfig.xml");
            //AdOperate ado = new AdOperate();
            //ado.GetADConfig(filePath, user.ComName);
            AdOperate ado = new AdOperate(filePath);
            string domainPath = "LDAP://192.168.31.134/OU=南宁公司,DC=test,DC=com";
            string adminUser = "Administrator";
            string password = "Abc123456";           
            DirectoryEntry entry= ado.GetEntry(domainPath, adminUser,password);
            string filter = "(&(objectclass=organizationalUnit)(ou="+user.Department+"))";
            DirectoryEntry ouEntry= ado.GetOUEntry(entry, filter);
            if(ouEntry==null)
            {
                return Json(new AjaxResult { Status = "error", Msg = "nonDept" });
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            string data = js.Serialize(user);
            if(ado.IsADUserExist(entry,user.Name))
            {
                return Json(new AjaxResult { Status = "error", Msg = "用户已经存在" });
            }
            //return Json(new AjaxResult { Status = "error", Msg = "userdata", Data = data });
            if (!ado.AddAccount(ouEntry, user))
            {
                return Json(new AjaxResult { Status = "error", Msg = "添加用户到域失败", Data = data });
            }
            RtxManager rm = new RtxManager();
            string[] paths = ouEntry.Path.Replace("LDAP://192.168.31.134/","").Replace(",DC=test,DC=com","").Replace("OU=","").Split(',');
            string path = "";
            for (int i=paths.Length-1;i>=0;i--)
            {
                path = path + paths[i] + @"\";
            }            

            if(!rm.AddEditRtxUser(user, path, 1))
            {
                ado.GetUserEntry(entry,user.Name).DeleteTree();
                return Json(new AjaxResult { Status = "error", Msg = "rtx添加用户失败" });
            }
            return Json(new AjaxResult { Status = "ok", Msg = "success",Data=path });
        }

        public ActionResult AddOU(DomainUser user)
        {
            string filePath = Server.MapPath("~/ADConfig.xml");
            AdOperate ado = new AdOperate(filePath);
            DirectoryEntry entry = ado.GetEntry();
            string filter = "(&(objectclass=organizationalUnit)(ou=行政部))";
            DirectoryEntry ouEntry = ado.GetOUEntry(entry, filter);
            //DirectoryEntry userEntry = ado.GetUserEntry(entry,"yilidan");
            //JavaScriptSerializer js = new JavaScriptSerializer();
            return Json(new AjaxResult { Status="error",Msg="想不通",Data=entry.Path+":::"+ouEntry.Path+":::"+user.Name});
        }
        public ActionResult EditUser(DomainUser user)
        {
            string filePath = Server.MapPath("~/ADConfig.xml");
            AdOperate ado = new AdOperate(filePath);
            //ado.SetADConfig(user.ComName, filePath);
            DirectoryEntry entry = ado.GetEntry();
            string filter = "(&(objectclass=organizationalUnit)(ou=" + user.Department + "))";
            DirectoryEntry ouEntry = ado.GetOUEntry(entry, filter);
            if (ouEntry == null)
            {
                return Json(new AjaxResult { Status = "error", Msg = "部门不存在" });
            }
            DirectoryEntry userEntry = ado.GetUserEntry(entry,user.Name);
            if (userEntry==null)
            {
                return Json(new AjaxResult { Status = "error", Msg = "用户不存在" });
            }
            if(!ado.EditAccount(userEntry, user))
            {
                return Json(new AjaxResult { Status = "error", Msg = "编辑用户到域失败" });
            }
            RtxManager rm = new RtxManager();
            string[] paths = ouEntry.Path.Replace("LDAP://192.168.31.134/", "").Replace(",DC=test,DC=com", "").Replace("OU=", "").Split(',');
            string path = "";
            for (int i = paths.Length - 1; i >= 0; i--)
            {
                path = path + paths[i] + @"\";
            }

            if (!rm.AddEditRtxUser(user, path, 1))
            {
                ado.GetUserEntry(entry,user.Name).DeleteTree();
                return Json(new AjaxResult { Status = "error", Msg = "rtx编辑用户失败" });
            }
            return Json(new AjaxResult { Status = "ok", Msg = "rtx编辑用户成功"});
        }
        public ActionResult EditOU()
        {
            string filePath = Server.MapPath("~/ADConfig.xml");
            AdOperate ado = new AdOperate(filePath);
            DirectoryEntry entry = ado.GetEntry("LDAP://192.168.31.134/OU=南宁公司,DC=test,DC=com","Administrator","Abc123456");
            DirectoryEntry ouEntry = ado.GetOUEntry(entry, "(&(objectclass=organizationalUnit)(ou=php开发))");
            DomainUser user = new DomainUser();
            user.Name = "anna";
            user.DisplayName = "安娜";
            user.TelephoneNumber = "18618618686";
            user.UserPwd = "Asd123456";
            user.Mail = "afs@qq.com.cn";

            try
            {
                DirectoryEntry NewUser = ouEntry.Children.Add("CN=" + user.Name, "user");
                NewUser.Properties["sAMAccountName"].Add(user.Name); //account
                NewUser.Properties["userPrincipalName"].Value = user.Name + "@test.com"; //user logon name,xxx@bdxy.com
                if (!string.IsNullOrEmpty(user.Company))
                {
                    NewUser.Properties["company"].Value = user.Company;
                }
                if (!string.IsNullOrEmpty(user.Department))
                {
                    NewUser.Properties["department"].Value = user.Department;
                }
                if (!string.IsNullOrEmpty(user.Description))
                {
                    NewUser.Properties["description"].Value = user.Description;
                }
                if (!string.IsNullOrEmpty(user.DisplayName))
                {
                    NewUser.Properties["displayName"].Value = user.DisplayName;
                }
                if (!string.IsNullOrEmpty(user.GivenName))
                {
                    NewUser.Properties["givenName"].Value = user.GivenName;
                }
                if (!string.IsNullOrEmpty(user.Initials))
                {
                    NewUser.Properties["initials"].Value = user.Initials;
                }
                if (!string.IsNullOrEmpty(user.Mail))
                {
                    NewUser.Properties["mail"].Value = user.Mail;
                }
                if (!string.IsNullOrEmpty(user.Name))
                {
                    NewUser.Properties["name"].Value = user.Name;
                }
                if (!string.IsNullOrEmpty(user.PhysicalDeliveryOfficeName))
                {
                    NewUser.Properties["physicalDeliveryOfficeName"].Value = user.PhysicalDeliveryOfficeName;
                }
                if (!string.IsNullOrEmpty(user.SN))
                {
                    NewUser.Properties["sn"].Value = user.SN;
                }
                if (!string.IsNullOrEmpty(user.TelephoneNumber))
                {
                    NewUser.Properties["telephoneNumber"].Value = user.TelephoneNumber;
                }
                NewUser.Properties["initials"].Value = user.Gender;
                NewUser.CommitChanges();
                //设置密码
                //反射调用修改密码的方法（注意端口号的问题  端口号会引起方法调用异常）

                NewUser.Invoke("SetPassword", new object[] { user.UserPwd });
                NewUser.Properties["userAccountControl"].Value = 0x200;
                //默认设置新增账户启用          
                NewUser.CommitChanges();
                return Json(new AjaxResult { Status = "ok", Msg = "添加成功" });
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { Status = "error", Msg = "添加失败"+ex.ToString() });
            }
        }
        public ActionResult Check()
        {
            return View();
        }
        public ActionResult CheckOU()
        {
            return View();
        }
        public ActionResult Del()
        {
            return View();
        }
        public ActionResult DelOU()
        {
            return View();
        }
    }
}