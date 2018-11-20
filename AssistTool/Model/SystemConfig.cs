namespace AssistTool.Model
{
    public class SystemConfig
    {
        /// <summary>
        /// 等待时间[分钟]
        /// </summary>
        public int WaitTime { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// 上传地址
        /// </summary>
        public string UploadUrl { get; set; }
    }
}
