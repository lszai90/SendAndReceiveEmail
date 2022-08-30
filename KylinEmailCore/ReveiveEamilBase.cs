using KylinEmailCore.Dto;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KylinEmailCore
{

    /// <summary>
    /// 接收邮件
    /// </summary>
    public class ReveiveEamilBase
    {
        /// <summary>
        /// 存在本地地址
        /// </summary>
        public virtual string localfile { get; set; }
        /// <summary>
        /// 服务器 Smtp 服务器
        /// </summary>
        public virtual string smtpService { get; set; }
        /// <summary>
        /// 发送邮箱
        /// </summary>
        public virtual string sendEmail { get; set; }
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

        //实例化对象
        Pop3Client pop3;
        public ReveiveEamilBase()
        {
            smtpService = "pop.qiye.aliyun.com"; //"smtp.mxhichina.com.cn";//默认的Smtp服务
            sendEmail = "sgdsrm@cetccq.com.cn";// "cetccq_ic@vip.163.com";//发送邮箱
            sendpwd = "Sgd_Srm_12";//密码
            sendport = 110;//端口
            sendssl = false;
            localfile = "D:\\localfile";
            pop3 = new Pop3Client();
        }
        /// <summary>
        /// 接受邮件 Openpop
        /// 邮件接收：.NET中没有POP3邮件接收的类，邮件的内容和格式比复杂，
        /// 手动写代码进行解析很麻烦，也容易出错，开发中我们可以借助第三方插件来实现
        /// OpenPOP.NET插件的地址：http://sourceforge.net/projects/hpop/
        /// </summary>
        /// <returns></returns>
        public virtual List<ReveiveEamilBackDto> ReceiveEmail()
        {
            List<ReveiveEamilBackDto> reveiveEamilBackDtos = new List<ReveiveEamilBackDto>();
            try
            {
                //链接到邮件服务器
                pop3.Connect(smtpService, sendport, sendssl);
                //身份验证
                pop3.Authenticate(sendEmail, sendpwd);
                //读邮件列表
                //1.获取邮件的个数
                int count = pop3.GetMessageCount();
                //2.遍历显示出来
                for (int i = 1; i <= count; i++)
                {
                    Message msg = pop3.GetMessage(i);
                    string FromAddress = msg.Headers.From.Address;//发送者的邮箱地址
                    string FromDisplayName = msg.Headers.From.DisplayName;//发送者的名子
                    DateTime DateSent = msg.Headers.DateSent;//邮件的发送时间
                    string Subject = msg.Headers.Subject;//邮件的主题
                                                         //获取正文内容，其中包括\n\r这些换行符
                    string Body = String.Empty;
                    try
                    {
                        Body = msg.FindFirstPlainTextVersion().GetBodyAsText();
                    }
                    catch (Exception ex)
                    {
                    }

                    //获取邮件html内容
                    //OpenPop.Mime.MessagePart htmlMessage = msg.FindFirstHtmlVersion();
                    //string htmlText = htmlMessage.GetBodyAsText();
                    //只要有附件才添加到集合
                    List<MessagePart> messageParts = msg.FindAllAttachments();
                    if (messageParts != null)
                    {
                        if (messageParts.Count > 0)
                        {
                            ReveiveEamilBackDto reveiveEamilBackDto = new ReveiveEamilBackDto();
                            reveiveEamilBackDto.FromAddress = FromAddress;//发送者的邮箱
                            reveiveEamilBackDto.FromDisplayName = FromDisplayName;//发送者的名称
                            reveiveEamilBackDto.BodyAsText = Body;//文本
                            reveiveEamilBackDto.Subject = Subject;//title
                            reveiveEamilBackDto.DateSent = DateSent;//发送日期

                            //附件
                            reveiveEamilBackDto.fileLists = new List<FileList>();

                            foreach (MessagePart item in messageParts)
                            {
                                FileList fileList = new FileList();
                                //判断文件是否存在，不存在则创建的存在
                                string sPath = Path.Combine(localfile, "File");
                                if (!System.IO.Directory.Exists(sPath))
                                {
                                    //创建该文件
                                    System.IO.Directory.CreateDirectory(sPath);
                                }
                                FileInfo fileInfo = new FileInfo(sPath + "\\" + item.FileName);
                                //保存附件
                                item.Save(fileInfo);
                                //添加附件
                                fileList.DownDate = DateTime.Now;
                                fileList.FileName = item.FileName;
                                fileList.FilePath = fileInfo.FullName;
                                reveiveEamilBackDto.fileLists.Add(fileList);
                            }
                            reveiveEamilBackDtos.Add(reveiveEamilBackDto);
                        }
                    }
                    //暂时不需要删除 在测试阶段
                    pop3.DeleteMessage(i); //删除邮件 
                }
                //断开链接
                pop3.Disconnect();
            }
            catch (Exception ex)
            {

            }
            return reveiveEamilBackDtos;
        }
    }
}
