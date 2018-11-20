namespace AssistTool
{
    public class Common
    {
        
        /// <summary>
        /// 根目录
        /// </summary>
        public static readonly string ROOT_DIRECTORY = System.Environment.CurrentDirectory;
        /// <summary>
        /// 系统配置文件
        /// </summary>
        public static readonly string SYSTEM_CONFIG = ROOT_DIRECTORY + "\\System.json";
        /// <summary>
        /// 账户配置文件
        /// </summary>
        public static readonly string ACCOUNT_CONFIG = ROOT_DIRECTORY + "\\Account.json";
        /// <summary>
        /// 数据目录
        /// </summary>
        public static readonly string DATA_PATH = ROOT_DIRECTORY + "\\.data";
    }
}
