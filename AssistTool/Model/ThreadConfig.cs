using System.Collections;
using System.Net;
using System.Threading;

namespace AssistTool.Model
{
    public class ThreadConfig
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public int DateTime { get; set; }
        /// <summary>
        /// 总任务数
        /// </summary>
        public int CountTaskNum { get; set; }
        /// <summary>
        /// 当前任务数
        /// </summary>
        public int FinishTaskNum { get; set; }
        /// <summary>
        /// 任务项
        /// </summary>
        public EAction ActionOption { get; set; }
        /// <summary>
        /// 闲置
        /// </summary>
        public bool Idle { get; set; }
        /// <summary>
        /// 线程令牌
        /// </summary>
        public CancellationToken Token { get; set; }
        /// <summary>
        /// 消息池
        /// </summary>
        public Queue Message { get; set; }
        /// <summary>
        /// Cookie值
        /// </summary>
        public CookieContainer Cookies { get; set; }
        public enum EAction
        {
            none,//无(置空时使用)
            login,//登录
            refresh,//刷新
            query,//获取数据
            submit,//上传
            notice,//通知
            end//结束
        }
    }
}
