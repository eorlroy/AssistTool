using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssistTool.Business
{
    internal class TheThread
    {
        #region Thread
        public static void TaskInit()
        {
            Caches.TheStack = new Stack<Model.AccountConfig>();
            foreach (var config in Caches.AccountConfigDic)
            {
                Caches.TheStack.Push(config.Value);
            }
            if (Caches.ThreadConfigList == null)
            {
                Model.ThreadConfig threadConfig = null;
                Caches.ThreadConfigList = new List<Model.ThreadConfig>(0);
                System.Threading.CancellationTokenSource cancellTokenSource = null;
                foreach (string key in Caches.AccountConfigDic.Keys)
                {
                    cancellTokenSource = new System.Threading.CancellationTokenSource();
                    threadConfig = new Model.ThreadConfig();
                    threadConfig.ActionOption = Main.StepNext(Model.ThreadConfig.EAction.none); 
                    threadConfig.Token = cancellTokenSource.Token;
                }
            }
        }
        internal static void ActionTask()
        {
            TaskInit();
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

        public static void StopTask()
        {

        }
        #endregion
    }
}
