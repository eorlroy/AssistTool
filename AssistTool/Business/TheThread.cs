using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssistTool.Business
{
    internal class TheThread
    {
        #region Thread
        private static void TaskInit()
        {
            //if (Caches.TheStack == null)
            //{
            //    Caches.TheStack = new Stack<Model.AccountConfig>();
            //    foreach (var config in Caches.AccountConfigDic)
            //    {
            //        Caches.TheStack.Push(config.Value);
            //    }
            //}
            if (Caches.ThreadConfigList == null)
            {
                Model.ThreadConfig threadConfig = null;
                Caches.ThreadConfigList = new ConcurrentBag<Model.ThreadConfig>();
                System.Threading.CancellationTokenSource cancellTokenSource = null;
                foreach (string key in Caches.AccountConfigDic.Keys)
                {
                    cancellTokenSource = new System.Threading.CancellationTokenSource();
                    threadConfig = new Model.ThreadConfig();
                    threadConfig.ActionOption = Main.StepNext(Model.ThreadConfig.EAction.none); 
                    threadConfig.Token = cancellTokenSource.Token;
                    threadConfig.Idle = true;
                }
            }
        }
        internal static void StartTask()
        {
            TaskInit();
            System.Threading.CancellationTokenSource cancellTokenSource = null;
            foreach (Model.ThreadConfig config in Caches.ThreadConfigList)
            {
                if (!config.Idle)
                {
                    continue;
                }
                else if (config.ActionOption == Model.ThreadConfig.EAction.end)
                {
                    continue;
                }
                else if (config.ActionOption == Model.ThreadConfig.EAction.refresh && DateTime.Compare(config.NextRunDate, DateTime.Now)>0)
                {
                    continue;
                }
                cancellTokenSource = new System.Threading.CancellationTokenSource();
                Task.Factory.StartNew(()=> ExecuteTask(config.ActionOption,config.ID), cancellTokenSource.Token);
                config.Token = cancellTokenSource.Token;
            }
            //Model.ThreadConfig threadConfig = null;
            //System.Threading.CancellationTokenSource cancellTokenSource = null;
            //foreach (string key in Caches.AccountConfigDic.Keys)
            //{
            //    cancellTokenSource = new System.Threading.CancellationTokenSource();
            //    threadConfig = new Model.ThreadConfig();
            //    threadConfig.ActionOption = Model.ThreadConfig.EAction.query;
            //    threadConfig.Token = cancellTokenSource.Token;
            //    Task.Factory.StartNew(() => {
            //        if(Model.ThreadConfig.EAction.end)

            //    }, cancellTokenSource.Token);
            //    //ChangeTaskConfig(key, eaction);
            //}
            //CancellationTokenSource CancellTokenSource = new CancellationTokenSource();
            //CancellTokenSource.Token;
        }

        internal static void StopTask()
        {

        }
        private static void ExecuteTask(Model.ThreadConfig.EAction eaction,string actionId)
        {
            Model.ThreadConfig config = Config.GetTaskConfig(actionId);
            switch (eaction)
            {
                case Model.ThreadConfig.EAction.login:
                    bool res = Main.Login(actionId);
                    if (res)
                    {
                        config.ActionOption = Main.StepNext(eaction);
                        config.Idle = false;
                    }
                    else
                    {
                        config.ActionOption = Model.ThreadConfig.EAction.end;
                        config.NextRunDate = DateTime.Now.AddYears(10);
                        config.CountTaskNum = 0;
                        config.FinishTaskNum = 0;
                        config.Idle = false;
                    }
                    break;
                case Model.ThreadConfig.EAction.query:
                    Main.QueryData(actionId);
                    break;
                case Model.ThreadConfig.EAction.submit:
                    Main.Submit(actionId);
                    break;
                case Model.ThreadConfig.EAction.notice:
                    Main.Notice(actionId);
                    break;
                case Model.ThreadConfig.EAction.refresh:
                    Main.RefreshCookie(actionId);
                    break;
            }
        }
        //private static Action<string> GetAction(Model.ThreadConfig.EAction eaction)
        //{
        //    Action<string> result = null;
        //    if (eaction == Model.ThreadConfig.EAction.login)
        //    {
        //        result = new Action<string>(Main.Login);
        //    }
        //    return result;
        //}
        //private static bool CanExecute(DateTime dateTime)
        //{
        //    bool result = Utils.GetDateTime() - dateTime > Caches.SystemConfig.WaitTime;
        //    return result;
        //}
        #endregion
    }
}
