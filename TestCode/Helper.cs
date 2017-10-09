using CommonHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TestCode
{
    public class Helper
    {
        public static AjaxResult SendMessage(string url, Dictionary<string, string> formFields)
        {
            WebClient wc = new WebClient();
            StringBuilder postData = new StringBuilder();
            foreach (var data in formFields)
            {
                postData.Append("&").Append(data.Key).Append("=").Append(data.Value);
            }
            byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString().Trim('&'));
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("ContentLength", sendData.Length.ToString());
            byte[] recData = wc.UploadData(url, "POST", sendData);
            string resp = Encoding.UTF8.GetString(recData);
            //AjaxResult result = JsonConvert.DeserializeObject<AjaxResult>(resp);
            JavaScriptSerializer js = new JavaScriptSerializer();
            AjaxResult result = js.Deserialize<AjaxResult>(resp);
            return result;
        }
        public static AjaxResult SendMessage(string url)
        {
            WebClient wc = new WebClient();
            string postData = "";
            byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString().Trim('&'));
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("ContentLength", sendData.Length.ToString());
            byte[] recData = wc.UploadData(url, "POST", sendData);
            string resp = Encoding.UTF8.GetString(recData);
            AjaxResult result = JsonConvert.DeserializeObject<AjaxResult>(resp);
            return result;
        }
        public static AjaxResult SendMessage(string url, DomainUser user)
        {
            WebClient wc = new WebClient();
            StringBuilder postData = new StringBuilder();
            Type type = typeof(DomainUser);
            PropertyInfo[] pros = type.GetProperties();
            foreach (var pro in pros)
            {
                postData.Append("&").Append(pro.Name).Append("=").Append(pro.GetValue(user,null));
            }
            //Console.WriteLine(postData.ToString().Trim('&'));
            byte[] sendData = Encoding.UTF8.GetBytes(postData.ToString().Trim('&'));
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("ContentLength", sendData.Length.ToString());
            byte[] recData = wc.UploadData(url, "POST", sendData);
            string resp = Encoding.UTF8.GetString(recData);
            AjaxResult result = JsonConvert.DeserializeObject<AjaxResult>(resp);
            return result;
        }
    }
}
