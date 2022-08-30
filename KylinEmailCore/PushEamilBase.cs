using KylinEmailCore.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KylinEmailCore
{
    /// <summary>
    /// 抽象方法
    /// </summary>
    public abstract class PushEamilBase
    {
        /// <summary>
        /// 服务器 Smtp 服务器
        /// </summary>
        public virtual string smtpService { get; set; }
        /// <summary>
        /// 发送邮箱
        /// </summary>
        public virtual string sendEmail { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string displayName { get; set; }
        /// <summary>
        /// 发送密码
        /// </summary>
        public virtual string sendpwd { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public virtual int sendport { get; set; }

        /// <summary>
        /// 是否使用ssl加密
        /// </summary>
        public virtual bool sendssl { get; set; }


        //确定smtp服务器地址 实例化一个Smtp客户端
        SmtpClient smtpclient = null;//实例化smtp服务器
        /// <summary>
        /// 构造函数注入必要的参数
        /// </summary>
        protected PushEamilBase()
        {
            smtpService = "smtp.xxxxx.com"; ////默认的Smtp服务
            sendEmail = "xxxxx";///发送邮箱
            sendpwd = "xxxx";//密码
            sendport = 25;
            displayName = "显示名称";//Kylin<ls_za@163.com> displayName 就是显示的Kylin
            sendssl = false;
            this.smtpclient = new SmtpClient();
        }


        //没有采用方式
        public virtual EmailSendBackDto SendEmail(EmailPushDto emailPushDto)
        {
            EmailSendBackDto emailSendBackDto = new EmailSendBackDto();
            smtpclient.Host = smtpService;
            smtpclient.Port = sendport;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(sendEmail, displayName);
            //主送人
            if (emailPushDto.strReceiver != null)
            {
                for (int i = 0; i < emailPushDto.strReceiver.Count; i++)
                {
                    if (emailPushDto.strReceiver[i] != "")
                        mailMessage.To.Add(emailPushDto.strReceiver[i]);
                }
            }
            //抄送人
            if (emailPushDto.strCReceiver != null)
            {
                for (int i = 0; i < emailPushDto.strCReceiver.Count; i++)
                {
                    if (emailPushDto.strCReceiver[i] != "")
                        mailMessage.CC.Add(emailPushDto.strCReceiver[i]);
                }
            }

            mailMessage.Subject = emailPushDto.strSubject;//发送邮件主题
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

            mailMessage.Body = emailPushDto.strContent;//发送邮件正文 

            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            //附件
            if (emailPushDto.AttachFile != null)
            {
                foreach (string key in emailPushDto.AttachFile.Keys)
                {
                    Attachment file = new Attachment(emailPushDto.AttachFile[key]);
                    file.Name = key;
                    mailMessage.Attachments.Add(file);
                }
            }
            //邮件发送方式  通过网络发送到smtp服务器
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

            //如果服务器支持安全连接，则将安全连接设为true
            smtpclient.EnableSsl = sendssl;
            try
            {
                //是否使用默认凭据，若为false，则使用自定义的证书，就是下面的networkCredential实例对象
                smtpclient.UseDefaultCredentials = false;
                //指定邮箱账号和密码,需要注意的是，这个密码是你在QQ邮箱设置里开启服务的时候给你的那个授权码
                NetworkCredential networkCredential = new NetworkCredential(sendEmail, sendpwd);
                smtpclient.Credentials = networkCredential;
                //发送邮件
                //LogManager.WriteLog("SAI发送邮件:" + mailMessage.ToJson());
                smtpclient.Send(mailMessage);
                emailSendBackDto.IsSuccess = true;
                // to do  发送成功

            }
            catch (System.Net.Mail.SmtpException ex)
            {
                // LogManager.WriteLog("SAI发送邮箱提示错误:" + ex.Message);
                emailSendBackDto.IsSuccess = false;
                emailSendBackDto.Msg = ex.Message;
                // to do  发送失败
            }
            return emailSendBackDto;
        }

        //采用html发送方式
        public virtual EmailSendBackDto SendHtmlEmail(EmailPushDto emailPushDto)
        {
            EmailSendBackDto emailSendBackDto = new EmailSendBackDto();
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式   
            //如果服务器支持安全连接，则将安全连接设为true
            smtpclient.EnableSsl = sendssl;
            smtpclient.Host = smtpService;//邮件服务器
            smtpclient.UseDefaultCredentials = true;
            smtpclient.Credentials = new NetworkCredential(sendEmail, sendpwd);//用户名、密码 
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(sendEmail, displayName);
            if (emailPushDto.strReceiver != null)
            {
                for (int i = 0; i < emailPushDto.strReceiver.Count; i++)
                {
                    mailMessage.To.Add(emailPushDto.strReceiver[i]);
                }
            }
            if (emailPushDto.strCReceiver != null)
            {
                for (int i = 0; i < emailPushDto.strCReceiver.Count; i++)
                {
                    mailMessage.CC.Add(emailPushDto.strCReceiver[i]);
                }
            }
            mailMessage.Subject = emailPushDto.strSubject;//邮件标题   


            mailMessage.Body = emailPushDto.strContent;//发送邮件正文 

            //mailMessage.Body = emailPushDto.strContent;//邮件内容   
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            mailMessage.IsBodyHtml = true;//是否是HTML邮件   
            mailMessage.Priority = MailPriority.High;//邮件优先级 
            //附件
            if (emailPushDto.AttachFile != null)
            {
                foreach (string key in emailPushDto.AttachFile.Keys)
                {
                    Attachment file = new Attachment(emailPushDto.AttachFile[key]);
                    file.Name = key;
                    mailMessage.Attachments.Add(file);
                }
            }
            try
            {
                smtpclient.Send(mailMessage);
                emailSendBackDto.IsSuccess = true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                emailSendBackDto.IsSuccess = true;
                emailSendBackDto.Msg = ex.Message;
            }
            return emailSendBackDto;
        }


    }
}
