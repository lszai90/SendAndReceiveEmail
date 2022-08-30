using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KylinEmailCore.Dto
{
    /// <summary>
    /// 接收邮件对象
    /// </summary>
    public class ReveiveEamilBackDto
    {
        /// <summary>
        /// 发送者的邮箱地址
        /// </summary>
        public string FromAddress { get; set; }
        /// <summary>
        /// 发送者的名
        /// </summary>
        public string FromDisplayName { get; set; }
        /// <summary>
        /// 邮件的发送时间
        /// </summary>
        public DateTime? DateSent { get; set; }
        /// <summary>
        /// 邮件的主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        public string BodyAsText { get; set; }
        /// <summary>
        /// 附件类库
        /// </summary>
        public List<FileList> fileLists { get; set; }
    }

    /// <summary>
    /// 附件类
    /// </summary>
    public class FileList
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 下载时间
        /// </summary>
        public DateTime? DownDate { get; set; }
    }
}
