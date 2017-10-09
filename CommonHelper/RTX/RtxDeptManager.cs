using RTXSAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper
{
    public class RtxDeptManager
    {
        RTXSAPILib.RTXSAPIRootObj RootObj;  //声明一个根对象
        RTXSAPILib.RTXSAPIDeptManager DeptManager;

        public RtxDeptManager()
        {
            RootObj = new RTXSAPIRootObj();     //创建根对象            
            RootObj.ServerIP = "127.0.0.1";
            RootObj.ServerPort = Convert.ToInt16("8006");
            DeptManager = RootObj.DeptManager;
        }

        public bool AddUserToDept(string bstrUserName, string bstrSrcDeptName, string bstrDestDeptName, bool bIsCopy)
        {
            try
            {
                DeptManager.AddUserToDept(bstrUserName, bstrSrcDeptName, bstrDestDeptName, bIsCopy);
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        } 

        public bool IsDeptExist(string bstrDeptName)
        {
            return DeptManager.IsDeptExist(bstrDeptName);
        }

        public bool AddDept(string bstrDeptName, string bstrParentDept)
        {
            try
            {
                DeptManager.AddDept(bstrDeptName, bstrParentDept);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveDept(string bstrDeptName)
        {
            try
            {
                DeptManager.DelDept(bstrDeptName, false);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DelDept(string bstrDeptName)
        {
            try
            {
                DeptManager.DelDept(bstrDeptName, true);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetUserDeptsName(string userName)
        {
            try
            {
                string[] paths = DeptManager.GetUserDepts(userName).Split('"');
                return paths[1];
            }
            catch (Exception ex)
            {                
                return null;
            }
        }
    }
}
