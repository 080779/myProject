using System;
using System.Collections.Generic;
using System.Text;
using RTXSAPILib;
using System.Diagnostics;

namespace CommonHelper
{

    class RTX
    {
        RTXSAPILib.RTXSAPIRootObj RootObj; //声明一个根对象
        RTXSAPILib.RTXSAPIUserAuthObj UserAuthObj; //声明一个用户认证对象
        private ADConfig ADC { get; set; }
        public string FilePath { get; set; }

        public void CreateRoot()
        {
            RootObj = new RTXSAPILib.RTXSAPIRootObj(); //创建根对象
            UserAuthObj = RootObj.UserAuthObj;//通过根对象创建用户认证对象
            UserAuthObj.OnRecvUserAuthRequest += new _IRTXSAPIUserAuthObjEvents_OnRecvUserAuthRequestEventHandler(UserAuthObj_OnRecvUserAuthRequest); //订阅用户认证响应事件
            RootObj.ServerIP = "127.0.0.1";
            RootObj.ServerPort = Convert.ToInt16("8006");
            UserAuthObj.AppGUID = "{8E85315D-342C-417d-9093-57F824638040}"; //设置应用GUID
            UserAuthObj.AppName = "RTX_LDAP"; //设置应用名
        }

        public void RegApp()
        {
            CreateRoot();
            try
            {
                UserAuthObj.RegisterApp();  //注册应用
                EventLog.WriteEntry("RTX", "注册应用成功", EventLogEntryType.Information, 8811);//系统日志
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RTX", "注册应用失败：" + ex.Message, EventLogEntryType.Error, 8801);//系统日志
            }
        }
        public void UnregApp()
        {
            CreateRoot();
            try
            {
                UserAuthObj.UnRegisterApp();  //注销应用
                EventLog.WriteEntry("RTX", "注消应用成功", EventLogEntryType.Information, 8812);//系统日志
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RTX", "注消应用失败：" + ex.Message, EventLogEntryType.Error, 8802);//系统日志
            }
        }
        public void StartApp()
        {
            CreateRoot();
            try
            {
                UserAuthObj.StartApp("", 8);  //启动应用
                EventLog.WriteEntry("RTX", "应用启动成功", EventLogEntryType.Information, 8813);//系统日志
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RTX", "应用启动失败：" + ex.Message, EventLogEntryType.Error, 8803);//系统日志
            }
        }
        public void StopApp()
        {
            CreateRoot();
            try
            {
                UserAuthObj.StopApp();  //停止应用
                EventLog.WriteEntry("RTX", "应用停止成功", EventLogEntryType.Information, 8814);//系统日志
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("RTX", "应用停止失败：" + ex.Message, EventLogEntryType.Error, 8804);//系统日志
            }
        }
        public void UserAuthObj_OnRecvUserAuthRequest(string bstrUserName, string bstrPwd, out RTXSAPI_USERAUTH_RESULT pResult)
        {
            ADC = Helper.DeserializeFromXML<ADConfig>(FilePath);
            string path=null;
            if (ADC != null)
            {
                path= ADC.DoMainPath;
            }
            AdOperate ado = new AdOperate();
            //bool login = ldap.IsAuthenticated(FrmRtxLdapPlugin.dc, bstrUserName, bstrPwd);
            bool login = ado.CheckADUser(path, bstrUserName, bstrPwd);
            if (login)
            {
                pResult = RTXSAPI_USERAUTH_RESULT.RTXSAPI_USERAUTH_RESULT_OK;//设置认证成功，客户端将正常登录
                //RTX_LDAP.WriteLog.LogManager.WriteLog(RTX_LDAP.WriteLog.LogFile.Trace, "用户登录成功：" + bstrUserName);//写入日志到文件
                //EventLog.WriteEntry("RTX", "用户登录成功：" + bstrUserName, EventLogEntryType.Information, 8815);//系统日志
            }

            else
            {
                pResult = RTXSAPI_USERAUTH_RESULT.RTXSAPI_USERAUTH_RESULT_ERRNOUSER;//设置认证失败，客户端弹出相应提示
                //RTX_LDAP.WriteLog.LogManager.WriteLog(RTX_LDAP.WriteLog.LogFile.Error, "用户登录失败：" + bstrUserName);//写入日志到文件
                //EventLog.WriteEntry("RTX", "用户登录失败：" + bstrUserName, EventLogEntryType.Error, 8805);//系统日志
            }

        }
        public void GetUserType(string username)
        {
            CreateRoot();
            bool BAuthType;
            try
            {
                BAuthType = UserAuthObj.QueryUserAuthType(username);

                //if (BAuthType == true)
                //    MessageBox.Show(username + " 的认证方式为第三方认证");
                //else
                //    MessageBox.Show(username + " 的认证方式为RTX系统本地认证");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        public void SetUserType(string username, string txttype)
        {
            CreateRoot();
            Int32 type = 0;
            if (txttype == "本地认证")
            {
                type = 1;
            }
            else
            {
                type = 0;
            }
            bool authType = false;
            if (type == 0)
                authType = true;
            else if (type == 1)
                authType = false;
            try
            {
                UserAuthObj.SetUserAuthType(username, authType);
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
