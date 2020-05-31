using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.HelperModels
{
    public class MailSendInfo
    {
        public string Email { set; get; }
        public string Subject { set; get; }
        public string Body { set; get; }
        public string AttachmentPath { set; get; }
    }
}
