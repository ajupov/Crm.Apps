using System.Threading.Tasks;

namespace Crm.Infrastructure.SmsSending.SmsSender
{
    public interface ISmsSender
    {
        Task SendAsync(string phoneNumber, string message);
    }
}