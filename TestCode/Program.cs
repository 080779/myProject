using CommonHelper;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestCode
{
    class Program
    {
        static void Main8(string[] args)
        {
            string url= "http://192.168.31.134:8083/domainrtx/EditOU";
            AjaxResult res = Helper.SendMessage(url);
            Console.WriteLine(res.Status + res.Msg + res.Data);
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            string url = "http://192.168.31.134:8083/domainrtx/addou";
            DomainUser user = new DomainUser();
            user.Name = "tdkdsi";
            user.DisplayName = "泰凯斯";
            user.Department = "测试部";
            user.ComName = "南宁公司";
            user.TelephoneNumber = "15615615656";
            user.Mail = "aiii10120@qq.com";
            user.Gender = 0;
            user.UserPwd = "Asd123456";
            AjaxResult res = Helper.SendMessage(url, user);
            Console.WriteLine(res.Status + res.Msg + res.Data);
            //AdOperate ado = new AdOperate();
            //string filePath = @" C:\1\RtxLdap\TestCode\ADConfig.xml";
            //ado.GetADConfig(filePath, "南宁公司");
            //DirectoryEntry ouEntry = ado.GetOUEntry(ado.GetEntry(), "(&(objectclass=organizationalUnit)(ou=php开发))");
            //Console.WriteLine(ado.AddAccount(ouEntry, user));
            //string[] paths = path.Replace("LDAP://192.168.31.134/", "").Replace(",DC=test,DC=com", "").Replace("OU=", "").Split(',');
            //path = "";
            //for (int i = paths.Length - 1; i >= 0; i--)
            //{
            //    path = path + paths[i] + "/";
            //}
            //Console.WriteLine(path);
            Console.ReadKey();
        }
        static void Main7(string[] args)
        {
            //string filePath =@" F:\1708\RtxLdap\TestCode\ADConfig.xml";
            //ADConfig adc = new ADConfig();
            //adc.AdminUser = "zqs";
            //adc.Password = "Asd123456";
            //adc.DoMainPath1 = "LDAP://192.168.31.134/";
            //adc.DoMainPath2 = "";
            //adc.DoMainPath3 = "DC=test,DC=com";
            //adc.SetDoMainPath();
            //CommonHelper.Helper.SerializeToXml(adc, filePath);
            //string comName = "南宁公司";
            //ADConfig adc= CommonHelper.Helper.DeserializeFromXML<ADConfig>(filePath);
            //adc.DoMainPath2 = "OU=" + comName + ",";
            //adc.SetDoMainPath();
            //CommonHelper.Helper.SerializeToXml(adc,filePath);
            StringBuilder postData = new StringBuilder();
            DomainUser user = new DomainUser();
            user.Name = "abc";
            user.Mail = "and@qq.com";
            user.GivenName = "343"; 
            Type type = typeof(DomainUser);
            PropertyInfo[] pros = type.GetProperties();
            foreach (var pro in pros)
            {
                //if (pro.GetValue(user, null) != null)
                    postData.Append("&").Append(pro.Name).Append("=").Append(pro.GetValue(user, null));
            }
            Console.WriteLine(postData.ToString().Trim('&'));
            Console.ReadKey();
        }
        static void Main5(string[] args)
        {
            OperateAD ado = new OperateAD();
            string path = "LDAP://192.168.31.134/OU=南宁总公司,DC=test,DC=com";
            string adminUser = "Administrator";
            string password = "Abc123456";
            DirectoryEntry entry = ado.GetEntry(path, adminUser, password);
            string filter = "(&(objectclass=organizationalUnit)(ou=财务部))";
            DirectoryEntry ouEntry = ado.GetOUEntry(entry, filter);
            if (ouEntry == null)
            {
                Console.WriteLine("要删除的组织单位不存在");
            }
            else
            {
                if (ado.DelEntry(ouEntry))
                {
                    Console.WriteLine("删除成功");
                }
                else
                {
                    Console.WriteLine("删除失败");
                }
            }
            Console.ReadKey();
        }
        static void Main2(string[] args)
        {
            OperateAD ado = new OperateAD();
            string path = "LDAP://192.168.31.134/OU=某单位,DC=test,DC=com";
            string adminUser = "Administrator";
            string password = "Abc123456";
            DirectoryEntry entry = ado.GetEntry(path, adminUser, password);
            string filter = "(&(objectclass=organizationalUnit)(ou=综合管理部))";
            DirectoryEntry ouEntry = ado.GetOUEntry(entry, filter);
            if (ado.AddOUEntry(ouEntry, "行政部"))
            {
                Console.WriteLine("组织单位添加成功");
            }
            else
            {
                Console.WriteLine("组织单位添加失败");
            }

            Console.ReadKey();
        }
        static void Main1(string[] args)
        {
            Type type = typeof(DomainUser);
            PropertyInfo[] pros = type.GetProperties();
            foreach (PropertyInfo pro in pros)
            {
                Console.WriteLine(pro.Name);
            }
            Console.ReadKey();
        }
        static void Main4(string[] args)
        {
            //Dictionary<string, string> dir = new Dictionary<string, string>();
            //dir.Add("name", "abs");
            //dir.Add("sAMAccountName", "abs");
            //dir.Add("userPrincipalName", "abs");
            //dir.Add("userPwd", "Asd123456");
            //dir.Add("displayName", "阿巴瑟");
            //dir.Add("mail", "aab@qq.com");
            //dir.Add("department", "行政部");
            DomainUser user = new DomainUser();
            user.Name = "leinuo";
            user.SAMAccountName = "leinuo";
            user.UserPrincipalName = "leinuo@test.com";
            user.UserPwd = "Asd654123";
            user.Mail = "abc@qq.com";
            user.Department = "综合管理部";
            user.TelephoneNumber = "15615615656";
            //AjaxResult result= Helper.SendMessage("http://192.168.31.134:8083/domainrtx/add", user);
            OperateAD ado = new OperateAD();
            string path = "LDAP://192.168.31.134/OU=某单位,DC=test,DC=com";
            string adminUser = "Administrator";
            string password = "Abc123456";
            DirectoryEntry entry = ado.GetEntry(path, adminUser, password);
            string filter = "(&(objectclass=organizationalUnit)(ou=综合管理部))";
            DirectoryEntry ouEntry = ado.GetOUEntry(entry, filter);
            DirectoryEntry userEntry = ado.GetUserEntry(entry, "leinuo");
            ado.UserMoveToOU(ouEntry, userEntry);
            if (ado.EditAccount(userEntry, user))
            {
                Console.WriteLine("成功");
            }
            else
            {
                Console.WriteLine("失败");
            }
            Console.ReadKey();
        }
        static void Main6(string[] args)
        {
            string filePath = @" F:\1708\RtxLdap\TestCode\ADConfig.xml";
            AdOperate ado = new AdOperate(filePath);
            DirectoryEntry entry = ado.GetEntry();
            RtxDeptManager dept = new RtxDeptManager();
            RtxUserManager user = new RtxUserManager();
            string filter = "objectclass=organizationalUnit";
            ado.OUEntrySyncRtx(entry, filter, dept);
            filter = "(&(objectCategory=person)(objectClass=user))";
            ado.UserEntrySyncRtx(entry, filter, user, dept);
            //if(ado.CheckADUser(path,"leinuo","Asd654123"))
            //{
            //    Console.WriteLine("测试成功");
            //}
            Console.WriteLine("测试成功");
            Console.ReadKey();
        }
        static void Main3(string[] args)
        {
            //AjaxResult res = Helper.SendMessage("http://192.168.31.134:8083/main/syncrtx");
            //if (res.Status == "ok")
            //{
            //    Console.WriteLine("测试成功"+res.Msg);
            //}
            //else
            //{
            //    Console.WriteLine("测试失败");
            //}
            string filePath = @" F:\1708\RtxLdap\TestCode\ADConfig.xml";
            AdOperate ado = new AdOperate(filePath);      
            DirectoryEntry entry = ado.GetEntry();
            if (ado.EditOUEntry(entry, "总公司"))
                Console.WriteLine("修改成功");
            else
                Console.WriteLine("修改失败");
            Console.ReadKey();
        }
    }
}
