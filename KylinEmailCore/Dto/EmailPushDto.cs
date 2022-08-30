using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KylinEmailCore.Dto
{
    /// <summary>
    /// 发送DTO
    /// </summary>
    public class EmailPushDto
    {
        /// <summary>
        /// 主送人
        /// </summary>
        public List<string> strReceiver { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        public List<string> strCReceiver { get; set; }
        /// <summary>
        /// 发送正文
        /// </summary>
        public string strContent { get; set; }
        /// <summary>
        /// 发送邮件主题
        /// </summary>
        public string strSubject { get; set; }
        /// <summary>
        /// 邮箱附件
        /// </summary>
        public Dictionary<string, string> AttachFile { get; set; }
    }
}
