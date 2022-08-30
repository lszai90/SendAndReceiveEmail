using KylinEmailCore.Dto;
using KylinEmailCore.implement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KylinSendAndReceiveEmail
{
    public partial class FrmEmail : Form
    {
        public FrmEmail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSend_Click(object sender, EventArgs e)
        {
            PushEamilCommon pushEamilCommon = new PushEamilCommon();
            EmailPushDto emailPushDto = new EmailPushDto();
            //附件
            emailPushDto.AttachFile = new Dictionary<string, string>();
            //to do 装载附件
            //  emailPushDto.AttachFile.Add();
            emailPushDto.strContent = "发送内容";
            emailPushDto.strSubject = "发送主题";
            //接收人
            emailPushDto.strReceiver = new List<string>();
            // to do  装载接收人
            //emailPushDto.strReceiver.Add();
            //抄送人
            emailPushDto.strCReceiver = new List<string>();
            // emailPushDto.strCReceiver.Add();
            pushEamilCommon.SendEmail(emailPushDto);
        }

        /// <summary>
        /// 接收邮件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceive_Click(object sender, EventArgs e)
        {
            ReveiveEamilCommon reveiveEamilCommon = new ReveiveEamilCommon();
            List<ReveiveEamilBackDto> reveiveEamilBackDtos = reveiveEamilCommon.ReceiveEmail();
            if (reveiveEamilBackDtos.Count > 0)
            {
                // 成功
            }
            else
            {
                //失败
            }
        }
    }
}
