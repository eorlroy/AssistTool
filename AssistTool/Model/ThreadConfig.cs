using System.Collections;

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
        /// 是否空闲
        /// </summary>
        public bool Idle { get; set; }
        /// <summary>
        /// 消息池
        /// </summary>
        public Queue Message { get; set; }
    }
}
