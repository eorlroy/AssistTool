using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AssistTool
{
    public partial class IndexForm : Form
    {
        private const string BUTTON_ACTION_TXT = "开始";
        private const string BUTTON_STOP_TXT = "停止";
        private const string BUTTON_WAIT_TXT = "等待";
        private static Model.ThreadConfig busThreadConfig = new Model.ThreadConfig();//总线任务
        private static List<Model.ThreadConfig> threadConfigList = new List<Model.ThreadConfig>();//不需要线程安全的
        Dictionary<string, Model.AccountConfig> accountConfigDic = new Dictionary<string, Model.AccountConfig>();

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
                            if (!accountConfigDic.ContainsKey(actionId))
                            {//防止重复代码

                                accountConfigDic.Add(actionId, config);
                            }
                        }
                        if (accountConfigDic.Count <= 0)
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
                    this.ShowAndClose("账户文件不存在!");
                }
                #endregion

                foreach (var item in accountConfigDic)
                {
                    AccountComboBox.Items.Add(item.Value.Email);
                }
                AccountComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                this.ShowAndClose(ex.ToString());
            }
            ActionButton.Text = BUTTON_ACTION_TXT;
        }
        #region Event
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.Equals(ActionButton.Text, BUTTON_ACTION_TXT))
            {//执行任务
                
                ActionButton.Text = BUTTON_STOP_TXT;
            }
            else
            {//执行停止任务
                ActionButton.Text = BUTTON_WAIT_TXT;

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
        /// 添加任务
        /// </summary>
        private static void AddTaskConfig(Model.ThreadConfig config)
        {
            if (config != null 
                && !string.IsNullOrWhiteSpace(config.ID) 
                && !threadConfigList.Exists(item=>string.Equals(item.ID, config.ID)))
            {//如果config有效且集合无重复值,添加集合&添加总线
                
            }
        }
        /// <summary>
        /// 展示信息并关闭程序
        /// </summary>
        /// <param name="msg"></param>
        private void ShowAndClose(string msg)
        {
            MessageBox.Show(msg);
            this.CloseForm();
        }
        /// <summary>
        /// 关闭程序
        /// </summary>
        private void CloseForm()
        {
            Environment.Exit(0);
        }

    }
}
