using AssistTool.Helper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AssistTool
{
    public partial class IndexForm : Form
    {
        //private const string BUTTON_ACTION_TXT = "开始";
        //private const string BUTTON_STOP_TXT = "停止";
        //private const string BUTTON_WAIT_TXT = "等待";
        //private static Model.SystemConfig _systemConfig = null;//系统配置
        //private static Stack<Model.AccountConfig> _stack = null;//任务堆栈
        //private static Model.ThreadConfig _busThreadConfig = new Model.ThreadConfig();//总线任务
        //private static List<Model.ThreadConfig> _threadConfigList = new List<Model.ThreadConfig>();//任务集合
        //private static Dictionary<string, Model.AccountConfig> _accountConfigDic = new Dictionary<string, Model.AccountConfig>();//常驻内存

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
                    Caches.SystemConfig = JsonHelper.DeserializeJsonToObject<Model.SystemConfig>(configTxt);
                    if (Caches.SystemConfig == null)
                    {
                        this.ShowAndClose("系统配置内容为空或加载失败!");
                    }
                    else if (Caches.SystemConfig.WaitTime <= 0)
                    {
                        Caches.SystemConfig.WaitTime = 10;
                    }
                    else if (string.IsNullOrWhiteSpace(Caches.SystemConfig.LoginUrl))
                    {
                        this.ShowAndClose("登录地址获取失败!");
                    }
                    else if (string.IsNullOrWhiteSpace(Caches.SystemConfig.UploadUrl))
                    {
                        this.ShowAndClose("上传数据地址获取失败!");
                    }
                    else if (string.IsNullOrWhiteSpace(Caches.SystemConfig.RefreshUrl))
                    {
                        this.ShowAndClose("刷新地址获取失败!");
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
                            if (!Business.Config.CheckAccountConfig(config))
                            {//如果信息有误,跳过
                                continue;
                            }
                            if (Caches.AccountConfigDic.ContainsKey(actionId))
                            {//防止重复代码

                                Caches.AccountConfigDic.Add(actionId, config);
                            }
                        }
                        if (Caches.AccountConfigDic.Count <= 0)
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
                foreach (var item in Caches.AccountConfigDic)
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
            ActionButton.Text = Caches.BUTTON_ACTION_TXT;
        }
        #region Event
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.Equals(ActionButton.Text, Caches.BUTTON_ACTION_TXT))
            {//执行任务
                ActionButton.Text = Caches.BUTTON_STOP_TXT;
                Caches.RunFlag = true;
                //Business.TheThread.ActionTask();
            }
            else
            {//执行停止任务
                ActionButton.Text = Caches.BUTTON_WAIT_TXT;
                Caches.RunFlag = false;
                //Business.TheThread.StopTask();
                //ActionButton.Text = Caches.BUTTON_ACTION_TXT;
            }
        }

        private void form_Closing(object sender, FormClosingEventArgs e)
        {
            if (!Caches.BusThreadConfig.Idle)
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
            else
            {
                e.Cancel = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Caches.RunFlag)
            {
                Business.TheThread.StartTask();
            }
            else
            {
                Business.TheThread.StopTask();
                ActionButton.Text = Caches.BUTTON_ACTION_TXT;
            }
        }
        /// <summary>
        /// 展示信息并关闭程序
        /// </summary>
        /// <param name="msg"></param>
        private void ShowAndClose(string msg)
        {
            MessageBox.Show(msg, "哎呦");
            this.CloseForm();
        }
        /// <summary>
        /// 关闭程序
        /// </summary>
        private void CloseForm()
        {
            Environment.Exit(0);
        }
        #endregion
    }
}
