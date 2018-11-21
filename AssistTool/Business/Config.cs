using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AssistTool.Business
{
    internal class Config
    {
        #region Config
        /// <summary>
        /// 校验账户信息
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static bool CheckAccountConfig(Model.AccountConfig config)
        {
            if (config == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.Email))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.Pwd))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.DataUrl))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(config.SendUrl))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 添加任务配置
        /// </summary>
        internal static void AddTaskConfig(Model.ThreadConfig config)
        {
            if (config != null
                && !string.IsNullOrWhiteSpace(config.ID)
                && !Caches.ThreadConfigList.Exists(item => string.Equals(item.ID, config.ID)))
            {//如果config有效且集合无重复值,添加集合&添加总线
                Caches.ThreadConfigList.Add(config);
                Caches.BusThreadConfig.CountTaskNum += config.CountTaskNum;
            }
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="eaction"></param>
        internal static void ChangeTaskConfig(string actionId, Model.ThreadConfig.EAction eaction)
        {
            ChangeTaskConfig(actionId, 0, 0, eaction, null);
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="Cookies"></param>
        internal static void ChangeTaskConfig(string actionId, CookieContainer Cookies)
        {
            ChangeTaskConfig(actionId, 0, 0, Model.ThreadConfig.EAction.none, Cookies);
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="finishTaskNum"></param>
        /// <param name="countTaskNum"></param>
        internal static void ChangeTaskConfig(string actionId, int finishTaskNum, int countTaskNum)
        {
            ChangeTaskConfig(actionId, finishTaskNum, countTaskNum, Model.ThreadConfig.EAction.none, null);
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="finishTaskNum"></param>
        /// <param name="countTaskNum"></param>
        internal static void ChangeTaskConfig(string actionId, int finishTaskNum, int countTaskNum, Model.ThreadConfig.EAction eaction, CookieContainer cookies)
        {
            Model.ThreadConfig config = Caches.ThreadConfigList.Find(item => item.ID == actionId);
            if (config != null && config != default(Model.ThreadConfig))//TOTEST
            {
                if (finishTaskNum > 0)
                {
                    Caches.BusThreadConfig.FinishTaskNum -= config.FinishTaskNum;//去掉旧值
                    Caches.BusThreadConfig.FinishTaskNum += finishTaskNum;//替换新值
                    config.FinishTaskNum = finishTaskNum;
                }
                if (countTaskNum > 0)
                {
                    Caches.BusThreadConfig.CountTaskNum -= config.CountTaskNum;//去掉旧值
                    Caches.BusThreadConfig.CountTaskNum += countTaskNum;//替换新值
                    config.CountTaskNum = countTaskNum;
                }
                if (cookies != null)
                {
                    config.Cookies = cookies;
                }
                if (eaction != Model.ThreadConfig.EAction.none)
                {
                    config.ActionOption = eaction;
                }
            }
        }
        #endregion
    }
}
