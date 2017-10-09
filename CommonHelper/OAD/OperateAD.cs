using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper
{
    public class OperateAD
    {
        public DirectoryEntry GetEntry(string path, string adminUser, string password)
        {
            DirectoryEntry domain = new DirectoryEntry();
            try
            {
                domain.Path = path;
                domain.Username = adminUser;
                domain.Password = password;
                domain.AuthenticationType = AuthenticationTypes.Secure;
                domain.RefreshCache();
                return domain;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //启用账户
        public bool EnableAccount(DirectoryEntry userEntry)
        {
            try
            {
                int val = (int)userEntry.Properties["userAccountControl"].Value;
                userEntry.Properties["userAccountControl"].Value = val & ~0x2;
                userEntry.CommitChanges();
                userEntry.Close();
                //DomainUser.Success = "启用账户成功！";
                return true;
            }
            catch (Exception ex)
            {
                //DomainUser.Failed = ex.Message.ToString();
                return false;
            }
        }

        // 停用账号
        public bool DisableAccount(DirectoryEntry userEntry)
        {
            try
            {
                userEntry.Properties["userAccountControl"].Value = 0x2;
                userEntry.CommitChanges();
                userEntry.Close();
                //DomainUser.Success = "停用账户成功！";
                return true;
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                //DomainUser.Failed = ex.Message.ToString();
                return false;
            }
        }

        public bool UserMoveToOUDisable(DirectoryEntry moveToEntry, DirectoryEntry userEntry)
        {
            try
            {
                userEntry.MoveTo(moveToEntry);
                if (!DisableAccount(userEntry))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckADUser(string domainPath, string adminUser, string password)
        {
            try
            {
                DirectoryEntry domain = new DirectoryEntry(domainPath, adminUser, password);
                domain.AuthenticationType = AuthenticationTypes.Secure;
                domain.RefreshCache();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsADUserExist(DirectoryEntry entry, string userName)
        {
            using (DirectorySearcher search = new DirectorySearcher(entry))
            {
                search.Filter = "(sAMAccountName=" + userName + ")";
                //search.PropertiesToLoad.Add("cn"); //不指定加载查询属性，不会把属性查出来
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsADUserExist(string path, string adminUser, string password, string userName)
        {
            DirectoryEntry entry = GetEntry(path, adminUser, password);
            using (DirectorySearcher search = new DirectorySearcher(entry))
            {
                search.Filter = "(sAMAccountName=" + userName + ")";
                //search.PropertiesToLoad.Add("cn"); //不指定加载查询属性，不会把属性查出来
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    return false;
                }
                return true;
            }
        }
        public bool IsOUExist(string path, string adminUser, string password, string ouName)
        {
            DirectoryEntry entry = GetEntry(path, adminUser, password);
            using (DirectorySearcher search = new DirectorySearcher(entry))
            {
                search.Filter = "(ou=" + ouName + ")";
                //search.PropertiesToLoad.Add("cn"); //不指定加载查询属性，不会把属性查出来
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    return false;
                }
                return true;
            }
        }

        public bool UserMoveToOU(DirectoryEntry moveToEntry, DirectoryEntry userEntry)
        {
            try
            {
                userEntry.MoveTo(moveToEntry);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //返回组织单位地址
        public string OUEntryPath(DirectoryEntry objDE, string ouName)
        {
            using (DirectorySearcher objSearcher = new DirectorySearcher(objDE, "ou=" + ouName))
            {
                SearchResult src = objSearcher.FindOne();
                return src.Path;
            }
        }


        public DirectoryEntry GetUserEntry(string doMainPath, string adminUser, string userName, string password)
        {
            DirectoryEntry userPath = GetEntry(doMainPath, adminUser, password);
            using (DirectorySearcher objSearcher = new DirectorySearcher(userPath, "(&(objectCategory=person)(objectClass=user)(cn=" + userName + "))"))
            {
                SearchResult src = objSearcher.FindOne();
                if (src != null)
                {
                    return src.GetDirectoryEntry();
                }
                return null;
            }
        }

        public DirectoryEntry GetUserEntry(DirectoryEntry entry, string userName)
        {
            using (DirectorySearcher objSearcher = new DirectorySearcher(entry, "(&(objectCategory=person)(objectClass=user)(cn=" + userName + "))"))
            {
                SearchResult src = objSearcher.FindOne();
                if (src != null)
                {
                    return src.GetDirectoryEntry();
                }
                return null;
            }
        }

        public string OUEntryPathForFilter(DirectoryEntry objDE, string filter, string rootOUName)
        {
            using (DirectorySearcher objSearcher = new DirectorySearcher(objDE, filter))
            {
                SearchResultCollection srcs = objSearcher.FindAll();
                string path = null;
                foreach (SearchResult src in srcs)
                {
                    if (src.Path.Contains(rootOUName))
                    {
                        path = src.Path;
                    }
                }
                return path;
            }
        }

        public DirectoryEntry GetOUEntry(string path, string adminUser, string password, string filter)
        {
            DirectoryEntry ouEntry = GetEntry(path, adminUser, password);
            using (DirectorySearcher objSearcher = new DirectorySearcher(ouEntry, filter))
            {
                SearchResult src = objSearcher.FindOne();
                if (src != null)
                {
                    return src.GetDirectoryEntry();
                }
                return null;
            }
        }

        public DirectoryEntry GetOUEntry(DirectoryEntry ouEntry, string filter)
        {
            using (DirectorySearcher objSearcher = new DirectorySearcher(ouEntry, filter))
            {
                SearchResult src = objSearcher.FindOne();
                if (src != null)
                {
                    return src.GetDirectoryEntry();
                }
                return null;
            }
        }

        //返回组地址
        public string GroupEntryPath(DirectoryEntry objDE, string groupName)
        {
            using (DirectorySearcher objSearcher = new DirectorySearcher(objDE, "cn=" + groupName))
            {
                SearchResult src = objSearcher.FindOne();
                return src.Path;
            }
        }

        public bool AddAccount(DirectoryEntry entry, DomainUser user)
        {
            try
            {
                DirectoryEntry NewUser = entry.Children.Add("CN=" + user.Name, "user");
                NewUser.Properties["sAMAccountName"].Add(user.Name); //account
                NewUser.Properties["userPrincipalName"].Value = user.UserPrincipalName; //user logon name,xxx@bdxy.com
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
                NewUser.CommitChanges();
                //设置密码
                //反射调用修改密码的方法（注意端口号的问题  端口号会引起方法调用异常）
                NewUser.Invoke("SetPassword", new object[] { user.UserPwd });
                //默认设置新增账户启用
                NewUser.Properties["userAccountControl"].Value = 0x200;
                NewUser.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EditAccount(DirectoryEntry userEntry, DomainUser user)
        {            
            if (userEntry == null)
            {
                return false;
            }
            //User.Properties["sAMAccountName"].Add(user.Name); //account
            //User.Properties["userPrincipalName"].Value = user.UserPrincipalName; //user logon name,xxx@bdxy.com
            if (!string.IsNullOrEmpty(user.Company))
            {
                userEntry.Properties["company"].Value = user.Company;
            }
            if (!string.IsNullOrEmpty(user.Department))
            {
                userEntry.Properties["department"].Value = user.Department;
            }
            if (!string.IsNullOrEmpty(user.Description))
            {
                userEntry.Properties["description"].Value = user.Description;
            }
            if (!string.IsNullOrEmpty(user.DisplayName))
            {
                userEntry.Properties["displayName"].Value = user.DisplayName;
            }
            if (!string.IsNullOrEmpty(user.GivenName))
            {
                userEntry.Properties["givenName"].Value = user.GivenName;
            }
            if (!string.IsNullOrEmpty(user.Initials))
            {
                userEntry.Properties["initials"].Value = user.Initials;
            }
            if (!string.IsNullOrEmpty(user.Mail))
            {
                userEntry.Properties["mail"].Value = user.Mail;
            }
            //if (!string.IsNullOrEmpty(user.Name))
            //{
            //    userEntry.Properties["name"].Value = user.Name;
            //}
            if (!string.IsNullOrEmpty(user.PhysicalDeliveryOfficeName))
            {
                userEntry.Properties["physicalDeliveryOfficeName"].Value = user.PhysicalDeliveryOfficeName;
            }
            if (!string.IsNullOrEmpty(user.SN))
            {
                userEntry.Properties["sn"].Value = user.SN;
            }
            if (!string.IsNullOrEmpty(user.TelephoneNumber))
            {
                userEntry.Properties["telephoneNumber"].Value = user.TelephoneNumber;
            }
            try
            {
                userEntry.CommitChanges();
                //设置密码
                //反射调用修改密码的方法（注意端口号的问题  端口号会引起方法调用异常）
                userEntry.Invoke("SetPassword", new object[] { user.UserPwd });
                //默认设置新增账户启用
                //User.Properties["userAccountControl"].Value = 0x200;
                userEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EditUserEntry(DirectoryEntry userEntry, DomainUser user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Company))
                {
                    userEntry.Properties["company"].Value = user.Company;
                }
                if (!string.IsNullOrEmpty(user.Department))
                {
                    userEntry.Properties["department"].Value = user.Department;
                }
                if (!string.IsNullOrEmpty(user.Description))
                {
                    userEntry.Properties["description"].Value = user.Description;
                }
                if (!string.IsNullOrEmpty(user.DisplayName))
                {
                    userEntry.Properties["displayName"].Value = user.DisplayName;
                }
                if (!string.IsNullOrEmpty(user.GivenName))
                {
                    userEntry.Properties["givenName"].Value = user.GivenName;
                }
                if (!string.IsNullOrEmpty(user.Initials))
                {
                    userEntry.Properties["initials"].Value = user.Initials;
                }
                if (!string.IsNullOrEmpty(user.Mail))
                {
                    userEntry.Properties["mail"].Value = user.Mail;
                }
                if (!string.IsNullOrEmpty(user.PhysicalDeliveryOfficeName))
                {
                    userEntry.Properties["physicalDeliveryOfficeName"].Value = user.PhysicalDeliveryOfficeName;
                }
                if (!string.IsNullOrEmpty(user.SN))
                {
                    userEntry.Properties["sn"].Value = user.SN;
                }
                if (!string.IsNullOrEmpty(user.TelephoneNumber))
                {
                    userEntry.Properties["telephoneNumber"].Value = user.TelephoneNumber;
                }
                userEntry.CommitChanges();
                //设置密码
                //反射调用修改密码的方法（注意端口号的问题  端口号会引起方法调用异常）
                if (!string.IsNullOrEmpty(user.UserPwd))
                {
                    userEntry.Invoke("SetPassword", new object[] { user.UserPwd });
                    userEntry.CommitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddOUEntry(DirectoryEntry entry,string ouName)
        {
            try
            {
                DirectoryEntry newOUEntry = entry.Children.Add("OU="+ouName, "organizationalUnit");
                newOUEntry.CommitChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool DelEntry(DirectoryEntry entry)
        {
            try
            {
                entry.DeleteTree();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //....        
    }
}
