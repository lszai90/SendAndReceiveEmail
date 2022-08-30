using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KylinEmailCore.Dto
{
    public class EmailSendBackDto
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 错误原因
        /// </summary>
        public string Msg { get; set; }
    }
}
