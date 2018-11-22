using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AssistTool
{
    internal static class Caches
    {
        internal static bool RunFlag = false;
        internal const string BUTTON_ACTION_TXT = "开始";
        internal const string BUTTON_STOP_TXT = "停止";
        internal const string BUTTON_WAIT_TXT = "等待";
        internal static Model.SystemConfig SystemConfig = null;//系统配置
        internal static Stack<Model.AccountConfig> TheStack = null;//任务堆栈
        internal static Model.ThreadConfig BusThreadConfig = null;//总线任务
        internal static ConcurrentBag<Model.ThreadConfig> ThreadConfigList = null;//任务集合
        internal static Dictionary<string, Model.AccountConfig> AccountConfigDic = new Dictionary<string, Model.AccountConfig>();//常驻内存
    }
}
