using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AssistTool
{
    public partial class IndexForm : Form
    {
        Dictionary<string, Model.AccountConfig> dic = new Dictionary<string, Model.AccountConfig>();
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
                        IOHelper.CreateDir(Common.DATA_PATH);//内部有存在性判断
                        foreach (Model.AccountConfig config in acountConfigList)
                        {
                            if (!this.CheckAccountConfig(config))
                            {//如果信息有误,跳过
                                continue;
                            }
                            if (!dic.ContainsKey(Utils.MD5(config.Email.Trim())))
                            {//防止重复代码
                                dic.Add(Utils.MD5(config.Email.Trim()), config);
                            }
                        }
                        if (dic.Count <= 0)
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

                foreach (var item in dic)
                {
                    AccountComboBox.Items.Add(item.Value.Email);
                }
                AccountComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                this.ShowAndClose(ex.ToString());
            }
        }
        #region Event
        private void button1_Click(object sender, EventArgs e)
        {
            ActionButton.Text = string.Equals(ActionButton.Text, "开始") ? "停止":"开始";
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
