using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace AssistTool
{
    public partial class IndexForm : Form
    {
        private const string BUTTON_ACTION_TXT = "开始";
        private const string BUTTON_STOP_TXT = "停止";
        private const string BUTTON_WAIT_TXT = "等待";
        private static Model.SystemConfig _systemConfig = null;//系统配置
        private static Stack<Model.AccountConfig> _stack = null;//任务堆栈
        private static Model.ThreadConfig _busThreadConfig = new Model.ThreadConfig();//总线任务
        private static List<Model.ThreadConfig> _threadConfigList = new List<Model.ThreadConfig>();//不需要线程安全的
        private static Dictionary<string, Model.AccountConfig> _accountConfigDic = new Dictionary<string, Model.AccountConfig>();//常驻内存

        public IndexForm()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            try
            {
                //this.MaximizeBox = false;
                //this.MinimizeBox = false;
                #region 初始化系统配置
                //systemConfig = 
                if (IOHelper.Exists(Common.SYSTEM_CONFIG))
                {
                    string configTxt = IOHelper.Read(Common.SYSTEM_CONFIG);
                    _systemConfig = JsonHelper.DeserializeJsonToObject<Model.SystemConfig>(configTxt);
                    if (_systemConfig == null)
                    {
                        this.ShowAndClose("系统配置内容为空或加载失败!");
                    }
                    else if (_systemConfig.WaitTime <= 0)
                    {
                        _systemConfig.WaitTime = 10;
                    }
                    else if (string.IsNullOrWhiteSpace(_systemConfig.LoginUrl))
                    {
                        this.ShowAndClose("登录地址获取失败!");
                    }
                    else if (string.IsNullOrWhiteSpace(_systemConfig.UploadUrl))
                    {
                        this.ShowAndClose("上传数据地址获取失败!");
                    }
                }
                else
                {
                    this.ShowAndClose("系统配置文件不存在!");
                }
                #endregion
                #region 初始化账户配置
                if (IOHelper.Exists(Common.ACCOUNT_CONFIG))
                {
                    string configTxt = IOHelper.Read(Common.ACCOUNT_CONFIG);
                    List<Model.AccountConfig> acountConfigList = JsonHelper.DeserializeJsonToObject<List<Model.AccountConfig>>(configTxt);
                    if (acountConfigList != null)
                    {
                        string actionId = null;
                        IOHelper.CreateDir(Common.DATA_PATH);//内部有存在性判断
                        foreach (Model.AccountConfig config in acountConfigList)
                        {
                            actionId = Utils.MD5(config.Email.Trim());
                            if (!this.CheckAccountConfig(config))
                            {//如果信息有误,跳过
                                continue;
                            }
                            if (!_accountConfigDic.ContainsKey(actionId))
                            {//防止重复代码

                                _accountConfigDic.Add(actionId, config);
                            }
                        }
                        if (_accountConfigDic.Count <= 0)
                        {
                            this.ShowAndClose("未检测到有效账户数据!");
                        }
                    }
                    else
                    {
                        this.ShowAndClose("账户内容为空或加载失败!");
                    }
                }
                else
                {
                    this.ShowAndClose("账户配置文件不存在!");
                }
                #endregion
                #region 装载页面
                foreach (var item in _accountConfigDic)
                {
                    AccountComboBox.Items.Add(item.Value.Email);
                }
                AccountComboBox.SelectedIndex = 0;
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowAndClose(ex.ToString());
            }
            ActionButton.Text = BUTTON_ACTION_TXT;
        }
        private static void TaskInit()
        {

        }
        #region 业务
        /// <summary>
        /// 查询数据
        /// </summary>
        private static void QueryData(string actionId)
        {

        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        private static bool Login(string actionId)
        {
            bool result = false;
            try
            {
                Model.AccountConfig config = _accountConfigDic[actionId];
                string userName = config.Email.Trim();
                string userPasspwd = config.EnCode > 0 ? config.Pwd : Utils.MD5(config.Pwd);
                string timestamp = GetTimeStamp(DateTime.Now);
                string encrypt_type = "02";
                string url = _systemConfig.LoginUrl;
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
                string resultPost = PostLogin(parames, url, ref cookies);
                if (resultPost != null)
                {
                    Dictionary<string, string> dic = JsonHelper.DeserializeJsonToObject<Dictionary<string, string>>(resultPost);
                    if (dic != null && dic.Keys.Contains("status") && string.Equals((dic["status"] ?? string.Empty).Trim(),"00"))
                    {
                        result = true;
                        ChangeTaskConfig(actionId, cookies);
                    }
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 刷新,保持Cookie
        /// </summary>
        private static void RefreshCookie(string actionId)
        {

        }
        /// <summary>
        /// 提交数据
        /// </summary>
        private static void Submit(string actionId)
        {

        }
        /// <summary>
        /// 通知执行情况
        /// </summary>
        private static void SendMsg(string actionId)
        {

        }
        public static string PostLogin(string postData, string requestUrlString, ref CookieContainer cookie)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            //向服务端请求
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrlString);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.Accept = "application/json, text/javascript, */*; q=0.01";
            myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:32.0) Gecko/20100101 Firefox/32.0";
            myRequest.ContentLength = data.Length;
            myRequest.CookieContainer = new CookieContainer();
            Stream newStream = myRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            //将请求的结果发送给客户端(界面、应用)
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            cookie.Add(myResponse.Cookies);
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            return reader.ReadToEnd();
        }
        #endregion
        private static void ActionTask()
        {
            _stack = new Stack<Model.AccountConfig>();
            foreach (var config in _accountConfigDic)
            {
                _stack.Push(config.Value);
            }
            Model.ThreadConfig threadConfig = null;
            System.Threading.CancellationTokenSource cancellTokenSource = new System.Threading.CancellationTokenSource();
            foreach (string key in _accountConfigDic.Keys)
            {
                threadConfig = new Model.ThreadConfig();
                //Task.Factory.StartNew(()=>,);
            }
            //CancellationTokenSource CancellTokenSource = new CancellationTokenSource();
            //CancellTokenSource.Token;
        }
        //private static void 
        private static void StopTask()
        {

        }
        #region Event
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.Equals(ActionButton.Text, BUTTON_ACTION_TXT))
            {//执行任务
                ActionButton.Text = BUTTON_STOP_TXT;
                ActionTask();
            }
            else
            {//执行停止任务
                ActionButton.Text = BUTTON_WAIT_TXT;
                StopTask();
                ActionButton.Text = BUTTON_ACTION_TXT;
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("有事务在执行,你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        #endregion
        private bool CheckAccountConfig(Model.AccountConfig config)
        {
            if(config == null)
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
        private static void AddTaskConfig(Model.ThreadConfig config)
        {
            if (config != null 
                && !string.IsNullOrWhiteSpace(config.ID) 
                && !_threadConfigList.Exists(item=>string.Equals(item.ID, config.ID)))
            {//如果config有效且集合无重复值,添加集合&添加总线
                _threadConfigList.Add(config);
                _busThreadConfig.CountTaskNum += config.CountTaskNum;
            }
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="Cookies"></param>
        private static void ChangeTaskConfig(string actionId, CookieContainer Cookies)
        {
            ChangeTaskConfig(actionId, 0, 0,Cookies);
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="finishTaskNum"></param>
        /// <param name="countTaskNum"></param>
        private static void ChangeTaskConfig(string actionId, int finishTaskNum, int countTaskNum)
        {
            ChangeTaskConfig(actionId, finishTaskNum, countTaskNum, null);
        }
        /// <summary>
        /// 修改任务配置
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="finishTaskNum"></param>
        /// <param name="countTaskNum"></param>
        private static void ChangeTaskConfig(string actionId, int finishTaskNum,int countTaskNum, CookieContainer cookies)
        {
            Model.ThreadConfig config = _threadConfigList.Find(item=>item.ID == actionId);
            if (config!= null && config != default(Model.ThreadConfig))//TOTEST
            {
                if (finishTaskNum > 0)
                {
                    _busThreadConfig.FinishTaskNum -= config.FinishTaskNum;//去掉旧值
                    _busThreadConfig.FinishTaskNum += finishTaskNum;//替换新值
                    config.FinishTaskNum = finishTaskNum;
                }
                if (countTaskNum > 0)
                {
                    _busThreadConfig.CountTaskNum -= config.CountTaskNum;//去掉旧值
                    _busThreadConfig.CountTaskNum += countTaskNum;//替换新值
                    config.CountTaskNum = countTaskNum;
                }
                if (cookies != null)
                {
                    config.Cookies = cookies;
                }
            }
        }
        /// <summary>
        /// 展示信息并关闭程序
        /// </summary>
        /// <param name="msg"></param>
        private void ShowAndClose(string msg)
        {
            MessageBox.Show(msg,"失败");
            this.CloseForm();
        }
        /// <summary>
        /// 关闭程序
        /// </summary>
        private void CloseForm()
        {
            Environment.Exit(0);
        }
        #region 时间戳
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        private static long ConvertDateTimeToInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }
        #endregion

    }
}
