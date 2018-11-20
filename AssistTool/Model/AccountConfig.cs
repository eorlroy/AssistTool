using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistTool.Model
{
    public class AccountConfig
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public string Mid { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 接收数据地址
        /// </summary>
        public string DataUrl { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        public string SendUrl { get; set; }
        /// <summary>
        /// 已加密[0:否,1:是]
        /// </summary>
        public int EnCode { get; set; }
    }
}
