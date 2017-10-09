using RTXSAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper
{
    public class RtxUserManager
    {
        RTXSAPILib.RTXSAPIRootObj RootObj;  //声明一个根对象
        RTXSAPILib.IRTXSAPIUserManager UserManager;

        public RtxUserManager()
        {
            RootObj = new RTXSAPIRootObj();     //创建根对象            
            RootObj.ServerIP = "127.0.0.1";
            RootObj.ServerPort = Convert.ToInt16("8006");
            UserManager = RootObj.UserManager;
        }

        public bool IsUserExist(string bstrUserName)
        {            
             return UserManager.IsUserExist(bstrUserName);
        }

        public bool AddRtxUser(string bstrUserName, int IAuthType)
        {
            try
            {
                UserManager.AddUser(bstrUserName, IAuthType);
                return true;
            }
            catch (Exception ex)
            {                
                return false;
            }
        }

        public bool RemoveUser(string userName)
        {
            try
            {
                UserManager.DeleteUser(userName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetBasicRtxUser(string bstrUserName, string bstrName = "RTX_NULL", int gender = -1, string bstrMobile = "RTX_NULL", string bstrEMail = "RTX_NULL", string bstrPhone = "RTX_NULL", int IAuthType = -1)
        {
            try
            {
                UserManager.SetUserBasicInfo(bstrUserName, bstrName, gender, bstrMobile, bstrEMail, bstrPhone, IAuthType);

                return true;
            }
            catch (Exception ex)
            {               
                return false;
            }
        }
        
        public bool AddUserBasic(DomainUser user,string path, int IAuthType)
        {
            try
            {
                UserManager.AddUser(user.Name,IAuthType);
                string userName = user.Name;
                string displayName = "RTX_NULL";
                int gender = -1;
                string mobile = "RTX_NULL";
                string email = "RTX_NULL";
                string phone = "RTX_NULL";
                if (string.IsNullOrEmpty(user.DisplayName))
                {
                    displayName = user.DisplayName;
                }
                if (string.IsNullOrEmpty(user.Mail))
                {
                    email = user.Mail;
                }
                if (string.IsNullOrEmpty(user.TelephoneNumber))
                {
                    mobile = user.TelephoneNumber;
                    phone = mobile;
                }
                if (string.IsNullOrEmpty(user.Initials))
                {
                    gender = Convert.ToInt32(user.Initials);
                }
                UserManager.SetUserBasicInfo(userName, displayName, gender, mobile, email, phone, IAuthType);
                RtxDeptManager rdm = new RtxDeptManager();
                rdm.AddUserToDept(userName, null, path, false);
                return true;
            }
            catch(Exception ex)
            {
                return false; 
            }
        }
                
        //////////
    }
}
