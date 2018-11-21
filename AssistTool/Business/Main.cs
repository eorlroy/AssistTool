using AssistTool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AssistTool.Business
{
    internal class Main
    {
        #region 业务
        /// <summary>
        /// 查询数据
        /// </summary>
        internal static void QueryData(string actionId)
        {

        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        internal static bool Login(string actionId)
        {
            bool result = false;
            try
            {
                Model.AccountConfig config = Caches.AccountConfigDic[actionId];
                string userName = config.Email.Trim();
                string userPasspwd = config.EnCode > 0 ? config.Pwd : Utils.MD5(config.Pwd);
                string timestamp = Utils.GetTimeStamp(DateTime.Now);
                string encrypt_type = "02";
                string url = Caches.SystemConfig.LoginUrl;
                string parames = string.Concat(
                    "userName=", userName,
                    "&passpwd=", userPasspwd,
                    "&randomCode=",
                    "&emailCode=",
                    "&secondFalg=",
                    "&timestamp=", timestamp,
                    "&encrypt_type=", encrypt_type
                    );
                CookieContainer cookies = new CookieContainer();
                Helper.HttpHelper.Header header = new Helper.HttpHelper.Header();
                header.Accept = "application/json, text/javascript, */*; q=0.01";
                header.ContentType = "application/x-www-form-urlencoded";
                string resultPost = Helper.HttpHelper.Post(url, parames, header, Helper.HttpHelper.ECookie.get, ref cookies);//PostLogin(parames, url, ref cookies);
                if (resultPost != null)
                {
                    Dictionary<string, string> dic = Helper.JsonHelper.DeserializeJsonToObject<Dictionary<string, string>>(resultPost);
                    if (dic != null && dic.Keys.Contains("status") && string.Equals((dic["status"] ?? string.Empty).Trim(), "00"))
                    {
                        result = true;
                        Config.ChangeTaskConfig(actionId, cookies);
                    }
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 刷新,保持Cookie
        /// </summary>
        internal static void RefreshCookie(string actionId)
        {
            Model.ThreadConfig config = Caches.ThreadConfigList.Find(item => item.ID == actionId);
            if (config != null)
            {
                string url = Caches.SystemConfig.RefreshUrl;
                CookieContainer cookie = config.Cookies;
                HttpHelper.Post(url, null, HttpHelper.ECookie.use, ref cookie);
            }
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        internal static void Submit(string actionId)
        {
            Model.ThreadConfig config = Caches.ThreadConfigList.Find(item => item.ID == actionId);

        }
        /// <summary>
        /// 通知执行情况
        /// </summary>
        internal static void Notice(string actionId)
        {

        }
        /// <summary>
        /// 规划执行步骤
        /// </summary>
        /// <param name="eaction"></param>
        internal static void StepNext(ref Model.ThreadConfig.EAction eaction)
        {
            //Model.ThreadConfig.EAction result = Model.ThreadConfig.EAction.none;
        }
        #endregion
    }
}
