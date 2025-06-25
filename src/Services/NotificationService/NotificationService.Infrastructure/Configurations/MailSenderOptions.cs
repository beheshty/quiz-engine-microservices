using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Configurations
{
    public class MailSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}
