using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public interface IEmail
    {
        Task SendEmail(IEnumerable<string> recipients, string subject, string body);
    }
}
