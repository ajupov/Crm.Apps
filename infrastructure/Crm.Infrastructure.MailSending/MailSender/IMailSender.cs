using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Infrastructure.MailSending.MailSender
{
    public interface IMailSender
    {
        Task SendAsync(string fromName, string fromAddress, string subject, IEnumerable<string> toAddresses,
            bool isBodyHtml, string body);
    }
}